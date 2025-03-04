namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Image(int id, string content, int itemId)
    {
        public int Id { get; set; } = id;

        public string Content { get; set; } = content;

        public int ItemId { get; set; } = itemId;

    }
}