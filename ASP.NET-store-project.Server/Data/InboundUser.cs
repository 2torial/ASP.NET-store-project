using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data
{
    [Keyless]
    public class InboundUser
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string PassWord { get; set; }
    }
}