using FacebookReadMVC.Models;
using FacebookReadMVC.Properties;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookReadMVC.App_Start
{
    public class GroupReadContext
    {
        public MongoDatabase db;
        public GroupReadContext()
        {
            var client = new MongoClient(Settings.Default.GroupReadConnectionString);
            var server = client.GetServer();
            db = server.GetDatabase(Settings.Default.GroupReadDB);
        }

        public MongoCollection<BackupPost> Posts
        {
            get
            {
                return db.GetCollection<BackupPost>("posts");
            }
        }
    }
}