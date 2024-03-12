using ClosedXML.Excel;
using Dawem.Helpers;
using Dawem.Models.Dtos.Excel;
using Dawem.Translations;

namespace FollowUp.Validation.BusinessValidation.General
{
    public class ExcelValidator
    {
        public static Dictionary<string, string> InitialValidate(IniValidationModelDTO iniValidationDTO, string lang)
        {
            bool IsvalidExcel = IsValidExcelFile(iniValidationDTO.FileStream);
            Dictionary<string, string> validationMessages = new();
            using var workbook = new XLWorkbook(iniValidationDTO.FileStream);
            var worksheet = workbook.Worksheet(1); // Assuming you want to validate the first worksheet
            var actualHeaders = worksheet.FirstRow().CellsUsed().Select(cell => cell.Value.ToString()).ToArray();
            if (!IsvalidExcel)
            {
                validationMessages.Add(AmgadKeys.FileProblem, TranslationHelper.GetTranslation(AmgadKeys.FileExtentionNotValidOnlyExcelFilesAllawed, lang));
            }
            // Check header
            if (!Enumerable.SequenceEqual(iniValidationDTO.ExpectedHeaders, actualHeaders))
            {
                validationMessages.Add(AmgadKeys.HeaderProblem, TranslationHelper.GetTranslation(AmgadKeys.Headersdonotmatchtheexpectedvalues, lang));
            }
            // Check row count
            int rowCount = worksheet.RowsUsed().Count();
            if (rowCount > iniValidationDTO.MaxRowCount)
            {
                validationMessages.Add(AmgadKeys.RowCountProblem, TranslationHelper.GetTranslation(AmgadKeys.RowCountExceedsTheExpected, lang));
            }
            // if no data found
            if (rowCount == 1)
            {
                validationMessages.Add(AmgadKeys.EmptyData, TranslationHelper.GetTranslation(AmgadKeys.NoDataImportedInFileTheFileIsEmpty, lang));
            }

            List<Tuple<int, string>> nullColumns = new List<Tuple<int, string>>();
            foreach (int columnIndex in iniValidationDTO.ColumnIndexToCheckNull)
            {
                int excelColumnIndex = columnIndex; // Adjust to 1-based index for ClosedXML
                // Iterate through each row in the column, starting from the second row (skipping header)
                for (int rowNum = 2; rowNum <= worksheet.RowsUsed().Count(); rowNum++)
                {
                    var cell = worksheet.Cell(rowNum, excelColumnIndex);
                    // Check if the cell is empty or null
                    if (cell.IsEmpty() || string.IsNullOrWhiteSpace(cell.GetString()))
                    {
                        // Get the cell reference (e.g., A1, B2)
                        string cellReference = cell.Address.ToString();
                        // Add the column index and cell reference to the list
                        nullColumns.Add(new Tuple<int, string>(excelColumnIndex, cellReference));
                    }
                }
            }
            int index = 0;
            foreach (var nullColumn in nullColumns)
            {
                validationMessages.Add(AmgadKeys.NullColumnsProblem + index++, $"(Cell {nullColumn.Item2}) has no value.");
            }

            return validationMessages;
        }

        public static bool IsValidExcelFile(Stream fileStream)
        {
            try
            {
                using (XLWorkbook workbook = new XLWorkbook(fileStream))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
