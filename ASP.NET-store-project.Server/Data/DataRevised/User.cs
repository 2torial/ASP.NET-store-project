using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data.DataRevised
{
    public class User(string userName, string passWord, bool isAdmin = false)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserName { get; set; } = userName;

        public string PassWord { get; set; } = passWord;

        public bool IsAdmin { get; set; } = isAdmin;



        public CustomerDetails CustomerDetails { get; set; } = null!;

        public AdressDetails AdressDetails { get; set; } = null!;

        public List<BasketProduct> BasketProducts { get; set; } = [];

    }
}