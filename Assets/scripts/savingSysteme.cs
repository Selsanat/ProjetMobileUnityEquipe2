using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class savingSysteme
{
    public static void SaveData(savingData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saveData.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static savingData LoadData()
    {
        string path = Application.persistentDataPath + "/saveData.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            savingData data = formatter.Deserialize(stream) as savingData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
