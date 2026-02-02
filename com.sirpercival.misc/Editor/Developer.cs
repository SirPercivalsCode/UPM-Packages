using System.IO;
using UnityEditor;
using UnityEngine;


// Most code and idea taken from https://youtu.be/za_pBB80Nt8
public class Developer
{
    //* -----------------------GAME----------------------------
    [MenuItem("Developer/Game/Clear Saves")]
    public static void ClearSaves()
    {
        // Maybe ask for confirmation?

        PlayerPrefs.DeleteAll();
        // Clear serialized Saves

        Logger.Instance.Log("All saves have been cleared.", null, LoggerClass.System);
    }


    [MenuItem("Developer/Game/Cheats/Give Money")]
    public static void GiveMoney()
    {
        // Give player money or smth

        Logger.Instance.Log("Money given to player.", null, LoggerClass.System);
    }


    //* -----------------------EDITOR----------------------------
    [MenuItem("Developer/Editor/Project Setup/Create Folder Structure")]
    public static void CreateFolders()
    {
        string[] folders =
        {
            "Assets/Scripts",
            "Assets/Scripts/Misc",
            "Assets/Scenes",
            "Assets/Sprites"
        };

        foreach (var folder in folders)
        {
            if (!AssetDatabase.IsValidFolder(folder)) Directory.CreateDirectory(folder);
        }

        AssetDatabase.Refresh();
    }

    // taken from https://youtu.be/iAEh7FkY7o4
    [MenuItem("Developer/Editor/Missing Scripts/Find all")]
    public static void FindMissingScriptsMenuItem()
    {
        foreach (GameObject go in GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            foreach (Component component in go.GetComponentsInChildren<Component>())
            {
                if (component == null)
                {
                    Logger.Instance.Log("GameObject found with missing script " + go.name, null, LoggerClass.System);
                    break;
                }
            }
        }
    }

    // taken from https://youtu.be/iAEh7FkY7o4
    [MenuItem("Developer/Editor/Missing Scripts/Delete all")]
    public static void DeleteMissingScriptsMenuItem()
    {
        foreach (GameObject go in GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            foreach (Component component in go.GetComponentsInChildren<Component>())
            {
                if (component == null)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                    break;
                }
            }
        }
    }
}