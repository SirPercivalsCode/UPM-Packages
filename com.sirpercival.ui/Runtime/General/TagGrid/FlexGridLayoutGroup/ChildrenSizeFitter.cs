using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildrenSizeFitter : MonoBehaviour
{
    [SerializeField] private bool useWidth, useHeight, runOnUpdate;
    [SerializeField] private Vector2 padding, minSize;
    [SerializeField] private RectTransform parent;
    [SerializeField] private List<RectTransform> accountedChildren = new List<RectTransform>();
    [SerializeField] private VerticalLayoutGroup vLayoutGroup;
    [SerializeField] private HorizontalLayoutGroup hLayoutGroup;

    private bool useAllChildren, useVLayoutGroup, useHLayoutGroup;

    private void Update()
    {
        if(runOnUpdate) ResizeParentToChildren();
    }

    public void ResizeParentToChildren()
    {
        if(parent == null) parent = GetComponent<RectTransform>();
        if(parent == null) throw new MissingReferenceException("Parent not specified");
        if(useHeight && vLayoutGroup == null) vLayoutGroup = GetComponent<VerticalLayoutGroup>();
        if(useWidth && hLayoutGroup == null) hLayoutGroup = GetComponent<HorizontalLayoutGroup>();

        useVLayoutGroup = vLayoutGroup != null;
        useHLayoutGroup = hLayoutGroup != null;
        useAllChildren = accountedChildren.Count <= 0;

        float totalHeight = 0f;
        float totalWidth= 0f;
        if (useAllChildren) foreach (RectTransform child in parent)
            {
                if (!child.gameObject.activeInHierarchy) continue;
                if (useWidth)
                {
                    totalWidth += child.sizeDelta.x;
                    if (useHLayoutGroup) totalWidth += hLayoutGroup.spacing;
                }
                if (useHeight)
                {
                    totalHeight += child.sizeDelta.y;
                    if (useVLayoutGroup) totalHeight += vLayoutGroup.spacing;
                }
                
            }
        else foreach (RectTransform child in accountedChildren)
            {
                if (!child.gameObject.activeInHierarchy) continue;
                if (useWidth)
                {
                    totalWidth += child.sizeDelta.x;
                    if (useHLayoutGroup) totalWidth += hLayoutGroup.spacing;
                }
                if (useHeight)
                {
                    totalHeight += child.sizeDelta.y;
                    if (useVLayoutGroup) totalHeight += vLayoutGroup.spacing;
                }
            }

        if (useWidth) totalWidth += padding.x;
        if (useHeight) totalHeight += padding.y;
        if (useHLayoutGroup && useWidth)
        {
            totalWidth -= hLayoutGroup.spacing;
            totalWidth += hLayoutGroup.padding.left + hLayoutGroup.padding.right;
        }
        if(useVLayoutGroup && useHeight)
        {
            totalHeight -= vLayoutGroup.spacing;
            totalHeight += vLayoutGroup.padding.top + vLayoutGroup.padding.bottom;
        }
        if(totalWidth < minSize.x) totalWidth = minSize.x;
        if(totalHeight < minSize.y) totalHeight = minSize.y;

        // if (useAllChildren) Debug.Log($"using children to determine size on {gameObject.name}: {totalWidth}|{totalHeight}");
        Vector2 size = parent.sizeDelta;
        if(useWidth) size.x = totalWidth;
        if(useHeight) size.y = totalHeight;
        parent.sizeDelta = size;
    }
}
