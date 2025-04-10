namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class Image(string content, Guid itemId)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Content { get; set; } = content;

        public Guid ItemId { get; set; } = itemId;

    }
}