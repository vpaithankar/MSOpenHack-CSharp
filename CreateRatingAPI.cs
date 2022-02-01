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

    public static class CreateRatingAPI
    {
        [FunctionName("CreateRatingAPI")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            IActionResult returnValue = null;

            log.LogInformation("Creating a new contact");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic input = JsonConvert.DeserializeObject(requestBody);

            var newRating = new RatingModel
            {
                UserId = input.userId,
                ProductId = input.productId,
                LocationName = input.locationName,
                Rating = input.rating,
                UserNotes = input.userNotes
            };
            
            return new OkObjectResult(newRating);
        }
    }
}
