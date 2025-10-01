using System.IO;
using UnityEngine;

public class LoadFiles
{


    public string[] LoadAllFilePath(string _path)
    {
        if (Directory.Exists(_path))
        {
            return Directory.GetFiles(_path);
        }
        else
        {
            Debug.Log("Not Found File In " + _path);
            return null;
        }
    }
}
