using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Azure.Cosmos;
using MongoDB.Driver;

namespace RatinAPI.Function
{

    public class CreateRatingAPI
    {
        private MongoClient mongoClient;
        
            //private  Database ratingDatabase;
            //private  Container dbcontainer;
        public CreateRatingAPI()
        {
           

        }
        [FunctionName("CreateRatingAPI")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            //IActionResult returnValue = null;

            log.LogInformation("Creating a new user rating");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic input = JsonConvert.DeserializeObject(requestBody);

            if(string.IsNullOrEmpty(input.userId.ToString()) || string.IsNullOrEmpty(input.productId.ToString()))
                return new OkObjectResult("Please enter UserId and ProductId");
            
            HttpClient client1 = new HttpClient();
            HttpRequestMessage request1 = new HttpRequestMessage(HttpMethod.Get,string.Format("https://serverlessohapi.azurewebsites.net/api/GetProduct?productId={0}",input.productId));

            HttpResponseMessage response1 = await client1.SendAsync(request1);
            //string responseContent = await new StreamReader(response1.Content).ReadToEndAsync();
            
            if(response1.IsSuccessStatusCode)
            {
                Product product = JsonConvert.DeserializeObject<Product>(response1.Content.ReadAsStringAsync().Result);
                  if(product==null)
            {   
                return new OkObjectResult("Invalid Product");
            }
            }

            HttpRequestMessage request2 = new HttpRequestMessage(HttpMethod.Get,string.Format("https://serverlessohapi.azurewebsites.net/api/GetUser?userId={0}",input.userId));

            HttpResponseMessage response2 = await client1.SendAsync(request2);
            //string responseContent = await new StreamReader(response1.Content).ReadToEndAsync();
            
            if(response2.IsSuccessStatusCode)
            {
                User user = JsonConvert.DeserializeObject<User>(response2.Content.ReadAsStringAsync().Result);
                  if(user==null)
            {   
                return new OkObjectResult("Invalid User");
            }
            }
            
            

           
            if(input.rating<0 || input.rating>5)       
                 return new OkObjectResult("Please enter Rating only between 0 and 5");

            
            string connectionString = 
  @"mongodb://dbohicecreamratings:YDjwWB1PW7C4lJjKxS2s4LDY99BVocbqErOV36YKTQ56eDYI2hJg5ASV1G9qFovTzjuTE8dnWCO2JH8XEP5T9A==@dbohicecreamratings.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@dbohicecreamratings@";
MongoClientSettings settings = MongoClientSettings.FromUrl(
  new MongoUrl(connectionString)
);
settings.SslSettings = 
  new SslSettings() { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
var mongoClient = new MongoClient(settings);
            //mongoClient = new MongoClient("mongodb://dbohicecreamratings:YDjwWB1PW7C4lJjKxS2s4LDY99BVocbqErOV36YKTQ56eDYI2hJg5ASV1G9qFovTzjuTE8dnWCO2JH8XEP5T9A==@dbohicecreamratings.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@dbohicecreamratings@");
            //var dbcontainer= mongoClient.GetDatabase("ratings_test","");
            var ratingDatabase= mongoClient.GetDatabase("icecreamratings");
            var dbcontainer= ratingDatabase.GetCollection<RatingModel>("ratings_test");
            var newRating = new RatingModel
            {
                UserId = input.userId,
                ProductId = input.productId,
                LocationName = input.locationName,
                Rating = input.rating,
                UserNotes = input.userNotes
            };
            
             try
            {
                await dbcontainer.InsertOneAsync(newRating);         
                return new OkObjectResult(newRating);
            }
            catch (Exception ex)
            {
                log.LogError($"Creating new contact failed. Exception thrown: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            //return new OkObjectResult(newRating);
        }
    }
}
