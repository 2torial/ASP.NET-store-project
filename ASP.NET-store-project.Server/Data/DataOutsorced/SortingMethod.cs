using System.ComponentModel.DataAnnotations;

namespace ASP.NET_store_project.Server.Data.DataOutsorced
{
    public class SortingMethod(string label, string sortBy, bool IsAscending = true)
    {
        [Key]
        public string Label { get; set; } = label;

        public string SortBy { get; set; } = sortBy;

        public bool IsAscending { get; set; } = IsAscending;
    }
}
