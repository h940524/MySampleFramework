using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

public class CheckTextureFormat
{
    private static readonly string formatLog = "TextureFormat";

    private static readonly string LogOneFile = "FileOne.txt";

    private static readonly string LogTwoFile = "FileTwo.txt";

    private static readonly string LogThreeFile = "FileThree.txt";


    [MenuItem("Setting/Check/Texture")]
    public static void CheckFormat()
    {
        string rootPath = Path.Combine(Application.dataPath, formatLog);
        if (!Directory.Exists(rootPath))
        {
            Directory.CreateDirectory(rootPath);
            using (StreamWriter fileOneRead = File.CreateText(Path.Combine(rootPath, LogOneFile)))
            {
                fileOneRead.Close();
            }
            using (StreamWriter fileTwoRead = File.CreateText(Path.Combine(rootPath, LogTwoFile)))
            {
                fileTwoRead.Close();
            }
            using (StreamWriter fileThreeRead = File.CreateText(Path.Combine(rootPath, LogThreeFile)))
            {
                fileThreeRead.Close();
            }
        }

        CheckTextureSize(Application.dataPath);

        Debug.Log("check finish");
        AssetDatabase.Refresh();
    }

    [MenuItem("Setting/Set/SetAndroid")]
    public static void SetAndroidFormat()
    {
		
    }

    [MenuItem("Setting/Set/SetIos")]
    public static void SetIosFormat()
    {

    }

    private static void CheckTextureSize(string path)
    {
        DirectoryInfo root = new DirectoryInfo(path);
        // 文件夹
        DirectoryInfo[] dirArray = root.GetDirectories();
        // 文件
        FileInfo[] fileArray = root.GetFiles();


        // 文件夹继续递归
        for (int i = 0; i < dirArray.Length; i++)
        {
            CheckTextureSize(dirArray[i].ToString());
        }

        string rootPath = Path.Combine(Application.dataPath, formatLog);

        // 文件检测
        for (int i = 0; i < fileArray.Length; i++)
        {
            if (fileArray[i].Name.EndsWith(".meta")) continue;

            if (fileArray[i].Name.EndsWith(".png") || fileArray[i].Name.EndsWith(".jpg") || fileArray[i].Name.EndsWith(".tga"))
            {
                int index = fileArray[i].ToString().IndexOf("Assets");
                string assetPath = fileArray[i].ToString().Substring(index, fileArray[i].ToString().ToCharArray().Length - index);
                Texture tex = (Texture)AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
                if (tex.width % 2 != 0 || tex.height % 2 != 0)
                {
                    using (StreamWriter sw = File.AppendText(Path.Combine(rootPath, LogOneFile)))
                    {
                        sw.WriteLine(fileArray[i].ToString());
                    }
                }
                else if (tex.width > 1024 || tex.height > 1024)
                {
                    using (StreamWriter sw = File.AppendText(Path.Combine(rootPath, LogTwoFile)))
                    {
                        sw.WriteLine(fileArray[i].ToString());
                    }
                }
                else if (tex.width != tex.height)
                {
                    using (StreamWriter sw = File.AppendText(Path.Combine(rootPath, LogThreeFile)))
                    {
                        sw.WriteLine(fileArray[i].ToString());
                    }
                }
            }
        }

    }
}
