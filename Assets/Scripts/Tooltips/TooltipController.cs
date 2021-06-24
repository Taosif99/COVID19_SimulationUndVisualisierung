using TMPro;
using UnityEngine;

namespace Tooltips
{
    public class TooltipController : MonoBehaviour
    {
        private Canvas _canvas;
        private RectTransform _canvasRectTransform;
        private RectTransform _rectTransform;
        private TMP_Text _text;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _text = GetComponentInChildren<TMP_Text>();
        }

        public void SetCanvas(Canvas canvas)
        {
            _canvas = canvas;
            _canvasRectTransform = _canvas.gameObject.GetComponent<RectTransform>();
        }
        
        public void SetText(string tooltipText)
        {
            _text.text = tooltipText;
        }
        
        private void Update()
        {
            Vector2 anchoredPosition = Input.mousePosition / _canvas.scaleFactor;

            //To check if tooltip left screen on right side
            if (anchoredPosition.x + _rectTransform.rect.width > _canvasRectTransform.rect.width)
            {
                anchoredPosition.x = _canvasRectTransform.rect.width - _rectTransform.rect.width;
            }
            
            //To check if tooltip left screen on top side
            if (anchoredPosition.y + _rectTransform.rect.height > _canvasRectTransform.rect.height)
            {
                anchoredPosition.y = _canvasRectTransform.rect.height - _rectTransform.rect.height;
            }

            _rectTransform.anchoredPosition = anchoredPosition; // + new Vector2(5f, 10f);
        }
    }
}