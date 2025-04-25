using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class ItemCategory(string type)
    {
        [Key]
        public string Type { get; set; } = type;
        // Headset, Microphone, Laptop, PersonalComputer

    }
}