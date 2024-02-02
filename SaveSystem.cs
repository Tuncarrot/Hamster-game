using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static string GetPath => Application.persistentDataPath + "/playerStats.json";

    public static void CreatePlayer(Player player)
    {
        //BinaryFormatter formatter = new BinaryFormatter();

        Debug.Log(Application.persistentDataPath.ToString());

        if (File.Exists(GetPath))
        {
            // File was created already, do nothing
            Debug.Log("MOBILE LOG >>> PLAYER FILE ALREADY EXISTS");
        }
        else
        {
            // Will create default player under default settings
            SavePlayer(player);
            Debug.Log("MOBILE LOG >>> WILL CREATE NEW PLAYER FILE");
        }

    }

    public static void SavePlayer(Player player)
    {
        //BinaryFormatter formatter = new BinaryFormatter();
        string json = JsonUtility.ToJson(player);

        File.WriteAllText(GetPath, json);
        //FileStream stream = new FileStream(json, FileMode.Create);

        PlayerStats data = new PlayerStats(player);

        Debug.Log("MOBILE LOG >>> SAVING PLAYER DATA TO FILE..." + data.ToString());

        //formatter.Serialize(stream, data);
        //stream.Close();
    }

    public static PlayerStats LoadPlayer()
    {
        Debug.Log(Application.persistentDataPath.ToString());

        if (File.Exists(GetPath))
        {
            // BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(GetPath, FileMode.Open);

            string contents;
            using (var sr = new StreamReader(stream))
            {
                contents = sr.ReadToEnd();
            }

            PlayerStats data = JsonUtility.FromJson<PlayerStats>(contents);

            //PlayerStats data = formatter.Deserialize(stream) as PlayerStats;

            Debug.Log(data);

            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save File Not Found");
        }

        return null;
    }
}
