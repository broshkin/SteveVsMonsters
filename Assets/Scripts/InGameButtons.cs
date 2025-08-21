using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameButtons : MonoBehaviour
{

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MenuButton()
    {
        GameLoopManager.win = false;
        GameLoopManager.lose = false;
        SceneManager.LoadScene(0);
    }
}
