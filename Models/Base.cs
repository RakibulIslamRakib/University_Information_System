namespace University_Information_System.Models
{
    public class Base
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string? SearchValue { get; set; }
        public string? SearchColumn { get; set; }
    }
}
