namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Image(string content, int itemId)
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = content;

        public int ItemId { get; set; } = itemId;

    }
}