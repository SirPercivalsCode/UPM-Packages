using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class CustomContentFitter : MonoBehaviour, ILayoutSelfController
{
    [SerializeField] private bool fitHorizontal;
    [SerializeField] private bool fitVertical;
    [SerializeField] private RectTransform rect;
    [SerializeField] private VerticalLayoutGroup vlg;
    [SerializeField] private HorizontalLayoutGroup hlg;


    [SerializeField] private float horizontalPadding = 0f;
    [SerializeField] private float verticalPadding = 0f;



    private Vector2 lastSize = Vector2.zero;

    void OnEnable()
    {
        rect = GetComponent<RectTransform>();
        vlg = GetComponent<VerticalLayoutGroup>();
        hlg = GetComponent<HorizontalLayoutGroup>();
        LayoutRebuilder.MarkLayoutForRebuild(rect);
    }


    private bool isAdjusting = false;
    //[Button("Adjust Size")]
    private void AdjustSize()
    {
        if (isAdjusting) return;
        isAdjusting = true;
        if (!fitHorizontal && !fitVertical)
        {
            Debug.LogWarning("ContentFitter not in use. activate fitvertical or horizontal.");
            return;
        }

        if (!rect) rect = GetComponent<RectTransform>();

        float totalHeight = 0f;
        float totalWidth = 0f;
        int activeChildCount = 0;

        foreach (RectTransform child in transform)
        {
            if (!child.gameObject.activeSelf) continue;
            if (fitVertical)
            {
                float preferredHeight = child.sizeDelta.y;
                totalHeight += preferredHeight;
                //Debug.Log($"{child.name} preferred height: {preferredHeight}\nTotal: {totalHeight}");
            }

            if (fitHorizontal)
            {
                float preferredWidth = child.sizeDelta.x;
                totalWidth += preferredWidth;
                //Debug.Log($"{child.name} preferred width: {preferredWidth}\nTotal: {totalWidth}");
            }

            activeChildCount++;
        }

        if (fitVertical && vlg != null && activeChildCount > 1)
        {
            totalHeight += vlg.spacing * (activeChildCount - 1);
            totalHeight += vlg.padding.top + vlg.padding.bottom;
        }
        if (fitHorizontal && hlg != null && activeChildCount > 1)
        {
            totalWidth += hlg.spacing * (activeChildCount - 1);
            totalWidth += hlg.padding.left + hlg.padding.right;
        }

        float width = fitHorizontal ? totalWidth + horizontalPadding : rect.sizeDelta.x;
        float height = fitVertical ? totalHeight + verticalPadding : rect.sizeDelta.y;

        // Debug.Log($"Vert size: {height}");

        Vector2 newSize = new Vector2(width, height);
        if (rect.sizeDelta != newSize)
        {
            rect.sizeDelta = newSize;
            ForceParentLayoutUpdate();
            lastSize = newSize;
        }

        isAdjusting = false;
    }

    private void ForceParentLayoutUpdate()
    {
        if (rect == null || rect.parent == null) return;

        // Only rebuild if really needed
        if (rect.sizeDelta != lastSize)
        {
            RectTransform parentRect = rect.parent as RectTransform;
            if (parentRect != null)
                LayoutRebuilder.MarkLayoutForRebuild(parentRect); // lighter than ForceRebuildLayoutImmediate
        }
    }

    public void SetLayoutHorizontal()
    {
        AdjustSize();
    }

    public void SetLayoutVertical()
    {
        AdjustSize();
    }
}
