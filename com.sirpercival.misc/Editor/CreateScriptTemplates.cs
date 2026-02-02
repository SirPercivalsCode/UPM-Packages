using UnityEditor;

// not mine, modified slightly
// allows to easily create different kinds of scripts (i.e. interfaces, SOs, etc.)
public static class CreateScriptTemplates
{

    [MenuItem("Assets/Create/Code/MonoBehaviour", priority = 0)]
    public static void CreateMonoBehaviourMenuItem()
    {
        string templatePath = "Assets/_Scripts/Misc/Editor/Templates/MonoBehaviour.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewMonoBehaviour.cs");
    }

    [MenuItem("Assets/Create/Code/ScriptableObject", priority = 1)]
    public static void CreateScriptableObjectMenuItem()
    {
        string templatePath = "Assets/_Scripts/Misc/Editor/Templates/ScriptableObject.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScriptableObject.cs");
    }

    [MenuItem("Assets/Create/Code/Singleton", priority = 2)]
    public static void CreateSingletonMenuItem()
    {
        string templatePath = "Assets/_Scripts/Misc/Editor/Templates/Singleton.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewSingleton.cs");
    }

    [MenuItem("Assets/Create/Code/DontDestroySingleton", priority = 3)]
    public static void CreateDontDestroySingletonMenuItem()
    {
        string templatePath = "Assets/_Scripts/Misc/Editor/Templates/DontDestroySingleton.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewSingleton.cs");
    }

    [MenuItem("Assets/Create/Code/SerializableClass", priority = 5)]
    public static void CreateSerializableClassMenuItem()
    {
        string templatePath = "Assets/_Scripts/Misc/Editor/Templates/SerializableClass.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewClass.cs");
    }

    [MenuItem("Assets/Create/Code/Interface", priority = 6)]
    public static void CreateInterfaceMenuItem()
    {
        string templatePath = "Assets/_Scripts/Misc/Editor/Templates/Interface.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewInterface.cs");
    }

    [MenuItem("Assets/Create/Code/Enum", priority = 7)]
    public static void CreateEnumMenuItem()
    {
        string templatePath = "Assets/_Scripts/Misc/Editor/Templates/Enum.cs.txt";

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewEnum.cs");
    }
}
