using ClosedXML.Excel;
using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Excel;
using Dawem.Models.Dtos.Dawem.Lookups;
using System.Reflection;

namespace Dawem.Helpers
{
    public static class ExcelManager
    {
        public static MemoryStream ExportEmptyDraft(EmptyExcelDraftModelDTO headerDraftDTO)
        {
            // Create a new workbook
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(headerDraftDTO.FileName);
            // Get the properties of the object
            Type objType = headerDraftDTO.Obj.GetType();
            PropertyInfo[] properties = objType.GetProperties();
            // Add column headers from object properties
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
            }

            // Add another sheet called "Read Me"
            var readmeSheet = workbook.Worksheets.Add("Read Me");
            readmeSheet.Cell(1, 1).Value = "***********  Instructions  ***********";
            readmeSheet.Cell(2, 1).Value = "** Please Follow This instructions Carefully  **";
            readmeSheet.Cell(3, 1).Value = "1. Headers Name Can't be changed.";
            readmeSheet.Cell(4, 1).Value = "2. IsActive Accepted Only true or false if empty the default value is false";

            if (headerDraftDTO.ExcelExportScreen == ExcelExportScreen.Employees)
            {
                readmeSheet.Cell(5, 1).Value = "3. Ensure all required fields are populated EmployeeNumber , EmployeeName , Email , MobilNumber  ";
                readmeSheet.Cell(6, 1).Value = "4. if deparment name is entered make sure that it already exist in db";
                readmeSheet.Cell(7, 1).Value = "5. if JobTitle name is entered make sure that it already exist in db";
                readmeSheet.Cell(8, 1).Value = "6. if Schedule name is entered make sure that it already exist in db";
                readmeSheet.Cell(9, 1).Value = "7. if DirectManagerName name is entered make sure that it already exist in db";
                readmeSheet.Cell(10, 1).Value = "8. AttendanceType May Be One Of this (FullAttendance Or PartialAttendance Or FreeOrShiftAttendance)";
                readmeSheet.Cell(11, 1).Value = "9. EmployeeType May Be One Of this (Military Or CivilService Or Contract Or ContractFromCompany)";
                readmeSheet.Cell(12, 1).Value = "10. ZoneName Is Seperated By comma for example zone1,zone2 ... Make Sure The Name Is Found In DB";
                readmeSheet.Cell(13, 1).Value = "11. EmployeeNumber , EmployeeName , Email , MobilNumber is unique for every employee";
                readmeSheet.Cell(14, 1).Value = "12. follow up any validation message and solve the problem thrown to insert file successfully";
                readmeSheet.Cell(15, 1).Value = "13. Save the file.";
                readmeSheet.Cell(1, 1).Style.Font.Bold = true;
                readmeSheet.Cell(2, 1).Style.Font.Bold = true;
                readmeSheet.Cell(3, 1).Style.Font.Bold = true;
                readmeSheet.Cell(4, 1).Style.Font.Bold = true;
                readmeSheet.Cell(5, 1).Style.Font.Bold = true;
                readmeSheet.Cell(6, 1).Style.Font.Bold = true;
                readmeSheet.Cell(7, 1).Style.Font.Bold = true;
                readmeSheet.Cell(8, 1).Style.Font.Bold = true;
                readmeSheet.Cell(9, 1).Style.Font.Bold = true;
                readmeSheet.Cell(10, 1).Style.Font.Bold = true;
                readmeSheet.Cell(11, 1).Style.Font.Bold = true;
                readmeSheet.Cell(12, 1).Style.Font.Bold = true;
                readmeSheet.Cell(13, 1).Style.Font.Bold = true;
                readmeSheet.Cell(14, 1).Style.Font.Bold = true;
                readmeSheet.Cell(15, 1).Style.Font.Bold = true;

                readmeSheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                readmeSheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                readmeSheet.Cell(1, 1).Style.Font.FontColor = XLColor.Redwood;
                readmeSheet.Cell(2, 1).Style.Font.FontColor = XLColor.Redwood;
                readmeSheet.Cell(3, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(4, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(5, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(6, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(7, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(8, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(9, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(10, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(11, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(12, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(13, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(14, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(15, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(1, 3).Value = "***********  Countries Code ***********";
                readmeSheet.Cell(2, 3).Value = "** This Code will Entered in MobileCountryCode  **";
                readmeSheet.Cell(3, 3).Value = "** Country  **";
                readmeSheet.Cell(3, 4).Value = "** Code  **";

                readmeSheet.Cell(1, 3).Style.Font.FontColor = XLColor.Redwood;
                readmeSheet.Cell(2, 3).Style.Font.FontColor = XLColor.Redwood;
                readmeSheet.Cell(3, 3).Style.Font.FontColor = XLColor.Redwood;
                readmeSheet.Cell(3, 4).Style.Font.FontColor = XLColor.Redwood;

                readmeSheet.Cell(1, 3).Style.Font.Bold = true;
                readmeSheet.Cell(2, 3).Style.Font.Bold = true;
                readmeSheet.Cell(3, 3).Style.Font.Bold = true;
                readmeSheet.Cell(3, 4).Style.Font.Bold = true;
                readmeSheet.Cell(1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                readmeSheet.Cell(2, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                readmeSheet.Cell(3, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                readmeSheet.Cell(3, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;



                int row = 4;

                if (headerDraftDTO.ReadMeObj is IEnumerable<object> enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        var propertiess = item.GetType().GetProperties();

                        readmeSheet.Cell(row, 3).Value = propertiess.First(prop => prop.Name == "NameEn").GetValue(item)?.ToString();
                        readmeSheet.Cell(row, 4).Value = propertiess.First(prop => prop.Name == "Iso3").GetValue(item)?.ToString();
                        readmeSheet.Cell(row, 3).Style.Font.FontColor = XLColor.Red;
                        readmeSheet.Cell(row, 4).Style.Font.FontColor = XLColor.Red;
                        readmeSheet.Cell(row, 3).Style.Font.Bold = true;
                        readmeSheet.Cell(row, 4).Style.Font.Bold = true;
                        readmeSheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        readmeSheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        row++;

                    }
                }



            }
            else if (headerDraftDTO.ExcelExportScreen == ExcelExportScreen.Departments)
            {
                readmeSheet.Cell(5, 1).Value = "3. Ensure all required fields are populated DepartmentName";
                readmeSheet.Cell(6, 1).Value = "4. if deparmenparentName is entered make sure that it already exist in db";
                readmeSheet.Cell(7, 1).Value = "5. if ManagerName name is entered make sure that it already exist in db";
                readmeSheet.Cell(8, 1).Value = "6. follow up any validation message and solve the problem thrown to insert file successfully";
                readmeSheet.Cell(9, 1).Value = "7. Save the file.";
                // Apply formatting to the "Read Me" sheet
                readmeSheet.Cell(1, 1).Style.Font.Bold = true;
                readmeSheet.Cell(2, 1).Style.Font.Bold = true;
                readmeSheet.Cell(3, 1).Style.Font.Bold = true;
                readmeSheet.Cell(4, 1).Style.Font.Bold = true;
                readmeSheet.Cell(5, 1).Style.Font.Bold = true;
                readmeSheet.Cell(6, 1).Style.Font.Bold = true;
                readmeSheet.Cell(7, 1).Style.Font.Bold = true;
                readmeSheet.Cell(8, 1).Style.Font.Bold = true;
                readmeSheet.Cell(9, 1).Style.Font.Bold = true;
                readmeSheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                readmeSheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                readmeSheet.Cell(1, 1).Style.Font.FontColor = XLColor.Redwood;
                readmeSheet.Cell(2, 1).Style.Font.FontColor = XLColor.Redwood;
                readmeSheet.Cell(3, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(4, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(5, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(6, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(7, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(8, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(9, 1).Style.Font.FontColor = XLColor.Red;
            }
            else if (headerDraftDTO.ExcelExportScreen == ExcelExportScreen.Zones)
            {
                readmeSheet.Cell(5, 1).Value = "3. Ensure all required fields are populated DepartmentName";
                readmeSheet.Cell(6, 1).Value = "4. Latitude & Longtude & Raduis Must be Double";
                readmeSheet.Cell(7, 1).Value = "5. follow up any validation message and solve the problem thrown to insert file successfully";
                readmeSheet.Cell(8, 1).Value = "6. Save the file.";
                // Apply formatting to the "Read Me" sheet
                readmeSheet.Cell(1, 1).Style.Font.Bold = true;
                readmeSheet.Cell(2, 1).Style.Font.Bold = true;
                readmeSheet.Cell(3, 1).Style.Font.Bold = true;
                readmeSheet.Cell(4, 1).Style.Font.Bold = true;
                readmeSheet.Cell(5, 1).Style.Font.Bold = true;
                readmeSheet.Cell(6, 1).Style.Font.Bold = true;
                readmeSheet.Cell(7, 1).Style.Font.Bold = true;
                readmeSheet.Cell(8, 1).Style.Font.Bold = true;
                readmeSheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                readmeSheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                readmeSheet.Cell(1, 1).Style.Font.FontColor = XLColor.Redwood;
                readmeSheet.Cell(2, 1).Style.Font.FontColor = XLColor.Redwood;
                readmeSheet.Cell(3, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(4, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(5, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(6, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(7, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(8, 1).Style.Font.FontColor = XLColor.Red;
            }
            else if (headerDraftDTO.ExcelExportScreen == ExcelExportScreen.EmployeeAttendance)
            {
                readmeSheet.Cell(5, 1).Value = "3. Ensure all required fields are populated EmployeeName";
                readmeSheet.Cell(6, 1).Value = "4. Latitude & Longtude Must be Double";
                readmeSheet.Cell(7, 1).Value = "5. follow up any validation message and solve the problem thrown to insert file successfully";
                readmeSheet.Cell(8, 1).Value = "6. FingerPrintType Must Be Of This (CheckIn , CheckOut , BreakIn ,BreakOut Or Summon";
                readmeSheet.Cell(9, 1).Value = "7. RecognitionWay Must Be Of This (NotSet ,FingerPrint , FaceRecognition , VoiceRecognition ,PaternRecognition , PinRecognition or PasswordRecognition ";
                readmeSheet.Cell(10, 1).Value = "8. Save the file.";
                // Apply formatting to the "Read Me" sheet
                readmeSheet.Cell(1, 1).Style.Font.Bold = true;
                readmeSheet.Cell(2, 1).Style.Font.Bold = true;
                readmeSheet.Cell(3, 1).Style.Font.Bold = true;
                readmeSheet.Cell(4, 1).Style.Font.Bold = true;
                readmeSheet.Cell(5, 1).Style.Font.Bold = true;
                readmeSheet.Cell(6, 1).Style.Font.Bold = true;
                readmeSheet.Cell(7, 1).Style.Font.Bold = true;
                readmeSheet.Cell(8, 1).Style.Font.Bold = true;
                readmeSheet.Cell(9, 1).Style.Font.Bold = true;
                readmeSheet.Cell(10, 1).Style.Font.Bold = true;
                readmeSheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                readmeSheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                readmeSheet.Cell(1, 1).Style.Font.FontColor = XLColor.Redwood;
                readmeSheet.Cell(2, 1).Style.Font.FontColor = XLColor.Redwood;
                readmeSheet.Cell(3, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(4, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(5, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(6, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(7, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(8, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(9, 1).Style.Font.FontColor = XLColor.Red;
                readmeSheet.Cell(10, 1).Style.Font.FontColor = XLColor.Red;


            }

            readmeSheet.Columns().AdjustToContents();
            // Create a memory stream to hold the Excel file content
            MemoryStream stream = new();
            // Save the workbook to the memory stream
            workbook.SaveAs(stream);
            // Rewind the stream to the beginning so it can be read from the start
            stream.Position = 0;
            // Return the memory stream containing the Excel file content
            return stream;
        }
    }
}
