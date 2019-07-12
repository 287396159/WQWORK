using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CiXinLocation.Utils
{
    class ConfigFileUtils
    {
        string currentPath = "";
        string file_name = "";

        public ConfigFileUtils(){}

        public void fileCreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }

        public string CurrentPath {
            get { return currentPath; }
            set { currentPath = value;}
        }
        
        public void setCurrentApplicationPatn()
        {
            currentPath = Directory.GetCurrentDirectory();
        }

        public void fileCreate(string filePath)
        {
            FileStream fs = File.Create(filePath);
            fs.Flush();
            fs.Close();//创建完文件，就得关闭，否则就是站着茅坑不拉屎
        }

        public void getFile(string directoryPath, string fileName)
        {
            file_name = fileName;
            string filePath = Path.Combine(directoryPath, fileName);
            if (File.Exists(filePath)) return;
            fileCreate(filePath);
        }

        public void clearData() {
            string filePath = Path.Combine(currentPath, file_name);
            if (!File.Exists(filePath))
            {
              //  Console.WriteLine("{0} already exists!", filePath);
                return;
            }
            // Create the writer for data.
            try {
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fs.Flush();
                fs.Close();
            }
            catch { }            
        }

        public void addDataInFile(byte[] byteData) {
            string filePath = Path.Combine(currentPath, file_name);
            if (!File.Exists(filePath)) {
             //   Console.WriteLine("{0} already exists!", filePath);
                return;
            }
            // Create the writer for data.
            FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);            
            BinaryWriter w = new BinaryWriter(fs);
            string data = "data";
            w.Write(System.Text.Encoding.ASCII.GetBytes(data));
            w.Write(byteData);
            w.Flush();
            fs.Flush();
            w.Close();           
            fs.Close();
        }

        public byte[] getDataFromFile() {
            string filePath = Path.Combine(currentPath, file_name);
            if (!File.Exists(filePath))
            {
             //   Console.WriteLine("{0} already exists!", filePath);
                return null;
            }
            // Create the read for data.

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            //Byte[] buffer = br.ReadBytes(2);
            long bufSize = br.BaseStream.Length;
            byte[] imageBuffer = new byte[bufSize];
            br.Read(imageBuffer, 0, Convert.ToInt32(bufSize));
            //br.Close();
            br.Close();
            fs.Dispose();
            fs.Close();
            return imageBuffer;
        }

    }
}
