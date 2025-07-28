using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;

public class UIControl : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject shopPanel;
    public GameObject loadingPanel;

    [Header("UI Elements")]
    public RectTransform logo;
    public Transform coffin;
    public RectTransform background;

    [Header("Buttons")]
    public Button playButton;
    public Button shopButton;
    public Button settingsButton;
    public Button languageButton;
    public Button marcoButton;  // Кнопка для перехода на телеграм

    private string currentPanel = "menu";

    private Vector3 coffinStartPos;
    private Vector3 coffinHiddenPos;
    private bool coffinIsHidden = true;

    private Vector3 loadingBarOriginalScale;

    private void Awake()
    {
        coffinStartPos = coffin.position;
        coffinHiddenPos = coffinStartPos + Vector3.down * 120f;
        coffin.position = coffinHiddenPos;

        // Навешиваем методы на кнопки программно
        playButton.onClick.AddListener(OnPlayPressed);
        shopButton.onClick.AddListener(() => OpenPanel("shop"));
        settingsButton.onClick.AddListener(() => OpenPanel("settings"));
        languageButton.onClick.AddListener(ToggleLanguage);
        marcoButton.onClick.AddListener(OpenMarcoLink);

        // Изначально показываем меню, скрываем остальные
        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        shopPanel.SetActive(false);
        loadingPanel.SetActive(false);

        // Запоминаем масштаб полосы загрузки и обнуляем ее
        Transform top = loadingPanel.transform.Find("top");
        if (top != null)
        {
            loadingBarOriginalScale = top.localScale;
            top.localScale = new Vector3(0f, loadingBarOriginalScale.y, loadingBarOriginalScale.z);
        }

        coffinIsHidden = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentPanel != "menu")
                CloseCurrentPanel();
        }
    }

    void OnPlayPressed()
    {
        ShowFakeLoading(() => SceneManager.LoadScene("MainScene"));
        coffinIsHidden = false;
    }

    void ShowFakeLoading(System.Action onComplete)
    {
        menuPanel.SetActive(false);
        loadingPanel.SetActive(true);

        Transform top = loadingPanel.transform.Find("top");
        if (top != null)
        {
            top.localScale = new Vector3(0f, loadingBarOriginalScale.y, loadingBarOriginalScale.z);

            top.DOScale(loadingBarOriginalScale, 2.5f)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() =>
                {
                    loadingPanel.SetActive(false);
                    onComplete?.Invoke();
                });
        }
        else
        {
            Debug.LogWarning("top image not found!");
            onComplete?.Invoke();
        }
    }

    void OpenPanel(string name)
    {
        menuPanel.SetActive(false);
        currentPanel = name;

        if (name == "settings")
        {
            settingsPanel.SetActive(true);
            FadePanel(settingsPanel, 2f);
        }
        else if (name == "shop")
        {
            shopPanel.SetActive(true);
            FadePanel(shopPanel, 2f);
        }

        background.DOShakePosition(0.6f, 25f, 50, 90, false, true);
        coffin.DOMove(coffinStartPos, 1.2f).SetEase(Ease.InOutSine);
        logo.DOAnchorPosY(500f, 1.2f).SetEase(Ease.OutCubic);

        coffinIsHidden = false;
    }

    void CloseCurrentPanel()
    {
        if (currentPanel == "settings") FadeOutPanel(settingsPanel);
        if (currentPanel == "shop") FadeOutPanel(shopPanel);

        coffin.DOMove(coffinHiddenPos, 1.2f).SetEase(Ease.InOutSine);
        logo.DOAnchorPosY(0f, 1.2f).SetEase(Ease.OutCubic);

        menuPanel.SetActive(true);
        currentPanel = "menu";

        coffinIsHidden = true;
    }

    void FadePanel(GameObject panel, float duration = 1.2f)
    {
        var cg = panel.GetComponent<CanvasGroup>();
        if (!cg) cg = panel.AddComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.DOFade(1, duration);
    }

    void FadeOutPanel(GameObject panel)
    {
        var cg = panel.GetComponent<CanvasGroup>();
        if (!cg) cg = panel.AddComponent<CanvasGroup>();
        cg.DOFade(0, 0.5f).OnComplete(() => panel.SetActive(false));
    }

    void ToggleLanguage()
    {
        TextMeshProUGUI langText = languageButton.GetComponentInChildren<TextMeshProUGUI>();
        if (langText != null)
        {
            if (langText.text == "RU") langText.text = "ENG";
            else langText.text = "RU";
        }
    }

    void OpenMarcoLink()
    {
        Application.OpenURL("https://t.me/marcostudio");
    }
}
