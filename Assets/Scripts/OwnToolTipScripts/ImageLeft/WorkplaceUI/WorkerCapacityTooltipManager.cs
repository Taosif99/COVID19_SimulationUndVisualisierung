using UnityEngine;
using TMPro;

public class WorkerCapacityTooltipManager : MonoBehaviour
{
    private static WorkerCapacityTooltipManager instance;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private WorkerCapacityTooltip workerCapacityInputField;

    private RectTransform backgroundRectTransform;
    private TextMeshProUGUI tooltipText;
    private RectTransform rectTransform;

    private void Awake()
    {
        instance = this;
        tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        rectTransform = transform.GetComponent<RectTransform>();
        SetText(workerCapacityInputField.TooltipText);
    }

    private void SetText(string tooltipText)
    {
        this.tooltipText.SetText(tooltipText);
        this.tooltipText.ForceMeshUpdate();

        Vector2 paddingSize = new Vector2(8, 8);
        Vector2 textSize = this.tooltipText.GetRenderedValues(false);
        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }

    private void Update()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        //To check if tooltip left screen on right side
        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }

        //To check if tooltip left screen on top side
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }
        rectTransform.anchoredPosition = anchoredPosition + new Vector2(5f, 10f);
    }
}