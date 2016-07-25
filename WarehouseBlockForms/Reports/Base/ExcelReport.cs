using System;
using Microsoft.Office.Interop.Excel;
using WarehouseBlockForms.Classess;
using System.Collections.Generic;
using System.IO;

namespace WarehouseBlockForms.Reports.Base
{
    public abstract class ExcelReport : Report
    {

        public Application reportApp;
        private Workbook excelWorkbook;
        private Worksheet excelWorkSheet;
        private object misValue = System.Reflection.Missing.Value;

        private int lastTopIndex = 1;

        protected Dictionary<int, double> columnsWidth = new Dictionary<int, double>();


        IniFile iniFile = new IniFile("settings.ini");
        protected string DefaultPath
        {
            get
            {
                return "c:";
            }
        }

        public ExcelReport ()
        {
            reportApp = new Application();
            if(reportApp == null)
            {
                throw new Exception("Excel not installed");
            }
            excelWorkbook = reportApp.Workbooks.Add(misValue);
            excelWorkSheet = (Worksheet)excelWorkbook.Worksheets[1];
        }

        public int ColumnCount { get; set; }
        public string LastError { get; private set; }

        protected bool Create ()
        {
            if (!Directory.Exists(Path))
            {
                Log.WriteError("Path '" + Path + "' not exist");
                return false;
            }
            bool result = false;
            try
            {
                //set header
                SetH1();
                //set titles
                SetHeaderRow();
                //set data
                SetData();
                //set footer
                SetF1();
                //set columns width
                foreach(KeyValuePair<int, double> w in columnsWidth)
                {
                    excelWorkSheet.Columns[w.Key].ColumnWidth = w.Value;
                }
                excelWorkbook.SaveAs(Path + @"\" + ReportName + ".xlsx");
                result = true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
                result = false;
            }
            finally
            {
                excelWorkbook.Close(true, misValue, misValue);
                reportApp.Quit();
                releaseObject(excelWorkSheet);
                releaseObject(excelWorkbook);
                releaseObject(reportApp);
            }
            return result;
        }

        protected void SetH1 ()
        {
            if(H1 != null)
            {
                excelWorkSheet.Cells[lastTopIndex, 1] = H1;
                merge(lastTopIndex, lastTopIndex + 1, 1, ColumnCount);
                bold(lastTopIndex, lastTopIndex + 1, 1, ColumnCount);
                wrap(lastTopIndex, lastTopIndex + 1, 1, ColumnCount);
                centerAlign(lastTopIndex, lastTopIndex + 1, 1, ColumnCount);
                lastTopIndex += 2;
            }
        }
        protected void SetHeaderRow ()
        {
            if(HeaderRow != null && HeaderRow.Length > 0)
            {
                for(int i = 0; i < HeaderRow.Length; i++)
                {
                    excelWorkSheet.Cells[lastTopIndex, i + 1] = HeaderRow[i];
                    bold(lastTopIndex, lastTopIndex, i + 1, i + 1);
                    border(lastTopIndex, lastTopIndex, i + 1, i + 1);
                }
                lastTopIndex += 1;
            }
        }
        protected void SetData ()
        {
            if (Data == null) return;

            for(int i = 0; i < Data.Count; i++)
            {
                for(int j = 0; j < Data[i].Row.Count; j++)
                {
                    excelWorkSheet.Cells[lastTopIndex, j + 1] = Data[i].Row[j];
                }
                setRowStyle(Data[i].Style);
                lastTopIndex++;
            }
        }
        protected void SetF1 ()
        {
            if(F1 != null)
            {
                excelWorkSheet.Cells[lastTopIndex, 1] = F1;
                merge(lastTopIndex, lastTopIndex + 1, 1, ColumnCount);
                lastTopIndex += 2;
            }
        }


        protected void merge(int r1, int r2, int c1, int c2)
        {
            excelWorkSheet.Range[
                excelWorkSheet.Cells[r1, c1],
                excelWorkSheet.Cells[r2, c2]
                ].Merge();
        }

        protected void bold (int r1, int r2, int c1, int c2)
        {

            excelWorkSheet.Range[
                excelWorkSheet.Cells[r1, c1],
                excelWorkSheet.Cells[r2, c2]
                ].Font.Bold = true;
        }

        protected void wrap(int r1, int r2, int c1, int c2)
        {

            excelWorkSheet.Range[
                excelWorkSheet.Cells[r1, c1],
                excelWorkSheet.Cells[r2, c2]
                ].Cells.WrapText = true;
        }

        protected void selection(int r1, int r2, int c1, int c2)
        {

            excelWorkSheet.Range[
                excelWorkSheet.Cells[r1, c1],
                excelWorkSheet.Cells[r2, c2]
                ].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
        }

        protected void centerAlign(int r1, int r2, int c1, int c2)
        {

            excelWorkSheet.Range[
                excelWorkSheet.Cells[r1, c1],
                excelWorkSheet.Cells[r2, c2]
                ].Cells.HorizontalAlignment = XlHAlign.xlHAlignCenter;
        }

        protected void border(int r1, int r2, int c1, int c2)
        {
            excelWorkSheet.Range[
                excelWorkSheet.Cells[r1, c1],
                excelWorkSheet.Cells[r2, c2]
                ].Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
        }

        protected void setLandscapeOrientation ()
        {
            excelWorkSheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
        }

        private void setRowStyle (List<ReportRow.RowStyle> styles)
        {
            if (styles == null) return;
            foreach (ReportRow.RowStyle style in styles)
            {
                switch (style)
                {
                    case ReportRow.RowStyle.Bold: bold(lastTopIndex, lastTopIndex, 1, Data.Count); break;
                    case ReportRow.RowStyle.TextAlignCenter: centerAlign(lastTopIndex, lastTopIndex, 1, Data.Count); break;
                    case ReportRow.RowStyle.Selection: selection(lastTopIndex, lastTopIndex, 1, Data[0].Row.Count); break;
                    case ReportRow.RowStyle.Border: border(lastTopIndex, lastTopIndex, 1, Data[0].Row.Count); break;
                }
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw new Exception("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}
