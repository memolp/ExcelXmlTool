using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ExcelXmlTool.Data
{
    [Serializable]
    public class RuntimeData
    {
        public static string RUNTIME_DATA_PATH = "cache.dat";

        public string XmlRawPath;
        public string XmlPath;
        public string ExcelPath;

        /// <summary>
        /// 加载运行时数据
        /// </summary>
        /// <returns></returns>
        public static RuntimeData LoadRuntime()
        {
            if (File.Exists(RUNTIME_DATA_PATH))
            {
                IFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(RUNTIME_DATA_PATH, FileMode.Open))
                {
                    try
                    {
                        RuntimeData rt = (RuntimeData)formatter.Deserialize(fs);
                        return rt;
                    }
                    catch (Exception e)
                    {
                        return new RuntimeData();
                    }
                }
            }
            else
            {
                return new RuntimeData();
            }
        }
        /// <summary>
        /// 存储运行时数据
        /// </summary>
        /// <param name="runtime"></param>
        public static void SaveRuntime(RuntimeData runtime)
        {
            IFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(RUNTIME_DATA_PATH, FileMode.Create))
            {
                formatter.Serialize(fs, runtime);
            }
        }
    }
}
