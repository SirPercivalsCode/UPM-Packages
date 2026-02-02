using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(RectTransform))]
public class TagSizeAdjuster : MonoBehaviour, ILayoutElement
{

    [SerializeField] private TMP_Text tagText;
    [SerializeField] private float horizontalPadding = 10f;
    [SerializeField] private float btnSize;
    [SerializeField] private RectTransform rectTransform;


    private float calculatedWidth;

    public void Setup(string tagName)
    {
        #region Error Catch
        if (tagText == null) tagText = GetComponentInChildren<TMP_Text>();
        if (string.IsNullOrEmpty(tagName))
        {
            Debug.LogError("TagSizeAdjuster: Tag name cannot be null or empty.");
            return;
        }
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        #endregion

        tagText.text = tagName;
        tagText.ForceMeshUpdate();

        CalculateLayoutInputHorizontal();
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    }

    public void CalculateLayoutInputHorizontal() => AdjustSize();
    
    private bool AdjustSize()
    {
        if (tagText == null || rectTransform == null) return false;

        tagText.ForceMeshUpdate();
        float textWidth = tagText.preferredWidth;
        calculatedWidth = textWidth + horizontalPadding * 2 + btnSize;

        rectTransform.sizeDelta = new Vector2(calculatedWidth, rectTransform.sizeDelta.y);
        return true;
    }

    public void CalculateLayoutInputVertical() { }

    // Layout system asks how wide it should be
    public float minWidth => preferredWidth;
    public float preferredWidth => calculatedWidth;
    public float flexibleWidth => -1;

    // Let layout system determine height, or override if needed
    public float minHeight => -1;
    public float preferredHeight => -1;
    public float flexibleHeight => -1;

    public int layoutPriority => 1;
}
