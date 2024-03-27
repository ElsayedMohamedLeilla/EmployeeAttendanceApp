using ClosedXML.Excel;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Dtos.Excel;
using Dawem.Translations;

namespace Dawem.Validation.BusinessValidation.ExcelValidations
{
    public class ExcelValidator
    {
        public static Dictionary<string, string> InitialValidate(IniValidationModelDTO iniValidationDTO)
        {
            bool IsvalidExcel = IsValidExcelFile(iniValidationDTO.FileStream);
            Dictionary<string, string> validationMessages = new();

            using var workbook = new XLWorkbook(iniValidationDTO.FileStream);
            var worksheet = workbook.Worksheet(1); // Assuming you want to validate the first worksheet
            var actualHeaders = worksheet.FirstRow().CellsUsed().Select(cell => cell.Value.ToString()).ToArray();
            int rowCount = worksheet.RowsUsed().Count();

            if (iniValidationDTO.MaxRowCount <= 0 && iniValidationDTO.ExcelExportScreen == ExcelExportScreen.Employees)
            {
                validationMessages.Add(AmgadKeys.RowCountProblem, TranslationHelper.GetTranslation(AmgadKeys.YouDonotAllowToAddAnyEmployee, iniValidationDTO.Lang));
                return validationMessages;
            }
            if (!IsvalidExcel)
            {
                validationMessages.Add(AmgadKeys.FileProblem, TranslationHelper.GetTranslation(AmgadKeys.FileExtentionNotValidOnlyExcelFilesAllawed, iniValidationDTO.Lang));
                return validationMessages;
            }
            // Check header
            else if (!iniValidationDTO.ExpectedHeaders.SequenceEqual(actualHeaders))
            {
                validationMessages.Add(AmgadKeys.HeaderProblem, TranslationHelper.GetTranslation(AmgadKeys.Headersdonotmatchtheexpectedvalues, iniValidationDTO.Lang));
                return validationMessages;
            }
            // Check row count
            else if (rowCount > iniValidationDTO.MaxRowCount + 1 && iniValidationDTO.ExcelExportScreen == ExcelExportScreen.Employees) // add 1 to exclude header from count
            {
                validationMessages.Add(AmgadKeys.RowCountProblem, TranslationHelper.GetTranslation(AmgadKeys.RowCountExceedsTheExpected, iniValidationDTO.Lang));
                return validationMessages;
            }
            // if no data found
            else if (rowCount == 1)
            {
                validationMessages.Add(AmgadKeys.EmptyDataProblem, TranslationHelper.GetTranslation(AmgadKeys.NoDataImportedInFileTheFileIsEmpty, iniValidationDTO.Lang));
                return validationMessages;
            }
            else
            {
                // Check for duplicate values in each column
                for (int columnIndex = 1; columnIndex <= actualHeaders.Length; columnIndex++)
                {
                    if (!iniValidationDTO.ColumnsToCheckDuplication.Contains(columnIndex))
                    {
                        continue; // Skip the column if it's in the list of excluded columns
                    }
                    HashSet<string> uniqueValues = new HashSet<string>();
                    Dictionary<string, List<int>> duplicateCells = new Dictionary<string, List<int>>();

                    for (int rowNum = 2; rowNum <= rowCount; rowNum++)
                    {
                        var cell = worksheet.Cell(rowNum, columnIndex);
                        var cellValue = cell.GetString();
                        if (!string.IsNullOrWhiteSpace(cellValue))
                        {
                            if (uniqueValues.Contains(cellValue))
                            {
                                string columnName = actualHeaders[columnIndex - 1];
                                if (!duplicateCells.ContainsKey(columnName))
                                {
                                    duplicateCells[columnName] = new List<int>();
                                }
                                duplicateCells[columnName].Add(rowNum);
                            }
                            else
                            {
                                uniqueValues.Add(cellValue);
                            }
                        }
                    }
                    // Add duplicate cell information to validation messages
                    foreach (var kvp in duplicateCells)
                    {
                        string cellReferences = string.Join(", ", kvp.Value.Select(row => $"A{row}"));
                        validationMessages.Add($"{AmgadKeys.DuplicateColumnValueProblem}{kvp.Key}",
                             $"{(TranslationHelper.GetTranslation(AmgadKeys.DuplicateColumnValueFound, iniValidationDTO.Lang))} ({cellReferences})");
                    }
                }
                
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
                validationMessages.Add(AmgadKeys.NullColumnsProblem + index++, $"(Cell {nullColumn.Item2}) " + TranslationHelper.GetTranslation(AmgadKeys.CannotBeNull, iniValidationDTO.Lang));
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
