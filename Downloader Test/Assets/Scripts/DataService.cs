using System.Collections.Generic;
using SqlCipher4Unity3D;
using UnityEngine;

#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif

public class DataService
{
    private readonly SQLiteConnection _connection;

    public DataService(string DatabaseName)
    {
#if UNITY_EDITOR
        string dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
            // check if file exists in Application.persistentDataPath
            string filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

            if (!File.Exists(filepath))
            {
                Debug.Log("Database not in Persistent path");
                // if it doesn't ->
                // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
                WWW loadDb =
     new WWW ("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName); // this is the path to your StreamingAssets in android
                while (!loadDb.isDone) { } // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                // then save to Application.persistentDataPath
                File.WriteAllBytes (filepath, loadDb.bytes);
#elif UNITY_IOS
                string loadDb =
     Application.dataPath + "/Raw/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy (loadDb, filepath);
#elif UNITY_WP8
                string loadDb =
     Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy (loadDb, filepath);
    
#elif UNITY_WINRT
                string loadDb =
     Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy (loadDb, filepath);
#elif UNITY_STANDALONE_OSX
                string loadDb =
     Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#else
                string loadDb =
     Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#endif

                Debug.Log("Database written");
            }

            var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, "password");
        Debug.Log("Final PATH: " + dbPath);
    }

    public void CreateDB()
    {
        _connection.DropTable<DataEntry>();
        _connection.CreateTable<DataEntry>();
        Debug.Log("creating db");

        _connection.InsertAll(new[]
        {
                new DataEntry
                {
                    Id = 1,
                    Nickname = "Tom",
                    Surname = "Perez",
                    Age = 56,
                    Birthday = System.DateTime.ParseExact("25/06/1988", "dd/MM/yyyy", null)
                },
                new DataEntry
                {
                    Id = 2,
                    Nickname = "Fred",
                    Surname = "Arthurson",
                    Age = 16,
                    Birthday = System.DateTime.ParseExact("12/02/1999", "dd/MM/yyyy", null)
                },
                new DataEntry
                {
                    Id = 3,
                    Nickname = "John",
                    Surname = "Doe",
                    Age = 25,
                    Birthday = System.DateTime.ParseExact("05/06/1978", "dd/MM/yyyy", null)
                },
                new DataEntry
                {
                    Id = 4,
                    Nickname = "Roberto",
                    Surname = "Huertas",
                    Age = 37,
                    Birthday = System.DateTime.ParseExact("15/05/2000", "dd/MM/yyyy", null)
                }
            });
    }

    public IEnumerable<DataEntry> GetPersons()
    {
        return _connection.Table<DataEntry>();
    }

    public IEnumerable<DataEntry> GetDataEntrys()
    {
        return _connection.Table<DataEntry>();
    }

    public IEnumerable<DataEntry> GetDataEntrysNamedRoberto()
    {
        return _connection.Table<DataEntry>().Where(x => x.Nickname == "Roberto");
    }

    public DataEntry GetJohnny()
    {
        return _connection.Table<DataEntry>().Where(x => x.Nickname == "Johnny").FirstOrDefault();
    }

    public DataEntry CreateDataEntry()
    {
        DataEntry p = new DataEntry
        {
            Nickname = "Johnny",
            Surname = "Mnemonic",
            Age = 21,
            Birthday = System.DateTime.ParseExact("15/05/2000", "dd/MM/yyyy", null)
        };
        _connection.Insert(p);
        return p;
    }
}
