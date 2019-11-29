using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AadHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CarbonCalculator.AzureUsage
{
    /// <summary>
    /// Used to get the usage details from Azure - this is pluggable and whilst the db is riged towards Azure should be able to support other providers
    /// </summary>
    public class AzureUsage : IUsageApi
    {
        public AzureUsage() { }


      /// <summary>
      /// Given a start date and end date gets all of the usage inbetween 
      /// </summary>
      public async Task<Usage> GetUsageByDate(UsageRequest usageRequest)
      {
         var billingRequest = new UsageBillingRequest();
         var usages = new List<UsageAggregate>();
         string nextLink = "dummy";
         while (!String.IsNullOrEmpty(nextLink))
         {
            var payload = await billingRequest.MakeObjectRequest<NextPayload>(usageRequest);
            if (payload.Payload != null)
            {
               usages.AddRange(payload.Payload);
            }
            nextLink = payload.NextLink;
         }
         return new Usage
         {
            Value = usages,
            ReportedStartDate = usageRequest.startTime,
            ReportedEndData = usageRequest.endTime
         };
      }

        

        #region Resource Group gets

        private string GetResourceGroup(UsageAggregate rateUsage)
        {
            return GetDetailFromResourceUriString(rateUsage, "resourceGroups");
        }

        private string GetResourceProvider(UsageAggregate rateUsage)
        {
            return GetDetailFromResourceUriString(rateUsage, "providers");
        }

        private string GetResourceName(UsageAggregate rateUsage)
        {
            return GetDetailFromResourceUriStringByOrdinal(rateUsage);
        }

        private string GetDetailedResourceName(UsageAggregate rateUsage)
        {
            return GetLastPositionFromResourceUriString(rateUsage);
        }

        private string GetDetailFromResourceUriString(UsageAggregate rateUsage, string stringtoMatch)
        {
            if (rateUsage.Properties.InstanceDataRaw != null && rateUsage.Properties.InstanceData?.MicrosoftResources?.ResourceUri != null)
            {
                var instanceParts = rateUsage.Properties.InstanceData.MicrosoftResources.ResourceUri.Split('/');
                for (int i = 0; i < instanceParts.Length; i++)
                {
                    if (instanceParts[i] == stringtoMatch)
                    {
                        return instanceParts[i + 1];
                    }
                }
            }
            else if (rateUsage.Properties.InfoFields?.Project != null && stringtoMatch == "resourceGroups")
            {
               return rateUsage.Properties.InfoFields.Project;
            }

            return null;
        }

        private string GetDetailFromResourceUriStringByOrdinal(UsageAggregate rateUsage, int ordinal = 8)
        {
            if (rateUsage.Properties.InstanceDataRaw != null && rateUsage.Properties.InstanceData?.MicrosoftResources?.ResourceUri != null)
            {
                try
                {
                    var instanceParts = rateUsage.Properties.InstanceData.MicrosoftResources.ResourceUri.Split('/');
                    if (instanceParts.Length > 7)
                        return instanceParts[8];
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return "";
        }

        private string GetLastPositionFromResourceUriString(UsageAggregate rateUsage)
        {
            if (rateUsage.Properties.InstanceDataRaw != null && rateUsage.Properties.InstanceData?.MicrosoftResources?.ResourceUri != null)
            {
                try
                {
                    var instanceParts = rateUsage.Properties.InstanceData.MicrosoftResources.ResourceUri.Split('/');
                    return instanceParts[instanceParts.Length - 1];
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        #endregion


        #region Tags gets 

        private string DeserializeTags(IDictionary<string, string> tags)
        {
            string output = tags?.Aggregate<KeyValuePair<string, string>, string>(null, (current, tag) => current + $"{tag.Key}:{tag.Value};");
            return output?.Length > 0 ? output.Substring(0, output.Length - 1) : null;
        } 

        #endregion 
    }
}