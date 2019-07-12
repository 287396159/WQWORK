using CiXinLocation.bean;
using MoveableListLib.Bean;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CiXinLocation.Utils
{
    /// <summary>
    /// 将数据存储到文件的Utils类
    /// </summary>
    class DataFileUtils
    {
        private string currentDirectoryPath = Environment.CurrentDirectory;

        public DataFileUtils() { }


        public void createFile(string fileName) 
        {
            FileStream fs = null;
            try
            {
                FileInfo fi = new FileInfo(fileName);
                var di = fi.Directory;
                if (!di.Exists)
                    di.Create();
                fs = File.Create(fileName);                
            } catch { }
            finally
            {
                if (fs != null) 
                {
                    fs.Flush();
                    fs.Close();//创建完文件，就得关闭，否则就是站着茅坑不拉屎
                }
            }           
        }

        /// <summary>
        /// 序列化存储数据
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool serializeCache(List<CenJiBean> obs, string name) 
        {
            if (null == obs) return false;
            if (obs.Count < 1) return false;
            return SerializeCacheBean2(obs, currentDirectoryPath + "\\" + name);
        }

        /// <summary>
        /// 序列化存储重新启动的数据
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool serializeRestart(RestartBean resBean, string name)
        {
            if (null == resBean) return false;
            return SerializeCacheBean2(resBean, currentDirectoryPath + "\\" + name);
        }

        /// <summary>
        /// 序列化存储CacheFileBean数据
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool serializeCacheFile(CacheFileBean ChFlBean, string name)
        {
            if (null == ChFlBean) return false;
            return SerializeCacheBean2(ChFlBean, currentDirectoryPath + "\\" + name);
        }

        /// <summary>
        /// 序列化存储CacheFileBean数据
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool serializeObject(Object obj, string name)
        {
            if (null == obj) return false;
            return SerializeCacheBean2(obj, currentDirectoryPath + "\\" + name);
        }

        /// <summary>
        /// 序列化增加存储CacheFileBean数据
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool serializeAppend(Object obj,string name)
        {
            if (null == obj) return false;
            return SerializeAppend(obj, currentDirectoryPath + "\\" + name);
        }

        /// <summary>
        /// 删掉文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool deleRestart(string name)
        {
            try 
            {
                File.Delete(currentDirectoryPath + "\\" + name);
            }
            catch {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 反序列化获取存储数据
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        public void deserializeCache(ref Object obs, string name)
        {
            Deserialize(currentDirectoryPath + "\\" + name, ref obs);
        }

        /// <summary>
        /// 序列化存储Object数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Strpath"></param>
        /// <returns></returns>
        private bool SerializeCacheBean2(object  obj, string Strpath)
        {
            FileStream fs = null;
            BinaryFormatter bf = null;
            bool isCache = true;
            var objBean = obj;
            try
            {
                fs = new FileStream(Strpath, FileMode.Create);
                bf = new BinaryFormatter();
                
                if (objBean != null) bf.Serialize(fs, objBean);
            }
            catch (Exception)
            {
                isCache = false;
            }
            finally
            {
                if (null != fs) fs.Close();
            }
            return isCache;
        }

        /// <summary>
        /// 序列化存储Object数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Strpath"></param>
        /// <returns></returns>
        private bool SerializeAppend(object obj, string Strpath)
        {
            FileStream fs = null;
            BinaryFormatter bf = null;
            bool isCache = true;
            var objBean = obj;
            try
            {
                createFile(Strpath);
                    fs = new FileStream(Strpath, FileMode.Append);
                bf = new BinaryFormatter();
                if (objBean != null) bf.Serialize(fs, objBean);
            }
            catch (Exception e)
            {
                isCache = false;
            }
            finally
            {
                if (null != fs) fs.Close();
            }
            return isCache;
        }


        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="StrPath"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Deserialize(string StrPath, ref Object obj)
        {
            FileStream fs = null;
            BinaryFormatter bf = null;
            bool isCache = true;
            try
            {
                fs = new FileStream(currentDirectoryPath + "\\" + StrPath, FileMode.Open);
                bf = new BinaryFormatter();
                obj = bf.Deserialize(fs);
            }
            catch (Exception e) 
            {
                isCache = false;
            }
            finally
            {
                if (null != fs) fs.Close();
                fs = null;
            }
            return isCache;
        }


        //存储二进制对象
        public void addDataInFile(byte[] byteData,string path)
        {
            string filePath = currentDirectoryPath + "\\" + path;
            if (!File.Exists(filePath))
            {
            //    Console.WriteLine("{0} already exists!", filePath);
                return;
            }
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
    
        /// <summary>
        /// 存储数据，path不存在，则创建一个
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="path"></param>
        public void writeDataInFile(byte[] byteData, string path)
        {
            FileStream fs = null;
            BinaryWriter w = null;
            try
            {
                string filePath = currentDirectoryPath + "\\" + path;
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                w = new BinaryWriter(fs);
                w.Write(byteData);
            }
            catch(Exception e)
            {
                Debug.Write("writeDataInFile" + byteData.Length+" ," + e.Message + "\r\n");
            }
            finally 
            {
                if (w != null) w.Flush();
                if (fs != null) fs.Flush();
                if (w != null) w.Close();
                if (fs != null) fs.Close();
            }                      
        }

        //存储二进制对象
        public void addDataInFile(List<byte[]> byteDatas, string path)
        {
            string filePath = currentDirectoryPath + "\\" + path;
            if (!File.Exists(filePath))
            {
                createFile(filePath);
            }
            FileStream fs = null;
            BinaryWriter w = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                w = new BinaryWriter(fs);
                foreach (byte[] btItem in byteDatas)
                {
                    string data = "data";
                    w.Write(System.Text.Encoding.ASCII.GetBytes(data));
                    w.Write(btItem);
                    string datas = "over";
                    w.Write(System.Text.Encoding.ASCII.GetBytes(datas));
                }                
            }
            catch { }
            finally 
            {
                if (w != null) w.Flush();
                if (fs != null) fs.Flush();
                if (w != null) w.Close();
                if (fs != null) fs.Close();
            }            
        }

        //读取数据
        public byte[] getDataFromFile(string path)
        {
            string filePath = currentDirectoryPath + "\\" + path;
            if (!File.Exists(filePath))
            {
             //   Console.WriteLine("{0} already exists!", filePath);
                return null;
            }
            byte[] imageBuffer = null;
            FileStream fs = null;
            BinaryReader br = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                long bufSize = br.BaseStream.Length;
                imageBuffer = new byte[bufSize];
                br.Read(imageBuffer, 0, Convert.ToInt32(bufSize));
                //br.Dispose();
            }
            catch { }
            finally 
            {
                if (br != null) br.Close();
                if (fs != null) fs.Dispose();
                if (fs != null) fs.Close();
            }                       
            return imageBuffer;
        }

        //存储二进制对象
        public void addErrorMsgInFile(string msg, string path,FileMode model)
        {
            string filePath = currentDirectoryPath + "\\" + path;           
            if (!File.Exists(filePath))
            {
                createFile(filePath);
            }
            FileStream fs = null;
            BinaryWriter w = null;
            try
            {
                fs = new FileStream(filePath, model, FileAccess.Write);
                w = new BinaryWriter(fs, System.Text.Encoding.Default);
                w.Write(msg);
                /*foreach (byte[] btItem in byteDatas)
                {
                    string data = "data";
                    w.Write(System.Text.Encoding.ASCII.GetBytes(data));
                    w.Write(btItem);
                    string datas = "over";
                    w.Write(System.Text.Encoding.ASCII.GetBytes(datas));
                }*/
            }
            catch { }
            finally
            {
                if (w != null) w.Flush();
                if (fs != null) fs.Flush();
                if (w != null) w.Close();
                if (fs != null) fs.Close();
            }
        }

        /// <summary>  
        /// 所给路径中所对应的文件大小  
        /// </summary>  
        /// <param name="filePath"></param>  
        /// <returns></returns>  
        public long FileSize(string path)
        {
            long length = 0;
            try 
            {
                string filePath = currentDirectoryPath + "\\" + path;
                //定义一个FileInfo对象，是指与filePath所指向的文件相关联，以获取其大小  
                FileInfo fileInfo = new FileInfo(filePath);
                length =  fileInfo.Length;
            }
            catch { }
            return length;
        }
    }
}
