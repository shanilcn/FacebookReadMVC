using Facebook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookReadMVC.Models
{
    public class GroupPost
    {
        public GroupPost(JToken jData)
        {
            JObject jFromObject = new JObject();
            GetName(jData, jFromObject);
            GetOtherFields(jData);
            GetMessageTags(jData);
        }
        
        private void GetName(JToken jData, JObject jFromObject)
        {
            if (jData["from"] == null)
            {
                name = (string)jData["id"].ToString();
            }
            else
            {
                jFromObject = JObject.Parse(jData["from"].ToString());
                name = (string)jFromObject["name"];
            }
        }
        
        private void GetOtherFields(JToken jData)
        {
            message = (string)jData["message"];
            created_time = (string)jData["created_time"];
            type = (string)jData["type"];
            objectId = (string)jData["object_id"];
        }

        private void GetMessageTags(JToken jData)
        {
            if (jData["message_tags"] != null)
            {
                foreach (JToken token in jData["message_tags"])
                {
                    MessageTag msgTag = new MessageTag();
                    msgTag.offset = Convert.ToInt32(token["offset"]);
                    msgTag.length = Convert.ToInt32(token["length"]);
                    messageTags.Add(msgTag);
                }
            }
        }              

        public string name { get; set; }
        public string message { get; set; }
        public string objectId { get; set; }
        public string type { get; set; }
        public List<MessageTag> messageTags = new List<MessageTag>();
        private string created_time
        {
            get
            {
                return DateTimeConvertor.ToUnixTime(createdTime).ToString();
            }
            set
            {
                createdTime = DateTime.Parse(value);
            }
        }
        public DateTime createdTime { get; set; }


    }

    public class MessageTag
    {
        public int offset { get; set; }
        public int length { get; set; }
    }
    public class ImageObject
    {
        public short Height { get; set; }
        public short Width { get; set; }
        public string Src { get; set; }
    }
       
}