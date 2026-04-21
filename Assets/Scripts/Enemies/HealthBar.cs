using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void UpdateHealthBar(float currentvalue, float maxValue)
    {
        slider.value = currentvalue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
