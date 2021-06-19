using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Niwatori
{
    public class GaugeButton : MonoBehaviour
    {
        [SerializeField] private bool reverse;
        private Image gaugeImg;
        private float maxValue;
        [SerializeField] private Color normalColor, highlightColor;
        private void Start()
        {
            gaugeImg = transform.Find("Gauge").GetComponent<Image>();
            gaugeImg.color = normalColor;
        }

        public void SetMaxValue(float max)
        {
            maxValue = max;
        }

        public void SetValue(float value)
        {
            if (gaugeImg != null)
            {
                gaugeImg.fillAmount = reverse ? 1f - value / maxValue : value / maxValue;
            }
        }

        public void SetHighlight()
        {
            if (gaugeImg != null)
                gaugeImg.color = highlightColor;
        }

        public void SetNormalColor()
        {
            if (gaugeImg != null)
                gaugeImg.color = normalColor;
        }
    }
}
