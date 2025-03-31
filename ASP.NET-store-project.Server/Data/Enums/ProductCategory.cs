using System.ComponentModel;

namespace ASP.NET_store_project.Server.Data.Enums
{
    public enum ProductCategory
    {
        [Description("Headsets")] Headset,
        [Description("Microphones")] Microphone,
        [Description("Laptops/Notebooks/Ultrabooks")] Laptop,
        [Description("Personal Computers")] PersonalComputer
    }

}
