using UnityEngine;
using UnityEngine.UI;

public class BaseBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private Gradient gradient;
    public Image fill;

    public void SetBar(int health) {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxBar(int health) {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
}
