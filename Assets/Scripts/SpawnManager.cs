using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using YG;

public class SpawnManager : MonoBehaviour
{
    private List<spawnInfo> spawns = new List<spawnInfo>();
    private Vector3[] spawnPositions = {new Vector3(8, 2.05f, -2f), new Vector3(8, 0.75f, -3f), new Vector3(8, -0.55f, -4f), new Vector3(8, -1.85f, -5f), new Vector3(8, -3.15f, -6f), new Vector3(1.9f, -1f, -6f) };
    public GameObject[] enemiesPrefab;
    public bool isAllEnemies = false;
    public static int[] levelIds = { 1, 1 };
    public DataStorage storage;

    public GameObject winWindow;
    public GameObject loseWindow;

    public struct spawnInfo
    {
        public float spawnTime;
        public int enemyId;
        public int spawnId;

        public spawnInfo(float spawnTime_, int enemyId_, int spawnId_)
        {
            spawnTime = spawnTime_;
            enemyId = enemyId_;
            spawnId = spawnId_;
        }

    }
   
    // Start is called before the first frame update
    void Start()
    {
        winWindow.SetActive(false);
        loseWindow.SetActive(false);
        StartLevelButton.isStart = false;
        GameLoopManager.win = false;
        GameLoopManager.lose = false;
        //Debug.Log(123);
        int firstNum = levelIds[0];
        int secondNum = levelIds[1];
        Debug.Log(secondNum);
        string contents = File.ReadAllText($"Assets/TextAssets/level{firstNum}_{secondNum}.txt");
        ParseFile(contents);
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameLoopManager.win && !winWindow.activeSelf)
        {
            if ((levelIds[0] - 1) * 10 + (levelIds[1] - 1) == storage.countLevelPasses)
            {
                storage.countLevelPasses++;
                storage.Save();
            }
            winWindow.SetActive(true);
        }
        else if (!GameLoopManager.win)
        {
            winWindow.SetActive(false);
        }

        if (GameLoopManager.lose && !loseWindow.activeSelf)
        {
            loseWindow.SetActive(true);
        }
        else if (!GameLoopManager.lose)
        {
            loseWindow.SetActive(false);
        }
    }

    IEnumerator Spawner()
    {
        yield return new WaitUntil(() => StartLevelButton.isStart);
        for (int i = 0; i < spawns.Count; i++)
        {
            yield return new WaitForSeconds(spawns[i].spawnTime - Time.timeSinceLevelLoad);
            Instantiate(enemiesPrefab[spawns[i].enemyId], spawnPositions[spawns[i].spawnId], enemiesPrefab[spawns[i].enemyId].transform.rotation);
        }
        isAllEnemies = true;
    }

    void ParseFile(string fileContent)
    {
        string[] lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                continue;

            string[] parts = line.Split(';');
            if (parts.Length != 3)
            {
                Debug.LogWarning($"Invalid line format: {line}");
                continue;
            }

            try
            {
                float floatValue = float.Parse(parts[0], CultureInfo.InvariantCulture);
                int intValue1 = int.Parse(parts[1], CultureInfo.InvariantCulture);
                int intValue2 = int.Parse(parts[2], CultureInfo.InvariantCulture);
                spawns.Add(new spawnInfo(floatValue, intValue1, intValue2));
            }
            catch (FormatException e)
            {
                Debug.LogWarning($"Failed to parse line: {line}. Error: {e.Message}");
            }
        }
    }
}
