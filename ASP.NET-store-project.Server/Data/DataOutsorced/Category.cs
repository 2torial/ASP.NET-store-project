using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Category(string type)
    {
        [Key]
        public string Type { get; set; } = type;
    }
}