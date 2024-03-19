using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Excel
{
    public class EmptyExcelDraftModelDTO
    {
        public object Obj { get; set; }
        public string FileName { get; set; }
        public ExcelExportScreen ExcelExportScreen { get; set; }
    }
}
