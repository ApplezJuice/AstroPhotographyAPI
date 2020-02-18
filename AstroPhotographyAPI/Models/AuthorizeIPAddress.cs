using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTools;
using Microsoft.AspNetCore.Mvc;

namespace AstroPhotographyAPI.Models
{
    public class AuthorizeIPAddress : ActionFilterAttribute
    {
        private readonly IEnumerable<IPAddressRange> authorizedRanges;

        public AuthorizeIPAddress(IIPWhitelistConfiguration configuration)
        {
            this.authorizedRanges = configuration.AuthorizedIPAddresses
                .Select(item => IPAddressRange.Parse(item));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var clientIPAddress = context.HttpContext.Connection.RemoteIpAddress;
            if (!this.authorizedRanges.Any(range => range.Contains(clientIPAddress)))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }


    public class IPWhitelistConfiguration : IIPWhitelistConfiguration
    {
        public IEnumerable<string> AuthorizedIPAddresses { get; set; }
    }

    public interface IIPWhitelistConfiguration
    {
        IEnumerable<string> AuthorizedIPAddresses { get; }
    }
}
