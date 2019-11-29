using AadHelper;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarbonCalculator.AzureUsage
{
    /// <summary>
    /// Used to return the usage data for the subscription 
    /// </summary>
    public interface IUsageApi
    {
        /// <summary>
        /// Given date boundaries returns the usage for this period
        /// </summary>
        Task<Usage> GetUsageByDate(UsageRequest usageRequest);
    }
}
