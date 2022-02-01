using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RatinAPI.Function
{
    class RatingModel
    {
        [BsonElement("Id")]
        public string Id {get;set;} = Guid.NewGuid().ToString();
        [BsonElement("TimeStamp")]
[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime TimeStamp {get;set;} = DateTime.UtcNow;

         [BsonElement("UserId")]

        public string UserId {get;set;}
        [BsonElement("ProductId")]

         public string ProductId {get;set;}

[BsonElement("LocationName")]

          public string LocationName {get;set;}
[BsonElement("UserNotes")]

           public string UserNotes {get;set;}
           [BsonElement("Rating")]

        public int Rating {get;set;}

    }
}