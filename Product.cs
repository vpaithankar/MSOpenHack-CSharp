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
    public class Product
    {

        public string productId {get;set;}
        public string productName {get;set;}

        public string productDescription {get;set;}
    }
}