using UnityEngine;
using UnityEngine.EventSystems;

public class PlayTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Tooltip;
    public string TooltipText { get; private set; }

    private void Awake()
    {
        TooltipText = "Start or continue the simulation.";
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
