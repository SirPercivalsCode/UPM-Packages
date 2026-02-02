using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlexGridLayoutGroup : MonoBehaviour, ILayoutElement
{
    [Header("Settings")]
    [SerializeField] private bool adjustableHeight;
    [SerializeField] private int maxRows = 5;

    [Header("Spacing")]
    [SerializeField] private float horizontalSpacing = 5f;
    [SerializeField] private float verticalSpacing = 5f;
    [SerializeField] private float childHeight = 35f;
    [SerializeField] private RectTransform rect;

    private float height;

    public float minWidth => -1;

    public float preferredWidth => -1;

    public float flexibleWidth => -1;

    public float minHeight => childHeight;

    public float preferredHeight => height;

    public float flexibleHeight => -1;

    public int layoutPriority => 1;


    private void OnEnable()
    {
        ArrangeChildren();
    }

    [Button("Arrange Children")]
    public void ArrangeChildren()
    {
        if (rect == null) rect = GetComponent<RectTransform>();

        // LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

        float containerWidth = rect.rect.width;
        float posX = 0f;
        int row = 0;

        foreach (RectTransform child in transform)
        {
            float childWidth = child.sizeDelta.x;
            // Debug.Log($"ContainerWidth: {containerWidth} | ChildSizeX {child.sizeDelta.x}");

            if (posX + childWidth > containerWidth)
            {
                row++;
                posX = 0;
            }
            
            float x = posX + childWidth * 0.5f;
            float y = -row * (childHeight + verticalSpacing) - childHeight * 0.5f;

            child.sizeDelta = new Vector2(childWidth, childHeight);
            child.anchoredPosition = new Vector2(x, y);

            posX += childWidth + horizontalSpacing;
        }

        if (adjustableHeight)
        {
            int rowCount = row < maxRows ? row + 1 : maxRows;
            float rowHeight = childHeight + verticalSpacing;
            height = rowCount * rowHeight + verticalSpacing;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
            ForceParentLayoutUpdate();
        }
    }
    private void ForceParentLayoutUpdate()
    {
        if (rect == null || rect.parent == null) return;

        RectTransform parentRect = rect.parent as RectTransform;
        if (parentRect != null)
            LayoutRebuilder.MarkLayoutForRebuild(parentRect);
    }

    public void CalculateLayoutInputHorizontal()
    { }
    public void CalculateLayoutInputVertical()
    {
        ArrangeChildren();
    }
}
