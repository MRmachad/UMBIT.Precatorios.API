using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Query;

namespace TSE.Nexus.SDK.API.Models
{
    public class ODataQueryOptionsCustom : ODataQueryOptions
    {
        public ODataQueryOptionsCustom(ODataQueryContext context, HttpRequest request) : base(context, request)
        {

        }

    }
}
