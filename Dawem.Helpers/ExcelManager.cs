using ClosedXML.Excel;
using Dawem.Models.Dtos.Excel;
using System.Reflection;

namespace Dawem.Helpers
{
    public static class ExcelManager
    {
        public static MemoryStream ExportEmptyDraft(EmptyExcelDraftModelDTO employeeHeaderDraftDTO)
        {
            // Create a new workbook
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(employeeHeaderDraftDTO.FileName);
            // Get the properties of the object
            Type objType = employeeHeaderDraftDTO.Obj.GetType();
            PropertyInfo[] properties = objType.GetProperties();
            // Add column headers from object properties
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
            }
            // Create a memory stream to hold the Excel file content
            MemoryStream stream = new MemoryStream();
            // Save the workbook to the memory stream
            workbook.SaveAs(stream);
            // Rewind the stream to the beginning so it can be read from the start
            stream.Position = 0;
            // Return the memory stream containing the Excel file content
            return stream;
        }
    }
}
