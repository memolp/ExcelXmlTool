using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ExcelXmlTool.Data
{
    public class ExcelData
    {
        public string absolutepath = "";
        public List<List<string>> values = new List<List<string>>();
        public List<string> headers = new List<string>();
        public string xmlfilename = "";
    }

    public class ExcelTool
    {
        public Dictionary<string, ExcelData> excelData = new Dictionary<string, ExcelData>();
        void WriteExcelData(string filename, XmlData data)
        {
            string basename = Path.GetFileNameWithoutExtension(filename);
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(basename);
                // 第一行
                IRow row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("表名");
                row.CreateCell(1).SetCellValue(basename + ".xml");
                // 第二、三行 空行
                row = sheet.CreateRow(1);
                row = sheet.CreateRow(2);
                // 第四行 标题
                row = sheet.CreateRow(3);
                int columnIndex = 0;
                foreach(string title in data.headers)
                {
                    row.CreateCell(columnIndex++).SetCellValue(title);
                }
                // 第五行开始写入数据
                int rowIndex = 4;
                foreach(Dictionary<string, string> attrs in data.values)
                {
                    row = sheet.CreateRow(rowIndex++);
                    columnIndex = 0;
                    foreach(string title in data.headers)
                    {
                        if(attrs.ContainsKey(title))
                        {
                            row.CreateCell(columnIndex).SetCellValue(attrs[title]);
                        }
                        columnIndex++;
                    }
                }
                workbook.Write(fs);
            }
        }

        public void WriteExcelWithData(string path, Dictionary<string, XmlData> data)
        {
            foreach(string filename in data.Keys)
            {
                string name = Path.GetFileNameWithoutExtension(filename);
                string excel = Path.Combine(path, name + ".xlsx");
                WriteExcelData(excel, data[filename]);
            }
        }

        public void LoadExcel(string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Open))
            {
                fs.Position = 0;
                XSSFWorkbook workbook = new XSSFWorkbook(fs);
                ExcelData excel = new ExcelData();
                ISheet sheet = workbook.GetSheetAt(0);
                // 第一行
                IRow row = sheet.GetRow(0);
                excel.xmlfilename = row.GetCell(1).StringCellValue;
                excel.absolutepath = filename;
                // 第四行 得到标题
                row = sheet.GetRow(3);
                int cellCount = row.LastCellNum;
                for(int i = 0; i < cellCount; i++)
                {
                    excel.headers.Add(row.GetCell(i).StringCellValue);
                }
                // 第五行开始 得到数据
                int lastRow = sheet.LastRowNum;
                for(int i = 4; i <= lastRow; i++)
                {
                    row = sheet.GetRow(i);
                    List<string> line = new List<string>();
                    for(int j = 0; j < cellCount; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if(cell != null)
                        {
                            line.Add(cell.StringCellValue);
                        }else
                        {
                            line.Add(null);
                        }
                    }
                    excel.values.Add(line);
                }
                excelData.Add(Path.GetFileName(filename), excel);
            }
        }

        public void LoadExcelWithPath(string path)
        {
            foreach(string filename in Directory.EnumerateFiles(path))
            {
                if(filename.EndsWith(".xlsx") && File.Exists(filename))
                {
                    LoadExcel(filename);
                }
            }
        }
    }
}
