using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;

public class UIControl : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject settingsEyes;
    public GameObject shopPanel;
    public GameObject shopEyes;
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
    public Button marcoButton;

    [Header("Audio Clips")]
    public AudioClip coffinRaiseClip;
    public AudioClip buttonClickClip;

    [Header("Audio Sources")]
    public AudioSource coffinAudioSource;
    public AudioSource uiAudioSource;
    public AudioSource menuMelody;

    [Header("UI Elements")]
    public Slider buttonVolumeSlider;

    private string currentPanel = "menu";

    private Vector3 coffinStartPos;
    private Vector3 coffinHiddenPos;
    private bool coffinIsHidden = true;

    private Vector3 loadingBarOriginalScale;

    private bool isSliderBeingDragged = false;

    private void Awake()
    {
        coffinStartPos = coffin.position;
        coffinHiddenPos = coffinStartPos + Vector3.down * 120f;
        coffin.position = coffinHiddenPos;

        // Подписываем кнопки на методы с воспроизведением звука клика
        playButton.onClick.AddListener(() => { PlayButtonSound(); OnPlayPressed(); });
        shopButton.onClick.AddListener(() => { PlayButtonSound(); OpenPanel("shop"); });
        settingsButton.onClick.AddListener(() => { PlayButtonSound(); OpenPanel("settings"); });
        languageButton.onClick.AddListener(() => { PlayButtonSound(); ToggleLanguage(); });
        marcoButton.onClick.AddListener(() => { PlayButtonSound(); OpenMarcoLink(); });

        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        settingsEyes.SetActive(false);
        shopPanel.SetActive(false);
        shopEyes.SetActive(false);
        loadingPanel.SetActive(false);

        Transform top = loadingPanel.transform.Find("top");
        if (top != null)
        {
            loadingBarOriginalScale = top.localScale;
            top.localScale = new Vector3(0f, loadingBarOriginalScale.y, loadingBarOriginalScale.z);
        }

        coffinIsHidden = true;

        // Настройка слайдера громкости и подписка на события нажатия/отпускания
        if (buttonVolumeSlider != null && uiAudioSource != null)
        {
            buttonVolumeSlider.value = uiAudioSource.volume;
            buttonVolumeSlider.onValueChanged.AddListener(SetUIVolume);

            // Для звука при начале и конце взаимодействия нужно добавить EventTrigger компоненты
            EventTrigger trigger = buttonVolumeSlider.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = buttonVolumeSlider.gameObject.AddComponent<EventTrigger>();

            // Нажатие (начало перетаскивания)
            var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDown.callback.AddListener((data) => { OnSliderPointerDown(); });
            trigger.triggers.Add(pointerDown);

            // Отпускание (конец перетаскивания)
            var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUp.callback.AddListener((data) => { OnSliderPointerUp(); });
            trigger.triggers.Add(pointerUp);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentPanel != "menu")
                CloseCurrentPanel();
            // НЕ вызываем звук при нажатии ESC
        }
    }

    void OnPlayPressed()
    {
        // При нажатии Play гроб и звук НЕ показываем/не играем
        ShowFakeLoading(() => SceneManager.LoadScene("MainScene"));
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
            settingsEyes.SetActive(true);
            FadePanel(settingsPanel, 2f);
            FadePanel(settingsEyes, 2f);
        }
        else if (name == "shop")
        {
            shopPanel.SetActive(true);
            shopEyes.SetActive(true);
            FadePanel(shopPanel, 2f);
            FadePanel(shopEyes, 2f);
        }

        background.DOShakePosition(0.6f, 25f, 50, 90, false, true);
        coffin.DOMove(coffinStartPos, 1.2f).SetEase(Ease.InOutSine);

        // Звук поднятия гроба при открытии панели настроек или магазина
        coffinAudioSource.Stop();
        coffinAudioSource.PlayOneShot(coffinRaiseClip);

        logo.DOAnchorPosY(500f, 1.2f).SetEase(Ease.OutCubic);

        coffinIsHidden = false;
    }

    void CloseCurrentPanel()
    {
        if (currentPanel == "settings") FadeOutPanel(settingsPanel); FadeOutPanel(settingsEyes);
        if (currentPanel == "shop") FadeOutPanel(shopPanel); FadeOutPanel(shopEyes);

        coffin.DOMove(coffinHiddenPos, 1.2f).SetEase(Ease.InOutSine);

        // Звук опускания гроба
        coffinAudioSource.Stop();
        coffinAudioSource.PlayOneShot(coffinRaiseClip);

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

    void PlayButtonSound()
    {
        if (uiAudioSource != null && buttonClickClip != null)
        {
            uiAudioSource.PlayOneShot(buttonClickClip);
        }
    }

    void SetUIVolume(float volume)
    {
        if (uiAudioSource != null)
            uiAudioSource.volume = volume;
            coffinAudioSource.volume = volume;
        menuMelody.volume = volume;
    }

    // Вызывается при начале взаимодействия с слайдером
    void OnSliderPointerDown()
    {
        if (uiAudioSource != null && buttonClickClip != null)
        {
            uiAudioSource.PlayOneShot(buttonClickClip);
        }
        isSliderBeingDragged = true;
    }

    // Вызывается при окончании взаимодействия с слайдером
    void OnSliderPointerUp()
    {
        if (isSliderBeingDragged && uiAudioSource != null && buttonClickClip != null)
        {
            uiAudioSource.PlayOneShot(buttonClickClip);
        }
        isSliderBeingDragged = false;
    }
}
