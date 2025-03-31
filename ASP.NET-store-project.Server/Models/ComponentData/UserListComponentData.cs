namespace ASP.NET_store_project.Server.Models.ComponentData
{
    public class UserListComponentData
    {
        public List<UserData> Users { get; set; }

        public class UserData
        {
            public string Name { get; set; }

            public bool IsAdmin { get; set; }
        }
    }
}
