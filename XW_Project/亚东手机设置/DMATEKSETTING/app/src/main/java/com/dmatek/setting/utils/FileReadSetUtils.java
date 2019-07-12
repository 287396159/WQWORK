package com.dmatek.setting.utils;

import android.os.Environment;
import android.util.Log;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

public class FileReadSetUtils {

    public static String ReadPath = Environment.getExternalStoragePublicDirectory("") + "/111myprint/print.txt";
    public static String WritePath = Environment.getExternalStoragePublicDirectory("") + "/111myprint/print.txt";


    // 读取指定目录下的所有TXT文件的文件内容
    public static String getFileContent(String path) {
        File file = new File(path);
        if (!file.exists()) {
            file.mkdir();
        }
        String content  = "";
        content = getFileInfor(file);
        return content ;
    }


    /**
     * 读取指定目录下的所有TXT文件的文件内容，文件一定要存在
     * @param file
     */
    public static String getFileInfor(File file){
        String content  = "";
        try {
            InputStream instream = new FileInputStream(file);
            if (instream != null) {
                InputStreamReader inputreader
                        = new InputStreamReader(instream, "UTF-8");
                BufferedReader buffreader = new BufferedReader(inputreader);
                String line="";
                //分行读取
                while (( line = buffreader.readLine()) != null) {
                    content += line + "\n";
                }
                instream.close();		//关闭输入流
            }
        } catch (java.io.FileNotFoundException e) {
            Log.d("TestFile", "The File doesn't not exist.");
        } catch (IOException e)  {
            Log.d("TestFile", e.getMessage());
        }
        return content;
    }

    public static void clearFile(String path){
        try {
            // 第三个参数：真，后续内容被追加到文件末尾处，反之则替换掉文件全部内容
            FileWriter fw = new FileWriter(path, false);
            BufferedWriter bw = new BufferedWriter(fw);
            //bw.append(writeMsg);
            bw.write("");
            bw.close();
            fw.close();
        } catch (Exception e) {
            e.printStackTrace();
            Log.e("writeFile", "writeFile: "+e.getMessage());
        }
    }


    public static void writeFile(String path,String writeMsg){
        try {
            // 第三个参数：真，后续内容被追加到文件末尾处，反之则替换掉文件全部内容
            FileWriter fw = new FileWriter(path, true);
            BufferedWriter bw = new BufferedWriter(fw);
            //bw.append(writeMsg);
            bw.write(writeMsg);
            bw.write("\r\n ");
            //bw.write();// 往已有的文件上添加字符串
            //bw.write("def\r\n ");
            //bw.write("hijk ");
            //bw.write("hijk ");
            //bw.write("hijk ");
            bw.close();
            fw.close();
        } catch (Exception e) {
            e.printStackTrace();
            Log.e("writeFile", "writeFile: "+e.getMessage());
        }
    }

    public static void writeFile(String writeMsg){
        try {
            //获取手机本身存储根目录Environment.getExternalStoragePublicDirectory("")
            //sd卡根目录Environment.getExternalStorageDirectory()
            String path = Environment.getExternalStoragePublicDirectory("") + "/111myprint/";
            String fileName = "print.txt";
            File file = new File(path);
            if (!file.exists()) {
                file.mkdir();
            }
            writeFile(path+fileName,writeMsg);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }


    public File createFile(String path)
    {
        File file = new File(path);
        if (!file.exists()) {
            file.mkdir();
        }
        return file;
    }

}
