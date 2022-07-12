using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ExcelXmlTool.Data
{
    public class XmlData
    {
        public List<string> headers = new List<string>();
        public List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();
        public string absolutepath = "";

        public void addHeader(string tag)
        {
            if (headers.Contains(tag)) return;
            headers.Add(tag);
        }

        public void addRow(Dictionary<string, string> line)
        {
            values.Add(line);
        }
    }
    public class XmlTool
    {
        public Dictionary<string, XmlData> xmlData = new Dictionary<string, XmlData>();

        public XmlTool()
        {

        }

        void WriteXml(string filename, ExcelData excel)
        {
            XmlTextWriter textWriter = new XmlTextWriter(filename, Encoding.GetEncoding("GBK"));
            textWriter.Formatting = Formatting.Indented;
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("root");
            foreach(List<string> line in excel.values)
            {
                textWriter.WriteStartElement("content");
                for(int i =0; i < line.Count; i++)
                {
                    if(!string.IsNullOrEmpty(line[i]))
                    {
                        textWriter.WriteAttributeString(excel.headers[i], line[i]);
                    }else
                    {
                        textWriter.WriteAttributeString(excel.headers[i], "");
                    }
                }
                textWriter.WriteEndElement();
            }
            textWriter.WriteEndElement();
            textWriter.Flush();
            textWriter.Close();
        }
        public void WriteXmlWithPath(string path, Dictionary<string, ExcelData> data)
        {
            foreach(ExcelData excel in data.Values)
            {
                string filename = Path.Combine(path, excel.xmlfilename);
                WriteXml(filename, excel);
            }
        }

        void ParseXml(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlData data = new XmlData();
            data.absolutepath = filename;

            XmlNode root = doc.SelectSingleNode("root");
            foreach(XmlNode node in root.ChildNodes)
            {
                if(!node.Name.Equals("content", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                Dictionary<string, string> line = new Dictionary<string, string>();
                foreach(XmlAttribute atr in node.Attributes)
                {
                    data.addHeader(atr.Name);
                    line.Add(atr.Name, atr.Value);
                }
                data.addRow(line);
            }
            xmlData.Add(Path.GetFileName(filename), data);
        }


        public void LoadXmlFromDir(string path)
        {
            foreach(string filepath in Directory.EnumerateFiles(path))
            {
                if(filepath.EndsWith(".xml") && File.Exists(filepath))
                {
                    ParseXml(filepath);
                }
            }
        }
    }
}
