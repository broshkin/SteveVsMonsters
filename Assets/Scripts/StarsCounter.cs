using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsCounter : MonoBehaviour
{
    public int includeNumOfHeroesForThreeStars;
    public int includeNumOfHeroesForTwoStars;
    public int includeNumOfHeroesForOneStar;
    
    public int currentHeroes;

    public static int currentLevel;
    public static int currentStarCount;

    public DataStorage storage;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckInStars()
    {
        if (currentHeroes <= includeNumOfHeroesForThreeStars && currentStarCount < 3)
        {
            Debug.Log("3 звезды!");
            storage.starsNums[currentLevel] = 3;
        }
        else if (currentHeroes <= includeNumOfHeroesForTwoStars && currentStarCount < 2)
        {
            Debug.Log("2 звезды!");
            storage.starsNums[currentLevel] = 2;
        }
        else if (currentHeroes <= includeNumOfHeroesForOneStar && currentStarCount < 1)
        {
            Debug.Log("1 звезда!");
            storage.starsNums[currentLevel] = 1;
        }
        else if (currentStarCount == 0)
        {
            Debug.Log("0 звёзд!");
            storage.starsNums[currentLevel] = 0;
        }
        currentLevel++;
    }
}
