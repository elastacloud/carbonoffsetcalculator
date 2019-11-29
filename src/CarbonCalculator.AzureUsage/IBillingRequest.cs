using AadHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarbonCalculator.AzureUsage
{
    public interface IBillingRequest
    {
        Task<string> MakeRequest(UsageRequest usageRequest);
        Task<T> MakeObjectRequest<T>(UsageRequest usageRequest);
    }
}
