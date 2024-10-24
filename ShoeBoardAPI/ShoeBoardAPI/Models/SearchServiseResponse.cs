namespace ShoeBoardAPI.Models
{
    public class SearchServiseResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public T? Data { get; set; }
    }
}
