using ASP.NET_store_project.Server.Models.StructuredData;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Controllers.BasketController
{
    public record OrderSummaryData([FromForm] CustomerInfo CustomerInfo, [FromForm] AdressInfo AdressInfo);

}
