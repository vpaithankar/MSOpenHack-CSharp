using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RatinAPI.Function
{
    class RatingModel
    {
        
        public string Id {get;set;} = Guid.NewGuid().ToString();
        public DateTime TimeStamp {get;set;} = DateTime.UtcNow;

        public string UserId {get;set;}

         public string ProductId {get;set;}

          public string LocationName {get;set;}

           public string UserNotes {get;set;}
        public int Rating {get;set;}

    }
}