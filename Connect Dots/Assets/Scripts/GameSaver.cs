using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class GameSaver 
{
    private static string Path
    {
        get
        {
            return System.IO.Path.Combine(Application.dataPath, "save.json");
        }
    }

    public static void Save(GameState gameState)
    {
        var json = JsonConvert.SerializeObject(gameState);
        File.WriteAllText(Path, json);
    }

    public static GameState Load()
    {
        if (File.Exists(Path))
        {
            string json = File.ReadAllText(Path);
            GameState state = JsonConvert.DeserializeObject<GameState>(json);
            return state;
        }
        return null;
    }

}
