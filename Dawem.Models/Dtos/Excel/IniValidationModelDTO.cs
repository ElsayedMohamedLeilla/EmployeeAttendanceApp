namespace Dawem.Models.Dtos.Excel
{
    public class IniValidationModelDTO
    {
        public Stream FileStream { get; set; }
        public string[] ExpectedHeaders { get; set; }
        public int MaxRowCount { get; set; }
        public List<int> ColumnIndexToCheckNull { get; set; } = new List<int>();
    }
}
