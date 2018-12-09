using System;
using Microsoft.Office.Interop.Excel;

namespace Module_File_Manager.Content_Searcher
{
    /// <summary>
    /// The sub module excel_Searcher from sub module of file manager module for search on content of files
    /// </summary>
    class _m_File_Manager_Content_Searcher_excel_Searcher
    {
        /// <summary>
        /// Excels to text.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>return extracted all string from row and column of excel file</returns>
        public static string ExcelToText(string path)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);


            string ExcelText = "";
            foreach (Worksheet sheet in wb.Sheets)
            {

                //Worksheet sheet = (Worksheet)wb.Sheets[1];

                Microsoft.Office.Interop.Excel.Range excelRange = sheet.UsedRange;



                foreach (Microsoft.Office.Interop.Excel.Range row in excelRange.Rows)
                {
                    string rowText = "";
                    int rowNumber = row.Row;
                    //int cellcount=excelRange.Columns.Count;
                    foreach (Microsoft.Office.Interop.Excel.Range cell in excelRange.Rows[rowNumber].Cells)
                    {
                        var cellText = cell.Value2;
                        if (cellText != null)
                        {
                            rowText += cellText + " ";
                        }
                    }

                    ExcelText += rowText + "\r\n";

                }
            }
            wb.Close();
            app.Quit();
            return ExcelText;
        }
    }
}
