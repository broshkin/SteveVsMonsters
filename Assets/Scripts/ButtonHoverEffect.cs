using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color hoverTextColor = new Color(1f, 0.784f, 0.372f);
    private Color originalTextColor;

    private RectTransform rectTransform;
    private TextMeshProUGUI buttonText;

    private Vector3 originalScale;
    
    private Tween scaleTween;
    private Tween colorTween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        if (buttonText != null)
            originalTextColor = buttonText.color;

        originalScale = rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scaleTween != null && scaleTween.IsActive())
        {
            scaleTween.Kill();
        }
        
        scaleTween = rectTransform.DOScale(originalScale * 1.1f, 0.2f).SetEase(Ease.OutSine);

        if (buttonText != null)
        {
            if (colorTween != null && colorTween.IsActive())
            {
                colorTween.Kill();
            }
            colorTween = buttonText.DOColor(hoverTextColor, 0.2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleTween != null && scaleTween.IsActive())
        {
            scaleTween.Kill();
        }
        
        scaleTween = rectTransform.DOScale(originalScale, 0.2f).SetEase(Ease.OutSine);

        if (buttonText != null)
        {
            if (colorTween != null && colorTween.IsActive())
            {
                colorTween.Kill();
            }
            colorTween = buttonText.DOColor(originalTextColor, 0.2f);
        }
    }
    
    public void ResetButtonState()
    {
        if (scaleTween != null && scaleTween.IsActive())
        {
            scaleTween.Kill();
        }
        
        if (colorTween != null && colorTween.IsActive())
        {
            colorTween.Kill();
        }
        
        if (buttonText != null)
        {
            buttonText.color = originalTextColor;
        }
        
        rectTransform.localScale = originalScale;
    }
}
