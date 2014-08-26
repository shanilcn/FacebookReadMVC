using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FacebookReadMVC.Models
{
    public class BackupPost
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {get; set;}
        public string PostId { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
        [BsonIgnore ]
        public string MessageHTML
        {
            get
            { 
                return Message.Replace("\n", "<br/>"); 
            }
        }
        [DisplayName("Title")]
        public string MessageDisplay { get; set; }
        public ImageObject ImgObj { get; set; }
        public string Type { get; set; }

        [DisplayName("FB Post Id")]
        public string FBPostId { get; set; }

        [DisplayName("Posted Time")]    
        [BsonDateTimeOptions(Kind= DateTimeKind.Local)]
        public DateTime? CreatedTime { get; set; }
        public List<BackupComment> Comments = new List<BackupComment>();
        public List<MessageTag> MessageTags = new List<MessageTag>();
        public List<SubjectTag> Tags  { get; set; }
    }

    public class BackupComment
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public List<MessageTag> MessageTags = new List<MessageTag>();

        [BsonIgnore] 
        public string MessageHTML
        {
            get
            { 
                return Message.Replace("\n", "<br/>"); 
            }
        }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreatedTime { get; set; }
    }     

    public class SubjectTag
    {
        public string Subject { get; set; }
    }
    
}