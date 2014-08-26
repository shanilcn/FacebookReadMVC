using Facebook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookReadMVC.Models
{
    public class GroupPostPreview
    {
        public GroupPostPreview(JToken jData)
        {
            JObject jFromObject = JObject.Parse(jData["from"].ToString());
            JObject jActionsObject = JObject.Parse(jData["actions"].First().ToString());
            name = (string)jFromObject["name"]; ;
            link = (string)jActionsObject["link"]; ;
            message = (string)jData["message"];
            created_time = (string)jData["created_time"];

            if (message == null)
            {
                message = "\n" + name + ":   [link]: " + link;
            }
            else
            {
                message = "\n" + name + ":   [link]: " + link + "\n\n[clipped message]: " + message.Substring(0, message.Count() < 100 ? message.Count() - 1 : 100);
            }
        }
        public string name { get; set; }
        public string message { get; set; }
        public string link { get; set; }
        private string created_time { 
            get 
            { 
                return DateTimeConvertor.ToUnixTime(createdTime).ToString();
            }
            set
            {
                createdTime = DateTimeConvertor.FromUnixTime(value);
            }
        }
        public DateTime createdTime { get; set; }
    }   

}