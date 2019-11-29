using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AadHelper;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace CarbonCalculator.AzureUsage
{
    public class TokenIssuer
    {
        public async Task<string> GetOAuthTokenFromAad(
           string adalServiceUrl,
           string tenantDomain,
           string armBillingServiceUrl,
           string clientId,
           string clientKey
           )
        {
            var authenticationContext = new AuthenticationContext($"{adalServiceUrl}/{tenantDomain}");

            //Ask the logged in user to authenticate, so that this client app can get a token on his behalf
            var result = await authenticationContext.AcquireTokenAsync(
                $"{armBillingServiceUrl}/", new ClientCredential(clientId, clientKey));

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }

            return result.AccessToken;
        }
    }
}
