using Newtonsoft.Json;
using System.Linq;

namespace Elastacloud.CarbonCalculator.Manifest
{
   public class UnpackManifest
   {
      public UnpackManifest(string json)
      {
         AzureManifest = json;
      }

      public string AzureManifest { get; }

      public AzureServices Unplug()
      {
         var services = JsonConvert.DeserializeObject<AzureServices>(AzureManifest);
         // unpack all of the services here 
         return services;
      }
   }
}
