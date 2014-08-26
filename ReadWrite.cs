using Facebook;
using FacebookReadMVC.App_Start;
using FacebookReadMVC.Models;
using FacebookReadMVC.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FacebookReadMVC
{
    public static class ReadWrite
    {
        public static readonly GroupReadContext context = new GroupReadContext();
        //public static void PostRetrieval(FacebookClient fb)
        //{
        //    DateTime queryStartDate = new DateTime(2014, 8, 5);
        //    //DateTime queryEndDate = queryStartDate;
        //    string unixQueryStartDate;
        //    //string unixQueryEndDate;

        //    while (queryStartDate <= DateTime.Now.AddMinutes(-30))
        //    {
        //        unixQueryStartDate = "since=" + DateTimeConvertor.ToUnixTime(queryStartDate).ToString();
        //        //queryStartDate = queryEndDate;
        //        //queryEndDate = queryEndDate.AddMinutes(15);
        //        //unixQueryEndDate = "until=" + Infrastructure.DateTimeToUnixTimestamp(queryEndDate);

        //        dynamic me = fb.Get("/v2.1/791206814252173/feed?limit=25&" + unixQueryStartDate);
        //        JObject jPostObject = JObject.Parse(me.ToString());

        //        List<string> posts = ExtractPosts(fb, jPostObject);
        //        FBPreviewPost(fb, posts);
        //        queryStartDate = GetLastPostTimeInQuery(jPostObject);
        //    }

        //}


        public static void PostRetrieval(FacebookClient fb, BackupPost post)
        {
            JObject jPostObject = GetFeedJObject(fb, post.FBPostId);
            GroupPost groupPost = ExtractPost(fb, jPostObject);
            MapBackUpPost(fb, post, groupPost);

            GetPostComments(fb, post);
            context.Posts.Insert(post);
        }

        private static void GetPostComments(FacebookClient fb, BackupPost post)
        {
            string afterUri = string.Empty;
            while (true)
            {
                string afterCode = String.Empty;
                JObject jCommentsObject = GetFeedJObject(fb, post.FBPostId + "/comments?limit=25" + afterUri);
                List<BackupComment> comments = ExtractComments(jCommentsObject);

                if (comments.Count == 0)
                {
                    break;
                }
                post.Comments.AddRange(comments);

                afterCode = ExtractAfterUri(jCommentsObject);
                afterUri = "&after=" + afterCode;
            }
        }

        private static void MapBackUpPost(FacebookClient fb, BackupPost post, GroupPost groupPost)
        {
            post.Author = groupPost.name;
            post.Message = groupPost.message;
            post.CreatedTime = groupPost.createdTime;
            post.Type = groupPost.type;
            post.MessageTags = groupPost.messageTags;

            if (post.Type == "photo")
            {
                post.ImgObj = SavePhoto(fb, post.FBPostId, groupPost.objectId);
            }
        }

        private static ImageObject SavePhoto(FacebookClient fb, string sourceId, string objId)
        {
            string fileName = sourceId + "_1.jpg";
            string saveLocation = Settings.Default.FileLocation + fileName;
                        
            JObject imageSrcObject = GetFeedJObject(fb, objId + "?fields=images");
            List<ImageObject> imageSources = new List<ImageObject>();
            int LargestImg = 0;
            foreach (var token in imageSrcObject["images"])
            {
                ImageObject imgSrc = new ImageObject();
                imgSrc.Height = Convert.ToInt16(token["height"]);
                imgSrc.Width = Convert.ToInt16(token["width"]);
                imgSrc.Src = token["source"].ToString();
                if (imgSrc.Height > LargestImg)
                {
                    LargestImg = imgSrc.Height;
                }
                imageSources.Add(imgSrc);
            }

            WebClient webClient = new WebClient();
            ImageObject imgObj = imageSources.Find(i => i.Height == LargestImg);
            webClient.DownloadFile(imgObj.Src, saveLocation);
            imgObj.Src = saveLocation;
            return imgObj;
        }

        private static JObject GetFeedJObject(FacebookClient fb, string query)
        {
            dynamic feed = fb.Get("/v2.1/" + query);
            return JObject.Parse(feed.ToString());
        }

        private static string ExtractAfterUri(JObject jObject)
        {
            string afterCode = jObject["paging"]["cursors"]["after"].ToString();
            return afterCode;
        }

        //private static DateTime GetLastPostTimeInQuery(JObject jPostObject)
        //{
        //    DateTime lastCreateTime;
        //    GroupPostPreview lastPost = new GroupPostPreview(jPostObject["data"].Last());
        //    if (lastPost != null)
        //        lastCreateTime = lastPost.createdTime;
        //    else
        //        lastCreateTime = DateTime.MaxValue;

        //    return lastCreateTime;
        //}

        //private static DateTime GetLastCommentTimeInQuery(JObject jCommentObject)
        //{
        //    DateTime lastCreateTime;
        //    Comments lastComment = new Comments(jCommentObject["data"].Last());
        //    if (lastComment != null)
        //        lastCreateTime = lastComment.createdTime;
        //    else
        //        lastCreateTime = DateTime.MaxValue;

        //    return lastCreateTime;
        //}

        private static GroupPost ExtractPost(FacebookClient fb, JObject jPostObject)
        {
            string post = string.Empty;            
            JToken token = jPostObject; //jPostObject["data"];
            GroupPost groupPost = new GroupPost(token);
            return groupPost;
        }

        //private static List<string> ExtractPosts(FacebookClient fb, JObject jPostObject)
        //{
        //    string post = string.Empty;
        //    List<string> posts = new List<string>();
        //    foreach (JToken token in jPostObject["data"])
        //    {
        //        GroupPostPreview groupPost = new GroupPostPreview(token);
        //        post += groupPost.message + "\n\n\n";
        //        posts.Add(post);
        //    }
        //    return posts;
        //}

        //private static string FBPost(FacebookClient fb, GroupPost groupPost)
        //{
        //    var parameters = new Dictionary<string, object>();

        //    parameters["message"] = groupPost.message;
        //    //parameters["picture"] = groupPost.picture;
        //    //parameters["type"] = groupPost.type;
        //    //dynamic post = fb.Post("/v2.1/170020719862093/feed", parameters);  // AdminReserve
        //    dynamic postInfo = fb.Post("/v2.1/256859081119610/feed", parameters);  // ShanilTest

        //    //dynamic post = fb.Post("/v2.1/256859081119610/feed", parameters);  // ShanilTest
        //    //dynamic post = fb.Post("/v2.1/695439857195272/comments", parameters);//TFT Post 695439857195272     
        //    return postInfo.id;
        //}

        //private static void FBPreviewPost(FacebookClient fb, List<string> posts)
        //{
        //    var parameters = new Dictionary<string, object>();
        //    foreach (string post in posts)
        //    {
        //        parameters["message"] = post;
        //        //dynamic post = fb.Post("/v2.1/170020719862093/feed", parameters);  // AdminReserve
        //        dynamic postInfo = fb.Post("/v2.1/256859081119610/feed", parameters);  // ShanilTest

        //        //dynamic post = fb.Post("/v2.1/256859081119610/feed", parameters);  // ShanilTest
        //        //dynamic post = fb.Post("/v2.1/695439857195272/comments", parameters);//TFT Post 695439857195272                    
        //    }
        //}

        //private static void FBPostComments(FacebookClient fb, List<string> comments, string postId)
        //{
        //    var parameters = new Dictionary<string, object>();
        //    foreach (string comment in comments)
        //    {
        //        try
        //        {
        //            parameters["message"] = comment;
        //            dynamic postInfo = fb.Post("/v2.1/" + postId + "/comments", parameters);
        //        }
        //        catch (Exception)
        //        {

        //        }

        //    }
        //}

        private static List<BackupComment> ExtractComments(JObject jCommentsObject)
        {
            List<BackupComment> comments = new List<BackupComment>();
            foreach (JToken token in jCommentsObject["data"])
            {
                Comments postComment = new Comments(token);
                BackupComment comment = new BackupComment();
                comment.Message = postComment.message;
                comment.Name = postComment.name;
                comment.CreatedTime = postComment.createdTime;

                comments.Add(comment);
            }
            return comments;
        }

    }
}