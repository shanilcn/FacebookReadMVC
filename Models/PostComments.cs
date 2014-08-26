using Facebook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookReadMVC.Models
{
    public class Comments
    {
        public Comments(JToken jData)
        {
            JObject jFromObject = new JObject();
            if (jData["from"] == null)
            {
                name = (string)jData["id"].ToString();
            }
            else
            {
                jFromObject = JObject.Parse(jData["from"].ToString());
                name = (string)jFromObject["name"];
            }
            message = (string)jData["message"];
            created_time = (string)jData["created_time"];                     
        }

        public string name { get; set; }
        public string message { get; set; }
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
}