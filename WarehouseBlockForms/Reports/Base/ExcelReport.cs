using System;
using Microsoft.Office.Interop.Excel;

namespace WarehouseBlockForms.Reports.Base
{
    public abstract class ExcelReport : Report
    {

        public Application reportApp;
        private Workbook excelWorkbook;
        private Worksheet excelWorkSheet;
        private object misValue = System.Reflection.Missing.Value;

        private int lastTopIndex = 1;

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

        protected bool Create ()
        {
            SetH1();
            SetH2();
            SetH3();
            SetHeaderRow();
            SetData();
            SetF1();
            /*SetF2();
            SetF3();*/

            excelWorkbook.SaveAs(Path + @"\" + ReportName + ".xlsx");

            excelWorkbook.Close(true, misValue, misValue);
            reportApp.Quit();

            releaseObject(reportApp);
            releaseObject(excelWorkbook);
            releaseObject(excelWorkSheet);

            return true;
        }

        protected void SetH1 ()
        {
            if(H1 != null)
            {
                excelWorkSheet.Cells[lastTopIndex, 1] = H1;
                int lastColumn = (ColumnCount() == 0) ? 1 : ColumnCount();
                merge(lastTopIndex, lastTopIndex + 1, 1, lastColumn);
                lastTopIndex += 2;
            }
        }
        protected void SetH2()
        {
            if (H2 != null)
            {
                excelWorkSheet.Cells[lastTopIndex, 1] = H2;
                int lastColumn = (ColumnCount() == 0) ? 1 : ColumnCount();
                merge(lastTopIndex, lastTopIndex + 1, 1, lastColumn);
                lastTopIndex += 2;
            }
        }
        protected void SetH3()
        {
            if (H3 != null)
            {
                excelWorkSheet.Cells[lastTopIndex, 1] = H3;
                int lastColumn = (ColumnCount() == 0) ? 1 : ColumnCount();
                merge(lastTopIndex, lastTopIndex + 1, 1, lastColumn);
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
                }
                lastTopIndex += 1;
            }
        }
        protected void SetData ()
        {
            if (Data == null) return;
            for(int i = 0; i < Data.Length; i++)
            {
                for(int j = 0; j < Data[i].Length; j++)
                {
                    excelWorkSheet.Cells[lastTopIndex, j + 1] = Data[i][j];
                }
                lastTopIndex++;
            }
        }
        protected void SetF1 ()
        {
            if(F1 != null)
            {
                excelWorkSheet.Cells[lastTopIndex, 1] = F1;
                int lastColumn = (ColumnCount() == 0) ? 1 : ColumnCount();
                merge(lastTopIndex, lastTopIndex + 1, 1, lastColumn);
                lastTopIndex += 2;
            }
        }

        protected int RowCount ()
        {
            if(Data != null)
            {
                return Data.Length;
            }
            return 0;
        }
        protected int ColumnCount ()
        {
            if(RowCount() > 0)
            {
                return Data[0].Length;
            }
            return 0;
        }


        private void merge(int r1, int r2, int c1, int c2)
        {
            excelWorkSheet.Range[
                excelWorkSheet.Cells[r1, c1],
                excelWorkSheet.Cells[r2, c2]
                ].Merge();
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
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
