using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChooseHeroButton : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isChoose = false;
    [SerializeField] private GameObject heroButtonPrefab;
    [SerializeField] private GameObject newParent;
    public GameObject heroButton;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        heroButton = Instantiate(heroButtonPrefab, transform);
        heroButton.GetComponent<HeroButton>().ChooseHeroButton = gameObject.GetComponent<ChooseHeroButton>();
        heroButton.transform.localPosition = Vector3.zero;
        heroButton.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Button>().enabled = !isChoose;
    }

    public void OnClick()
    {
        if (newParent.GetComponent<ActiveHeroesPanel>().countOfHeroes < 6)
        {
            isChoose = true;
            heroButton.transform.parent = newParent.transform;
            var panel = newParent.GetComponent<ActiveHeroesPanel>();
            for (int i = 0; i < panel.freeWindows.Length; i++)
            {
                if (panel.freeWindows[i])
                {
                    panel.freeWindows[i] = false;
                    heroButton.GetComponent<HeroButton>().currentPosId = i;
                    heroButton.transform.localPosition = new Vector3(newParent.GetComponent<ActiveHeroesPanel>().xOffsets[i], -4f, 0f);
                    break;
                }
            }
            heroButton.transform.localScale = Vector3.one;
            newParent.GetComponent<ActiveHeroesPanel>().countOfHeroes++;
        }   
    }
}
