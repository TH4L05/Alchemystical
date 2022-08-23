using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class saver
{
    public List<int> saveList;
}

public class Saven : MonoBehaviour
{
    public void Save(string path, List<int> list)
    {
        saver s = new saver();

        s.saveList = list;

        if (File.Exists(Application.persistentDataPath + path))
        {
            File.Delete(Application.persistentDataPath + path);
        }

        BinaryFormatter b = new BinaryFormatter();
        FileStream f = File.Create(Application.persistentDataPath + path);
        b.Serialize(f, s);
        f.Close();
    }

    public List<int> Load(string path, List<int> list)
    {
        try
        {
            if (File.Exists(Application.persistentDataPath + path))
            {
                BinaryFormatter b = new BinaryFormatter();
                FileStream f = File.Open(Application.persistentDataPath + path, FileMode.Open);
                saver s = (saver)b.Deserialize(f);
                list = s.saveList;
                f.Close();
                return list;
            }
        }
        catch (IOException)
        {

        }
        return null;
    }
}
