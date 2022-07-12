using ExcelXmlTool.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelXmlTool
{
    public partial class MainFrame : Form
    {
        public MainFrame()
        {
            InitializeComponent();
        }

        private void SetRawXmlPathEvt(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = false;
            folderBrowser.Description = "选择原始xml存放的目录";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowser.SelectedPath;
                if(Directory.Exists(path))
                {
                    InputRawXmlPath.Text = path;
                    _RuntimeData.XmlRawPath = path;
                }
            }
        }

        private void SetExcelPathEvt(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = true;
            folderBrowser.Description = "选择将xml转成Excel存放的目录";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowser.SelectedPath;
                if (Directory.Exists(path))
                {
                    InputExcelPath.Text = path;
                    _RuntimeData.ExcelPath = path;
                }
            }
        }

        private void SetXmlPathEvt(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = true;
            folderBrowser.Description = "选择导出Excel为xml时存放的目录";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowser.SelectedPath;
                if (Directory.Exists(path))
                {
                    InputXmlPath.Text = path;
                    _RuntimeData.XmlPath = path;
                }
            }
        }

        private void OnRawXMLToExcelEvt(object sender, EventArgs e)
        {
            string xml_path = InputRawXmlPath.Text;
            string excel_path = InputExcelPath.Text;
            if(string.IsNullOrEmpty(xml_path))
            {
                MessageBox.Show("请先设置原始XML的目录");
                return;
            }
            if(string.IsNullOrEmpty(excel_path))
            {
                MessageBox.Show("请先设置Excel存放的目录");
                return;
            }
            XmlTool xml = new XmlTool();
            xml.LoadXmlFromDir(xml_path);

            ExcelTool excel = new ExcelTool();
            excel.WriteExcelWithData(excel_path, xml.xmlData);
            MessageBox.Show("Excel生成完成");
        }

        private void OnExcelExportXmlEvt(object sender, EventArgs e)
        {
            string xml_path = InputXmlPath.Text;
            string excel_path = InputExcelPath.Text;
            if (string.IsNullOrEmpty(xml_path))
            {
                MessageBox.Show("请先设置导出XML存放的目录");
                return;
            }
            if (string.IsNullOrEmpty(excel_path))
            {
                MessageBox.Show("请先设置Excel存放的目录");
                return;
            }

            ExcelTool excel = new ExcelTool();
            excel.LoadExcelWithPath(excel_path);

            XmlTool xml = new XmlTool();
            xml.WriteXmlWithPath(xml_path, excel.excelData);

            MessageBox.Show("Xml生成完成");
        }
        RuntimeData _RuntimeData;
        private void OnLoadFrameEvt(object sender, EventArgs e)
        {
            _RuntimeData = RuntimeData.LoadRuntime();
            InputRawXmlPath.Text = _RuntimeData.XmlRawPath;
            InputXmlPath.Text = _RuntimeData.XmlPath;
            InputExcelPath.Text = _RuntimeData.ExcelPath;
        }

        private void OnClosedFrameEvt(object sender, FormClosedEventArgs e)
        {
            RuntimeData.SaveRuntime(_RuntimeData);
        }
    }
}
