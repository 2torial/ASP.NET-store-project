using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class Status
    {
        [Key]
        public string Code { get; set; }

    }

    public class OrderStatus
    {
        public int OrderId { get; set; }

        public string StatusCode { get; set; }

        public DateTime DateOfChange { get; set; }

    }
}