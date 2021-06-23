using UnityEngine;
using UnityEngine.EventSystems;

public class SaveTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Tooltip;
    public string TooltipText { get; private set; }

    private void Awake()
    {
        TooltipText = "Save the current simulation to the savefile.";
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
