using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class InGameButtons : MonoBehaviour
{
    public DataStorage storage;
    public void RestartButton()
    {
        GameLoopManager.win = false;
        GameLoopManager.lose = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevelButton()
    {
        GameLoopManager.win = false;
        GameLoopManager.lose = false;
        if ((SpawnManager.levelIds[0] - 1) * 10 + (SpawnManager.levelIds[1] - 1) == storage.countLevelPasses)
        {
            storage.countLevelPasses++;
            storage.Save();
        }
        if (SpawnManager.levelIds[1] == 10)
        {
            SpawnManager.levelIds[0]++;
            SpawnManager.levelIds[1] = 1;
        }
        else
        {
            SpawnManager.levelIds[1]++;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MenuButton()
    {
        GameLoopManager.win = false;
        GameLoopManager.lose = false;
        SceneManager.LoadScene(6);
    }
}
