using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class OrderStatus
    {
        [Key]
        public string Status { get; set; }

        public DateTime DateOfStatusChange { get; set; }

    }
}