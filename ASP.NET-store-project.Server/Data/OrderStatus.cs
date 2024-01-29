using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class OrderStatus
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string Status { get; set; }

        public DateTime DateOfStatusChange { get; set; }

    }
}