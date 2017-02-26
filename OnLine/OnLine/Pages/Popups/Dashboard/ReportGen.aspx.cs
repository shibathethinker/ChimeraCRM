using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Data;
using System.Collections;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;

namespace OnLine.Pages.Popups.Dashboard
{
    public partial class ReportGen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Image1.Visible = true;
                Dictionary<String,DataTable> dt=(Dictionary<String,DataTable>)
                    Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                generateExcel(Request.QueryString.GetValues("fileName")[0], Request.QueryString.GetValues("heading")[0],
                    dt[Request.QueryString.GetValues("sessionDict")[0]]);
                    
            }
        }

        private static ExcelWorksheet CreateSheet(ExcelPackage p, string sheetName)
        {
            p.Workbook.Worksheets.Add(sheetName);
            ExcelWorksheet ws = p.Workbook.Worksheets[1];
            ws.Name = sheetName; //Setting Sheet's name
            ws.Cells.Style.Font.Size = 10.5F; //Default font size for whole sheet
            ws.Cells.Style.Font.Name = "Andalus"; //Default Font name for whole sheet

            return ws;
        }

        private static void CreateHeader(ExcelWorksheet ws, ref int rowIndex, DataTable dt)
        {
            int colIndex = 1;
            foreach (DataColumn dc in dt.Columns) //Creating Headings
            {
                var cell = ws.Cells[rowIndex, colIndex];

                //Setting the background color of header cells to Gray
                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.Gray);

                //Setting Top/left,right/bottom borders.
                var border = cell.Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //Setting Value in cell
                cell.Value = dc.ColumnName;

                colIndex++;
            }
        }

        private static void CreateData(ExcelWorksheet ws, ref int rowIndex, DataTable dt)
        {
            int colIndex = 0;
            foreach (DataRow dr in dt.Rows) // Adding Data into rows
            {
                colIndex = 1;
                rowIndex++;

                foreach (DataColumn dc in dt.Columns)
                {
                    var cell = ws.Cells[rowIndex, colIndex];

                    //Setting Value in cell
                    //cell.Value = Convert.ToInt32(dr[dc.ColumnName]);
                    cell.Value = dr[dc.ColumnName].ToString();
                    //Setting borders of cell
                    var border = cell.Style.Border;
                    border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex++;
                }
            }
        }

        protected void generateExcel(String fileName,String heading,DataTable dt)
        {
            ExcelPackage p = new ExcelPackage();
            p.Workbook.Properties.Title = heading;

            ExcelWorksheet ws = CreateSheet(p, "Sheet_Report");
            ws.Cells[1, 1].Value = heading;
            ws.Cells[1, 1, 1, dt.Columns.Count].Merge = true;
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
            ws.Cells[1, 1, 1, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            int rowIndex = 2;

            CreateHeader(ws, ref rowIndex, dt);
            CreateData(ws, ref rowIndex, dt);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}", (fileName.IndexOf(".xls")>0?fileName:fileName+".xls")));
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            
            //Generate A File with Random name
            Byte[] bin = p.GetAsByteArray();
            Response.BinaryWrite(bin);
            Image1.Visible = false;
            Response.End();
            
        }
    }
}