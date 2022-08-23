using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class ArraySaver
{
    public int[] SArray;
}

public class SaveArray : MonoBehaviour
{
    public void Save(string path, int[] array)
    {
        ArraySaver s = new ArraySaver();

        s.SArray = array;

        if (File.Exists(Application.persistentDataPath + path))
        {
            File.Delete(Application.persistentDataPath + path);
        }

        BinaryFormatter b = new BinaryFormatter();
        FileStream f = File.Create(Application.persistentDataPath + path);
        b.Serialize(f, s);
        f.Close();
    }

    public int[] Load(string path, int[] array)
    {
        try
        {
            if (File.Exists(Application.persistentDataPath + path))
            {
                BinaryFormatter b = new BinaryFormatter();
                FileStream f = File.Open(Application.persistentDataPath + path, FileMode.Open);
                ArraySaver s = (ArraySaver)b.Deserialize(f);
                array = s.SArray;
                f.Close();
                return array;
            }
        }
        catch (IOException)
        {

        }
        return null;
    }
}
