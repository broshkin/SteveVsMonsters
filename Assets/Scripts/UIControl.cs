using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

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
    
    [Header("Button Effects")]
    public ButtonHoverEffect playButtonEffect;
    public ButtonHoverEffect shopButtonEffect;
    public ButtonHoverEffect settingsButtonEffect;
    public ButtonHoverEffect marcoButtonEffect;

    [Header("Audio Clips")]
    public AudioClip coffinRaiseClip;
    public AudioClip buttonClickClip;

    [Header("Audio Sources")]
    public AudioSource coffinAudioSource;
    public AudioSource uiAudioSource;
    public AudioSource menuMelody;

    [Header("UI Elements")]
    public Slider buttonVolumeSlider;

    [Header("Chapter Selection System")]
    public GameObject chooseChapterPanel;
    public GameObject chooesLevelPanel;
    public RectTransform moon;
    public Button leftButton;
    public Button rightButton;
    public Button firstChapterButton;
    public Button secondChapterButton;
    
    public GameObject firstChapterPanel;
    public GameObject secondChapterPanel;

    public DataStorage storage;

    private RectTransform firstChapterRect;
    private RectTransform secondChapterRect;
    
    private Vector2 logoStartPos;
    private Vector2 playButtonStartPos;
    private Vector2 shopButtonStartPos;
    private Vector2 settingsButtonStartPos;
    private Vector2 marcoButtonStartPos;

    private string currentPanel = "menu";

    private Vector3 coffinStartPos;
    private Vector3 coffinHiddenPos;
    private bool coffinIsHidden = true;

    private Vector3 loadingBarOriginalScale;

    private bool isSliderBeingDragged = false;

    private bool isChapterSelectionActive = false;
    private int currentChapterIndex = 0;
    private Vector2 backgroundStartPos;
    private Vector2 starsStartPos;
    private Vector2 moonStartPos;
    private Vector2 firstChapterStartPos;
    private Vector2 secondChapterStartPos;
    private Vector2 firstChapterCenterPos;
    private Vector2 secondChapterCenterPos;
    private Vector2 firstChapterLeftPos;
    private Vector2 secondChapterLeftPos;
    private Vector2 firstChapterRightPos;
    private Vector2 secondChapterRightPos;
    
    private Vector2 firstChapterOriginalPos;
    private Vector2 secondChapterOriginalPos;
    private Vector2 playButtonOriginalPos;
    private Vector2 shopButtonOriginalPos;
    private Vector2 settingsButtonOriginalPos;
    private Vector2 marcoButtonOriginalPos;
    private Vector3 firstChapterCenterScale = Vector3.one;
    private Vector3 secondChapterCenterScale = Vector3.one;
    private Vector3 firstChapterSideScale = new Vector3(0.7f, 0.7f, 0.7f);
    private Vector3 secondChapterSideScale = new Vector3(0.7f, 0.7f, 0.7f);

    private void Awake()
    {
        coffinStartPos = coffin.position;
        coffinHiddenPos = coffinStartPos + Vector3.down * 120f;
        coffin.position = coffinHiddenPos;

        firstChapterRect = firstChapterButton.GetComponent<RectTransform>();
        secondChapterRect = secondChapterButton.GetComponent<RectTransform>();

        playButton.onClick.AddListener(() => { PlayButtonSound(); OnPlayPressed(); });
        shopButton.onClick.AddListener(() => { PlayButtonSound(); OpenPanel("shop"); });
        settingsButton.onClick.AddListener(() => { PlayButtonSound(); OpenPanel("settings"); });
        marcoButton.onClick.AddListener(() => { PlayButtonSound(); OpenMarcoLink(); });

        leftButton.onClick.AddListener(() => { PlayButtonSound(); OnLeftButtonPressed(); });
        rightButton.onClick.AddListener(() => { PlayButtonSound(); OnRightButtonPressed(); });
        firstChapterButton.onClick.AddListener(() => { PlayButtonSound(); OnFirstChapterSelected(); });
        secondChapterButton.onClick.AddListener(() => { PlayButtonSound(); OnSecondChapterSelected(); });
        
        secondChapterButton.interactable = false;

        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        settingsEyes.SetActive(false);
        shopPanel.SetActive(false);
        shopEyes.SetActive(false);
        loadingPanel.SetActive(false);
        
        chooseChapterPanel.SetActive(false);
        firstChapterPanel.SetActive(false);
        secondChapterPanel.SetActive(false);

        Transform top = loadingPanel.transform.Find("top");
        loadingBarOriginalScale = top.localScale;
        top.localScale = new Vector3(0f, loadingBarOriginalScale.y, loadingBarOriginalScale.z);

        coffinIsHidden = true;

        buttonVolumeSlider.value = uiAudioSource.volume;
        buttonVolumeSlider.onValueChanged.AddListener(SetUIVolume);

        EventTrigger trigger = buttonVolumeSlider.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = buttonVolumeSlider.gameObject.AddComponent<EventTrigger>();

        var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        pointerDown.callback.AddListener((data) => { OnSliderPointerDown(); });
        trigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        pointerUp.callback.AddListener((data) => { OnSliderPointerUp(); });
        trigger.triggers.Add(pointerUp);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentPanel == "chapters")
            {
                HideChapterSelection();
            }
            else if (currentPanel != "menu")
            {
                CloseCurrentPanel();
            }
        }
        
        if (isChapterSelectionActive)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnLeftButtonPressed();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnRightButtonPressed();
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                if (currentChapterIndex == 0)
                {
                    OnFirstChapterSelected();
                }
                else
                {
                    OnSecondChapterSelected();
                }
            }
        }
    }

    void OnPlayPressed()
    {
        ShowChapterSelection();
    }

    public void ShowFakeLoading(System.Action onComplete)
    {
        menuPanel.SetActive(false);
        loadingPanel.SetActive(true);

        Transform top = loadingPanel.transform.Find("top");
        top.localScale = new Vector3(0f, loadingBarOriginalScale.y, loadingBarOriginalScale.z);

        top.DOScale(loadingBarOriginalScale, 2.5f)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
            {
                loadingPanel.SetActive(false);
                onComplete?.Invoke();
            });
    }

    public void StartGame()
    {
        ShowFakeLoading(() => SceneManager.LoadScene("MainScene"));
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
        else if (name == "chapters")
        {
            chooseChapterPanel.SetActive(true);
            firstChapterPanel.SetActive(true);
            secondChapterPanel.SetActive(true);
            ShowChapterSelection();
        }

            background.DOShakePosition(0.6f, 25f, 50, 90, false, true);
        coffin.DOMove(coffinStartPos, 1.2f).SetEase(Ease.InOutSine);

        coffinAudioSource.Stop();
        coffinAudioSource.PlayOneShot(coffinRaiseClip);

        logo.DOAnchorPosY(560f, 1.2f).SetEase(Ease.OutCubic);

        coffinIsHidden = false;
    }

    public void CloseCurrentPanel()
    {
        if (currentPanel == "settings") 
        { 
            FadeOutPanel(settingsPanel); 
            FadeOutPanel(settingsEyes);
            ResetAllButtonStates();
        }
        if (currentPanel == "shop") 
        { 
            FadeOutPanel(shopPanel); 
            FadeOutPanel(shopEyes);
            ResetAllButtonStates();
        }
        if (currentPanel == "chapters") 
        {
            chooseChapterPanel.SetActive(false);
            firstChapterPanel.SetActive(false);
            secondChapterPanel.SetActive(false);
        }

        coffin.DOMove(coffinHiddenPos, 1.2f).SetEase(Ease.InOutSine);

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
        if (langText.text == "RU") langText.text = "ENG";
        else langText.text = "RU";
    }

    void OpenMarcoLink()
    {
        Application.OpenURL("https://t.me/marcostudio");
    }

    void PlayButtonSound()
    {
        uiAudioSource.PlayOneShot(buttonClickClip);
    }

    void SetUIVolume(float volume)
    {
        uiAudioSource.volume = volume;
        coffinAudioSource.volume = volume;
        menuMelody.volume = volume;
    }

    void OnSliderPointerDown()
    {
        uiAudioSource.PlayOneShot(buttonClickClip);
        isSliderBeingDragged = true;
    }

    void OnSliderPointerUp()
    {
        if (isSliderBeingDragged)
        {
            uiAudioSource.PlayOneShot(buttonClickClip);
        }
        isSliderBeingDragged = false;
    }

    private void InitializeChapterSelectionPositions()
    {
        if (firstChapterOriginalPos == Vector2.zero)
        {
            firstChapterOriginalPos = firstChapterButton.GetComponent<RectTransform>().anchoredPosition;
            secondChapterOriginalPos = new Vector2(firstChapterOriginalPos.x + 600f, firstChapterOriginalPos.y);
            
            var playButtonRect = playButton.GetComponent<RectTransform>();
            playButtonOriginalPos = playButtonRect.anchoredPosition;
            
            var shopButtonRect = shopButton.GetComponent<RectTransform>();
            shopButtonOriginalPos = shopButtonRect.anchoredPosition;
            
            var settingsButtonRect = settingsButton.GetComponent<RectTransform>();
            settingsButtonOriginalPos = settingsButtonRect.anchoredPosition;
            
            var marcoButtonRect = marcoButton.GetComponent<RectTransform>();
            marcoButtonOriginalPos = marcoButtonRect.anchoredPosition;
        }

        backgroundStartPos = background.anchoredPosition;
        moonStartPos = moon.anchoredPosition;
        firstChapterStartPos = firstChapterOriginalPos;
        secondChapterStartPos = secondChapterOriginalPos;
        
        logoStartPos = logo.anchoredPosition;

        playButtonStartPos = playButtonOriginalPos;
        shopButtonStartPos = shopButtonOriginalPos;
        settingsButtonStartPos = settingsButtonOriginalPos;
        marcoButtonStartPos = marcoButtonOriginalPos;

        firstChapterCenterPos = firstChapterStartPos;
        secondChapterCenterPos = firstChapterStartPos;

        firstChapterLeftPos = firstChapterStartPos + Vector2.left * 600f;
        secondChapterLeftPos = firstChapterStartPos + Vector2.left * 600f;

        firstChapterRightPos = firstChapterStartPos + Vector2.right * 600f;
        secondChapterRightPos = secondChapterStartPos;

        firstChapterCenterScale = Vector3.one;
        secondChapterCenterScale = Vector3.one;
        firstChapterSideScale = Vector3.one * 0.7f;
        secondChapterSideScale = Vector3.one * 0.7f;

        firstChapterRect.anchoredPosition = firstChapterCenterPos;
        firstChapterRect.localScale = firstChapterCenterScale;
        secondChapterRect.anchoredPosition = secondChapterStartPos;
        secondChapterRect.localScale = secondChapterSideScale;
        
        currentChapterIndex = 0;
    }

    void ShowChapterSelection()
    {
        isChapterSelectionActive = true;
        currentPanel = "chapters";
        
        InitializeChapterSelectionPositions();
        
        chooseChapterPanel.SetActive(true);
        FadePanel(chooseChapterPanel, 0.8f);

        Vector2 backgroundTargetPos = new Vector2(background.anchoredPosition.x, backgroundStartPos.y - 1080f);
        background.DOAnchorPos(backgroundTargetPos, 1.2f).SetEase(Ease.InOutSine);

        Vector2 moonTargetPos = new Vector2(moon.anchoredPosition.x, moonStartPos.y - 100f);
        moon.DOAnchorPos(moonTargetPos, 1.2f).SetEase(Ease.InOutSine);
        Vector2 logoTargetPos = new Vector2(logoStartPos.x, logoStartPos.y - 1080f);
        logo.DOAnchorPos(logoTargetPos, 1.2f).SetEase(Ease.InOutSine);

        var shopButtonRect = shopButton.GetComponent<RectTransform>();
        Vector2 shopTargetPos = new Vector2(shopButtonStartPos.x, shopButtonStartPos.y - 1080f);
        shopButtonRect.DOAnchorPos(shopTargetPos, 1.2f).SetEase(Ease.InOutSine);

        var playButtonRect = playButton.GetComponent<RectTransform>();
        Vector2 playTargetPos = new Vector2(playButtonStartPos.x, playButtonStartPos.y - 1080f);
        playButtonRect.DOAnchorPos(playTargetPos, 1.2f).SetEase(Ease.InOutSine);

        var settingsButtonRect = settingsButton.GetComponent<RectTransform>();
        Vector2 settingsTargetPos = new Vector2(settingsButtonRect.anchoredPosition.x, settingsButtonStartPos.y - 1080f);
        settingsButtonRect.DOAnchorPos(settingsTargetPos, 1.2f).SetEase(Ease.InOutSine);
    
        var marcoButtonRect = marcoButton.GetComponent<RectTransform>();
        Vector2 marcoTargetPos = new Vector2(marcoButtonRect.anchoredPosition.x, marcoButtonStartPos.y - 1080f);
        marcoButtonRect.DOAnchorPos(marcoTargetPos, 1.2f).SetEase(Ease.InOutSine);

        firstChapterRect.localScale = Vector3.zero;
        firstChapterRect.DOScale(firstChapterCenterScale, 0.8f).SetEase(Ease.OutBack).SetDelay(0.3f);
        secondChapterRect.localScale = Vector3.zero;
        secondChapterRect.DOScale(secondChapterSideScale, 0.8f).SetEase(Ease.OutBack).SetDelay(0.5f);
        firstChapterPanel.SetActive(true);
        secondChapterPanel.SetActive(true);
        
        SetInitialNavigationState();
    }

    void ShowLevelSelection(GameObject chapterPanel)
    {
        chooesLevelPanel.SetActive(true);
    }

    public void HideChapterSelection()
    {
        chooesLevelPanel.SetActive(false);
        isChapterSelectionActive = false;
        currentPanel = "menu";
        
        background.DOAnchorPosY(backgroundStartPos.y, 1.2f)
            .SetEase(Ease.InOutSine);
        
        moon.DOAnchorPosY(moonStartPos.y, 1.2f)
            .SetEase(Ease.InOutSine);
        
        logo.DOAnchorPosY(logoStartPos.y, 1.2f)
            .SetEase(Ease.InOutSine);
        
        shopButton.GetComponent<RectTransform>().DOAnchorPosY(shopButtonStartPos.y, 1.2f)
            .SetEase(Ease.InOutSine);
        
        settingsButton.GetComponent<RectTransform>().DOAnchorPosY(settingsButtonStartPos.y, 1.2f)
            .SetEase(Ease.InOutSine);
        
        marcoButton.GetComponent<RectTransform>().DOAnchorPosY(marcoButtonStartPos.y, 1.2f)
            .SetEase(Ease.InOutSine);
        
        playButton.GetComponent<RectTransform>().DOAnchorPosY(playButtonStartPos.y, 1.2f)
            .SetEase(Ease.InOutSine);
        
        firstChapterRect.DOScale(Vector3.zero, 0.6f)
            .SetEase(Ease.InBack)
            .OnComplete(() => firstChapterPanel.SetActive(false));
        
        secondChapterRect.DOScale(Vector3.zero, 0.6f)
            .SetEase(Ease.InBack)
            .OnComplete(() => secondChapterPanel.SetActive(false));
        
        FadeOutPanel(chooseChapterPanel);
        menuPanel.SetActive(true);
        
        currentChapterIndex = 0;
    }

    void OnLeftButtonPressed()
    {
        if (currentChapterIndex == 0) return;
        
        currentChapterIndex = 0;
        UpdateChapterPositions();
        UpdateNavigationButtons();
    }
    
    void OnRightButtonPressed()
    {
        if (currentChapterIndex == 1) return;
        
        currentChapterIndex = 1;
        UpdateChapterPositions();
        UpdateNavigationButtons();
    }
    
    void OnFirstChapterSelected()
    {
        ShowLevelSelection(firstChapterPanel);
        //ShowFakeLoading(() => SceneManager.LoadScene("MainScene"));
    }
    
    void OnSecondChapterSelected()
    {
        uiAudioSource.PlayOneShot(buttonClickClip);

        secondChapterPanel.transform.DOShakePosition(0.5f, 10f, 50, 90, false, true);

        secondChapterButton.transform.DOScale(secondChapterButton.transform.localScale * 0.9f, 0.1f)
            .OnComplete(() => {
                secondChapterButton.transform.DOScale(secondChapterButton.transform.localScale / 0.9f, 0.1f);
            });
        
        StartCoroutine(ShowLockedMessage());
        StartCoroutine(ReturnToFirstChapterAfterDelay(3f));
    }
    
    private System.Collections.IEnumerator ShowLockedMessage()
    {
        yield return new WaitForSeconds(2f);
    }
    
    private System.Collections.IEnumerator ReturnToFirstChapterAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        currentChapterIndex = 0;
        UpdateChapterPositions();
        UpdateNavigationButtons();
    }
    
    void UpdateChapterPositions()
    {
        if (currentChapterIndex == 0)
        {
            firstChapterRect.DOAnchorPos(firstChapterCenterPos, 0.8f).SetEase(Ease.InOutSine);
            firstChapterRect.DOScale(firstChapterCenterScale, 0.8f).SetEase(Ease.InOutSine);
            secondChapterRect.DOAnchorPos(secondChapterRightPos, 0.8f).SetEase(Ease.InOutSine);
            secondChapterRect.DOScale(secondChapterSideScale, 0.8f).SetEase(Ease.InOutSine);
        }
        else
        {
            firstChapterRect.DOAnchorPos(firstChapterLeftPos, 0.8f).SetEase(Ease.InOutSine);
            firstChapterRect.DOScale(firstChapterSideScale, 0.8f).SetEase(Ease.InOutSine);
            secondChapterRect.DOAnchorPos(secondChapterCenterPos, 0.8f).SetEase(Ease.InOutSine);
            secondChapterRect.DOScale(secondChapterCenterScale, 0.8f).SetEase(Ease.InOutSine);
        }
        
        UpdateNavigationButtons();
    }
    
    void UpdateNavigationButtons()
    {
        bool leftActive = (currentChapterIndex > 0);
        leftButton.interactable = leftActive;

        bool rightActive = (currentChapterIndex < 1);
        rightButton.interactable = rightActive;

        firstChapterButton.interactable = (currentChapterIndex == 0);
        secondChapterButton.interactable = false;
    }
    
    void SetInitialNavigationState()
    {
        currentChapterIndex = 0;
        UpdateNavigationButtons();
    }
    
    private IEnumerator SmoothMoveButton(RectTransform button, Vector2 targetPos, float duration)
    {
        Vector2 startPos = button.anchoredPosition;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);
            
            button.anchoredPosition = Vector2.Lerp(startPos, targetPos, easedProgress);
            yield return null;
        }
        
        button.anchoredPosition = targetPos;
    }
    
    private void ResetAllButtonStates()
    {
        if (playButtonEffect != null) playButtonEffect.ResetButtonState();
        if (shopButtonEffect != null) shopButtonEffect.ResetButtonState();
        if (settingsButtonEffect != null) settingsButtonEffect.ResetButtonState();
        if (marcoButtonEffect != null) marcoButtonEffect.ResetButtonState();
    }

    public void Test()
    {
        storage.countLevelPasses++;
        storage.Save();
    }
    public void ResetTest()
    {
        storage.countLevelPasses = 0;
        storage.Save();
    }
} 