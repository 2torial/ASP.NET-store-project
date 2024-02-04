using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    public class InboundUser
    {
        [Key]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string PassWord { get; set; }
    }
}