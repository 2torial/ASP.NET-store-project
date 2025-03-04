using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class User2(string userName, string passWord, string salt, bool isAdmin = false)
    {
        [Key]
        public string UserName { get; set; } = userName;

        public string HashedPassWord { get; set; } = passWord;

        public bool IsAdmin { get; set; } = isAdmin;

        public List<Order2> Orders { get; } = [];

    }
}