using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro; // ← добавляем поддержку TextMeshPro

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color hoverTextColor = new Color(1f, 0.784f, 0.372f); // ffc85f
    private Color originalTextColor;

    private RectTransform rectTransform;
    private TextMeshProUGUI buttonText; // ← используем TMP

    private Vector3 originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>(); // ← ищем TMP текст

        if (buttonText != null)
            originalTextColor = buttonText.color;
        else
            Debug.LogWarning("TextMeshProUGUI не найден на кнопке!");

        originalScale = rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOKill();
        rectTransform.DOScale(originalScale * 1.1f, 0.2f).SetEase(Ease.OutSine);

        if (buttonText != null)
        {
            buttonText.DOKill();
            buttonText.DOColor(hoverTextColor, 0.2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOKill();
        rectTransform.DOScale(originalScale, 0.2f).SetEase(Ease.OutSine);

        if (buttonText != null)
        {
            buttonText.DOKill();
            buttonText.DOColor(originalTextColor, 0.2f);
        }
    }
}
