using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class Category
    {
        [Key]
        public string Type { get; set; }

        public string Label { get; set; }

    }
}