using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class Status(string code)
    {
        [Key]
        public string Code { get; set; } = code;

    }

    public class OrderStatus(int orderId, string statusCode, DateTime dateOfChange)
    {
        public int OrderId { get; set; } = orderId;

        public string StatusCode { get; set; } = statusCode;

        public DateTime DateOfChange { get; set; } = dateOfChange;

    }
}