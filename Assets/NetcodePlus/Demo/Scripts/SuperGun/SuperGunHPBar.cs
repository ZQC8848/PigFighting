using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NetcodePlus.Demo
{

    public class SuperGunHPBar : MonoBehaviour
    {
        public float maxHealth = 100f;
        public float currentHealth;
        public bool auto_refresh = true;

        public Slider slider;
        private Image fillImage;
        public Color color;

        void Awake()
        {

        }
        private void Start()
        {
            if (slider != null)
            {
                fillImage = slider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
                currentHealth = maxHealth; // ³õÊ¼Ê±ÂúÑª
            }
        }

        void Update()
        {
            if (auto_refresh)
                Refresh();
        }

        public void Refresh()
        {
            if (slider != null)
            {
                fillImage.color = color;
                slider.value = currentHealth / maxHealth;
            }
        }

        public void SetVisible(bool visible)
        {
            if (visible != gameObject.activeSelf)
                gameObject.SetActive(visible);
        }
    }
}
