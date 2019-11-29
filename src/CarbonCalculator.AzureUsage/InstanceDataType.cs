using Newtonsoft.Json;

namespace CarbonCalculator.AzureUsage
{
    public class InstanceDataType
    {
        [JsonProperty("Microsoft.Resources")]
        public MicrosoftResourcesDataType MicrosoftResources { get; set; }
    }
}