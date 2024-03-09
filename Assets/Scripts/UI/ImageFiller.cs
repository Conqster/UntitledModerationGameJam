using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageFiller : MonoBehaviour
{
    [SerializeField] Gradient colourGradient;
    [SerializeField] float fillAmount;
    [SerializeField] bool inverse = false;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void UpdateFill(float fillAmount, bool inverse = false)
    {
        if (inverse)
        {
            fillAmount = 1f - fillAmount;
        }
        fillAmount = Mathf.Clamp01(fillAmount);

        image.fillAmount = fillAmount;
        image.color = colourGradient.Evaluate(fillAmount);
    }

    private void Update()
    {
        UpdateFill(fillAmount, inverse);
    }
}
