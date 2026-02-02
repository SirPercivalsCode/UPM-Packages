using UnityEngine;

public static class PersistentData
{

    public static TestingType testingType;
    public static bool gameInitialized = false;

    public static bool IsOffline => testingType == TestingType.Offline;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnRuntimeStart() => Reset();

    public static void Reset()
    {
        Debug.Log("PersistentData was Reset.");
        gameInitialized = false;
        testingType = TestingType.Live;
    }
}

public enum TestingType
{
    Offline,
    Local,
    Live
}