using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AadHelper;
using Newtonsoft.Json;

namespace CarbonCalculator.AzureUsage
{
    public class UsageBillingRequest : IBillingRequest
   {
      public static string ADALServiceURL = "https://login.microsoftonline.com";
      public static string ARMBillingServiceURL = "https://management.azure.com";
      public static string TenantDomain = "https://management.azure.com";

        public async Task<string> MakeRequest(UsageRequest usageRequest)
        {
            // configure the start time usage first of all and then end time in ISO8601 - time wi
            string usageStartTime = usageRequest.startTime.ToString("yyyy-MM-ddTHH:00:00Z").Replace(":", "%3a");
            string usageEndTime = usageRequest.endTime.ToString("yyyy-MM-ddTHH:00:00Z").Replace(":", "%3a");

            string payload = null;
            var issuer = new TokenIssuer();
            // Build up the HttpWebRequest 
            string requestUrl =
                $"{ARMBillingServiceURL}/subscriptions/{usageRequest.subscriptionId}/providers/Microsoft.Commerce/UsageAggregates?api-version=2015-06-01-preview&reportedstartTime={usageStartTime}&reportedEndTime={usageEndTime}&aggregationGranularity=Hourly&showDetails=true";
            var newRequest = usageRequest.nextLink ?? requestUrl;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(newRequest);

            // Add the OAuth Authorization header, and Content Type header 
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + await issuer.GetOAuthTokenFromAad(
               ADALServiceURL,
               TenantDomain,
               ARMBillingServiceURL,
               usageRequest.clientId,
               usageRequest.clientKey
               ));
            request.ContentType = "application/json";
            // TODO: check to see that a valid response code is being returned here...
            using (var response = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                payload = await response.ReadToEndAsync();
            }
            
            return payload;
        }

        public async Task<T> MakeObjectRequest<T>(UsageRequest usageRequest)
        {
            var payload = await MakeRequest(usageRequest);
            // read this into a JSON string we're going to get each next link
            var conversion = JsonConvert.DeserializeObject<Usage>(payload);
            var nextPayload = new NextPayload(conversion.NextLink, conversion.Value);
            // crappy but works - boxing/unboxing get rid of 
            return (T)(object)nextPayload;
        }   
    }

    public class NextPayload
    {
        public NextPayload(string nextLink, List<UsageAggregate> payload)
        {
            NextLink = nextLink;
            Payload = payload;
        }
        public string NextLink { get; }
        public List<UsageAggregate> Payload { get; }
    }
}