using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class TagManager : MonoBehaviour
{

    [SerializeField] private FlexGridLayoutGroup tagParent;
    private RectTransform tagParentRect;
    [SerializeField] private TMP_InputField tagIF;
    [SerializeField] private TagSizeAdjuster tagPrefab;

    private List<string> tags = new List<string>();

    public UnityEvent OnTagsChanged;

    public static int maxTagCount = 25;
    public static int maxTagLength = 30;


    public void AddTag()
    {
        if(tags.Count >= maxTagCount)
        {
            UIManager.Instance.Toast(ToastType.Warning, $"Maximum tag count {maxTagCount} reached. Please remove a tag before adding a new one.");
            return;
        }

        string tag = tagIF.text.ToLower().Trim();
        if (string.IsNullOrWhiteSpace(tag))
        {
            UIManager.Instance.Toast(ToastType.Warning, "Tag cannot be empty! Please enter a tag.");
            return;
        }

        if(tag.Length > maxTagLength)
        {
            UIManager.Instance.Toast(ToastType.Warning, $"Tag cannot be longer than {maxTagLength} characters.");
            return;
        }

        if (tags.Contains(tag)) // tags.Select(t => t.ToLower()).Contains(tag) we assume all tags are already lowercase now
        {
            UIManager.Instance.Toast(ToastType.Warning, "Tag already added.");
            return;
        }

        OnTagsChanged?.Invoke();
        tagIF.text = string.Empty;
        tagIF.ActivateInputField();

        tags.Add(tag);
        RefreshUI();
        EventSystem.current.SetSelectedGameObject(tagIF.gameObject);
    }

    public void RemoveTag(string tag)
    {
        if (tags.Remove(tag.ToLower()))
        {
            RefreshUI();
            OnTagsChanged?.Invoke();
        }
        else UIManager.Instance.Toast(ToastType.Warning, "Tag not found.");
    }

    public void ClearTags()
    {
        tagParent.transform.DeleteChildren();
        tags.Clear();
        OnTagsChanged?.Invoke();
    }

    //public void LoadIn(List<CampaignTagDto> existingTags) => LoadIn(existingTags?.Select(t => t.TagName.ToLower()).ToList() ?? new List<string>());
    
    public void LoadIn(List<string> existingTags)
    {
        if (existingTags == null || existingTags.Count == 0)
        {
            // Debug.Log("No tags to load.");
            return;
        }

        tags.Clear();
        tags.AddRange(existingTags);
        RefreshUI();
    }

    private void RefreshUI()
    {
        tagParent.transform.DeleteChildren();

        tags.Sort();
        StartCoroutine(RebuildTags());
    }

    private IEnumerator RebuildTags()
    {
        yield return null;  // wait one frame so Destroy() finishes

        for (int i = 0; i < tags.Count; i++)
        {
            TagSizeAdjuster tagGO = Instantiate(tagPrefab, tagParent.transform);
            tagGO.Setup(tags[i]);

            Button btnDelete = tagGO.GetComponentInChildren<Button>();
            string temp = tags[i]; // capture the current tag in a local variable for the listener
            btnDelete.onClick.AddListener(() =>
            {
                RemoveTag(temp);
                Destroy(tagGO.gameObject);
            });
        }

        if (!tagParentRect) tagParentRect = tagParent.GetComponent<RectTransform>();
        LayoutRebuilder.MarkLayoutForRebuild(tagParentRect);
        tagParent.ArrangeChildren();
    }

    // maybe shouldn't verify that here but rather in the API (or both?)
    public List<string> GetTagsAsStrings() =>
        tags.Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.Trim().ToLower())
            .Distinct()
            .ToList();
}
