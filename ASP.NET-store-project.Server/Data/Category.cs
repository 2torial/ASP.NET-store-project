using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class Category(string type, string label)
    {
        [Key]
        public string Type { get; set; } = type;

        public string Label { get; set; } = label;
    }
}