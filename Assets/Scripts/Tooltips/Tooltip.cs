using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Tooltips
{
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Canvas _canvas;
        
        [SerializeField]
        private GameObject _tooltipPrefab;
        
        [TextArea]
        [SerializeField]
        private string _text;

        private TooltipController _tooltip;

        private void Awake()
        {
            Assert.IsNotNull(_canvas, "_canvas != null");
            Assert.IsNotNull(_tooltipPrefab, "_tooltipPrefab != null");
            
            _tooltip = Instantiate(_tooltipPrefab, _canvas.transform)
                .GetComponent<TooltipController>();
            
            _tooltip.SetCanvas(_canvas);
            _tooltip.SetText(_text);
            
            _tooltip.gameObject.SetActive(false);
        }

        private void EnableTooltip()
        {
            _tooltip.gameObject.SetActive(true);
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
    }
}