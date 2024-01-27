using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace ASP.NET_store_project.Server.Data
{
    public class Specification
    {
        [Key]
        public int Id { get; set; }

        public string Label { get; set; }

        public string Configuration { get; set; }

        public List<Item> Items { get; } = [];

    }
}
