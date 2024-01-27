using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class User
    {
        [Key]
        public string UserName { get; set; }

        public string PassWord { get; set; }
    }
}