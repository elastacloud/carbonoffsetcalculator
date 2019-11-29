using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Elastacloud.CarbonCalculator;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace CarbonCaptureFunctions
{
   public static class CalculateCarbon
   {
      [FunctionName("CalculateCarbon")]
      public static async Task<IActionResult> Run(
          [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
          [Blob("carbon", Connection = "StorageConnectionString")] CloudBlobContainer outputContainer,
          ILogger log)
      {
         log.LogInformation("C# HTTP trigger function processed a request.");

         string monthIn = req.Query["month"];
         string yearIn = req.Query["year"];
         string application = req.Query["application"];

         if (string.IsNullOrEmpty(monthIn) || string.IsNullOrEmpty(yearIn))
         {
            return new BadRequestObjectResult("Query string must include month and year");
         }

         int month;
         if (!int.TryParse(monthIn, out month))
         {
            return new BadRequestObjectResult("Query month must be an integer");
         }

         int year;
         if (!int.TryParse(yearIn, out year))
         {
            return new BadRequestObjectResult("Query year must be an integer");
         }

         string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

         var coreCalculator = new CoreCalculator();

         try
         {
            var carbonResponses = coreCalculator.Calculate(requestBody, month, year, application);
            CloudBlockBlob carbonBlob = outputContainer.GetBlockBlobReference($"{application}/{year}/{month.ToString("00")}/carbon.json");
            string carbonStr = JsonConvert.SerializeObject(carbonResponses);
            await carbonBlob.UploadTextAsync(carbonStr);
            return new OkObjectResult(carbonResponses);
         }
         catch (Exception ex)
         {
            log.LogError(ex.ToString());
            throw;
         }

      }
   }
}
