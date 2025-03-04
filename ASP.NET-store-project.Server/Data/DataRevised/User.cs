using ASP.NET_store_project.Server.Data.DataOutsorced;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data.DataRevised
{
    public class User(string userName, string passWord, bool isAdmin = false)
    {
        [Key]
        public Guid Id { get; set; }

        public string UserName { get; set; } = userName;

        public string PassWord { get; set; } = passWord;

        public bool IsAdmin { get; set; } = isAdmin;



        public List<Order> Orders { get; } = [];

        public CustomerDetails CustomerDetails { get; set; } = null!;

        public AdressDetails AdressDetails { get; set; } = null!;



        public List<SelectedItem> SelectedItems { get; set; } = [];

    }
}