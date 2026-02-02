using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helpers
{
    // put own helpers up here

    /// <summary>
    /// Returns a random item from a list of Type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T GetRandom<T>(this List<T> list) => list[Random.Range(0, list.Count)];
    

    public static void LogList(List<T> list, string listName = "")
    {
        if (list.Count <= 0 || list == null) 
        {
            Debug.Log($"{listName} Contents:\nList is empty or null.");
            return;
        }

        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrWhiteSpace(listName)) sb.AppendLine("List Contents:");
        else sb.Append($"{listName} Contents:");

        for (int i = 0; i < list.Count(); i++)
        {
            sb.Append(list[i].ToString());
            if(i < list.Count - 1) sb.Append(", ");
        }
        Debug.Log(sb.ToString());
    }

    public static void LogJson(object json, string title = "")
    {
        Debug.Log($"{title}:\n" + JsonConvert.SerializeObject(json, Formatting.Indented));
    }

    public static void CycleEnum<T>(ref T enumValue, int increment = 1) where T : struct, System.Enum
    {
        var values = (T[])System.Enum.GetValues(typeof(T));
        int index = System.Array.IndexOf(values, enumValue);
        index = (index + increment + values.Length) % values.Length;
        enumValue = values[index];
    }

    // The following are inspired by https://youtu.be/JOABOQMurZo
    private static Camera camera;
    public static Camera Camera { get => camera ??= Camera.main; }

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    private static PointerEventData eventDataCurrentPosition;
    private static List<RaycastResult> results;
    public static bool IsOverUI()
    {
        eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    // use i.e. for particle effects on ui position
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }

    public static void DeleteChildren(this Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }
}

public class WeightedList<T>
{

    private List<(T item, int weight)> items = new List<(T item, int weight)>();
    private int totalWeight = 0;

    public WeightedList(params (T item, int weight)[] items)
    {
        this.items = items.ToList();
    }

    public void Add(T item, int weight = 1)
    {
        items.Add((item, weight));
        totalWeight += weight;
    }

    public T GetRandom()
    {
        int roll = UnityEngine.Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (var (item, weight) in items)
        {
            cumulativeWeight += weight;
            if (roll <= cumulativeWeight) return item;
        }

        return items.OrderByDescending(i => i.weight).First().item; // should never reach here if weights are set correctly
    }
}