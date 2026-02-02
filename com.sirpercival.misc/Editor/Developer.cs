using System.IO;
using UnityEditor;
using UnityEngine;

public class Developer
{
    //* -----------------------GAME----------------------------
    [MenuItem("Developer/Game/Clear Saves")]
    public static void ClearSaves()
    {
        // Maybe ask for confirmation?

        PlayerPrefs.DeleteAll();
        // Clear serialized Saves

        Debug.Log("All saves have been cleared.");
    }


    [MenuItem("Developer/Game/Cheats/Give Money")]
    public static void GiveMoney()
    {
        // Give player money or smth

        Debug.Log("Money given to player.");
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
                    Debug.Log($"GameObject found with missing script {go.name}");
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