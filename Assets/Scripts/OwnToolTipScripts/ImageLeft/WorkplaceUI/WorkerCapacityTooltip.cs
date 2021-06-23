using UnityEngine;
using UnityEngine.EventSystems;

public class WorkerCapacityTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Tooltip;
    public string TooltipText { get; private set; }

    private void Awake()
    {
        TooltipText = "You can define how many work in this workplace." +
                      "\nPlease only enter whole numbers." +
                      "\nExample: 12";
    }

    public void Start()
    {
        if (Tooltip != null)
            Tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventdata)
    {
        if (Tooltip != null)
            Tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventdata)
    {
        if (Tooltip != null)
        {
            Tooltip.SetActive(false);
        }
    }
}