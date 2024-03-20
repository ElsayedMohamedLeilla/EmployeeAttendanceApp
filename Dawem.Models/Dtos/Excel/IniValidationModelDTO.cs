using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Excel
{
    public class IniValidationModelDTO
    {
        public Stream FileStream { get; set; }
        public string[] ExpectedHeaders { get; set; }
        public int MaxRowCount { get; set; }
        public List<int> ColumnIndexToCheckNull { get; set; } = new List<int>();
        public List<int> ColumnsToCheckDuplication { get; set; } = new List<int>();
        public string Lang { get; set; }
        public  ExcelExportScreen ExcelExportScreen { get; set; }
    }
}
