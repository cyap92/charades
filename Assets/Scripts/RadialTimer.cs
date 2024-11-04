using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RadialTimer : MonoBehaviour
{
    [SerializeField] private Image image;  

    public void SetTimerPercentage(float percentage)
    {
        image.fillAmount = percentage;
    }
}
