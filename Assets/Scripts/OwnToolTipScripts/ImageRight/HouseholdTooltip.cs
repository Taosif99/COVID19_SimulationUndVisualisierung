using UnityEngine;
using UnityEngine.EventSystems;

public class HouseholdTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Tooltip;
    public string TooltipText { get; private set; }

    private void Awake()
    {
        TooltipText = "Place a household on the simulation field \nusing the drag and drop function.";
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
