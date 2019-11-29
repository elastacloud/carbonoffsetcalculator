using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonCalculator.AzureUsage
{
   public class UsageRequest
   {
      public DateTime startTime { get; set; }
      public DateTime endTime { get; set; }
      public string subscriptionId { get; set; }
      public string clientId { get; set; }
      public string clientKey { get; set; }
      public string nextLink { get; set; }
   }
}
