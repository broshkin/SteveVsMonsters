using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.ProBuilder.MeshOperations;

public class Levels : MonoBehaviour
{
    public List<GameObject> levelButtons;
    public int chapter;
    public Sprite openLevelImage;
    public Sprite closeLevelImage;
    public Sprite unfillStar;
    public Sprite fillStar;
    public UIControl uiControl;
    public DataStorage storage;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var levelButton in GetComponentsInChildren<LevelButton>())
        {
            levelButtons.Add(levelButton.gameObject);
        }
        foreach (var levelButton in levelButtons)
        {
            levelButton.GetComponent<Button>().onClick.AddListener(() => SetLevelIds(levelButtons.IndexOf(levelButton) + 1));
            levelButton.GetComponent<Button>().onClick.AddListener(uiControl.StartGame);
            levelButton.GetComponent<Button>().onClick.AddListener(() => StarsCounter.currentLevel = ((chapter - 1) * 10) + levelButtons.IndexOf(levelButton));
            levelButton.GetComponent<Button>().onClick.AddListener(() => StarsCounter.currentStarCount = levelButton.GetComponent<LevelButton>().starsNum);

        }

        for (int i = 0; i < levelButtons.Count; i++)
        {
            levelButtons[i].GetComponent<LevelButton>().num = i;
            if ((chapter - 1) * 10 + i <= storage.countLevelPasses)
            {
                levelButtons[i].GetComponent<LevelButton>().isOpen = true;
            }
            levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = (((chapter - 1) * 10) + i + 1).ToString();
        }

        foreach (var levelButton in levelButtons)
        {
            if (levelButton.GetComponent<LevelButton>().isOpen)
            {
                levelButton.GetComponent<LevelButton>().zamok.SetActive(false);
                levelButton.GetComponent<Image>().sprite = openLevelImage;
                levelButton.GetComponent<Button>().enabled = true;
            }
            else
            {
                levelButton.GetComponent<LevelButton>().zamok.SetActive(true);
                levelButton.GetComponent<Image>().sprite = closeLevelImage;
                levelButton.GetComponent<Button>().enabled = false;
            }

            levelButton.GetComponent<LevelButton>().Init(((chapter - 1) * 10 + levelButtons.IndexOf(levelButton)) == storage.countLevelPasses);
        }
    }

    

    public void SetLevelIds(int levelID)    
    {
        Debug.Log(levelID);
        SpawnManager.levelIds = new int[] { chapter, levelID};
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(storage.countLevelPasses);
        
    }
}
