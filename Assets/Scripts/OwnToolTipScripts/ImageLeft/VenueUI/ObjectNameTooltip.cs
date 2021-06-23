using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectNameTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Tooltip;
    public string TooltipText { get; private set; }

    private void Awake()
    {
        TooltipText = "You can give the venue a name.";
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
