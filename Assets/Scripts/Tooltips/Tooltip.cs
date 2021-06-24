using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Tooltips
{
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private GameObject _tooltipPrefab;
        
        [TextArea(3, 10)]
        [SerializeField]
        private string _text;

        private TooltipController _tooltip;

        private void Awake()
        {
            Assert.IsNotNull(_tooltipPrefab, "_tooltipPrefab != null");

            Canvas canvas = DetermineParentingCanvas();
            Assert.IsNotNull(canvas, "canvas != null");
            
            _tooltip = Instantiate(_tooltipPrefab, canvas.transform)
                .GetComponent<TooltipController>();
            
            _tooltip.SetCanvas(canvas);
            _tooltip.SetText(_text);
            
            _tooltip.gameObject.SetActive(false);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Invoke(nameof(EnableTooltip), 0.5f);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            CancelInvoke(nameof(EnableTooltip));
            _tooltip.gameObject.SetActive(false);
        }

        private Canvas DetermineParentingCanvas()
        {
            Transform parent = transform.parent;
            while (parent != null)
            {
                Canvas canvas;
                if ((canvas = parent.GetComponent<Canvas>()) != null)
                {
                    return canvas;
                }
                
                parent = parent.parent;
            }

            return null;
        }

        private void EnableTooltip()
        {
            _tooltip.gameObject.SetActive(true);
        }
    }
}