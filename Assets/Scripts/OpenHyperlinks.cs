using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Script to realize the opnening of pages enclosed in "<link> </link>" -Tags.
/// </summary>
public class OpenHyperlinks : MonoBehaviour, IPointerClickHandler
{
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var pTextMeshPro = GetComponent<TextMeshProUGUI>();
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, Input.mousePosition, null);
        if (linkIndex != -1)
        { 
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];

            
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}
