using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillbarController : MonoBehaviour
{
    CharacterManager character;

    private GameObject[] skillContainers;
    private Image[] skillFills;
    public Transform skillsParent;
    public GameObject skillContainerPrefab;
     
    // Start is called before the first frame update
    void Start()
    {
        character = CharacterManager.Instance;
        skillContainers = new GameObject[CharacterManager.Instance.skillCount];
        skillFills = new Image[CharacterManager.Instance.skillCount];

        CharacterManager.Instance.onSkillChangedCallback += UpdateSkillHUD;
        InstantiateSkillContainers();
        UpdateSkillHUD();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetSkillContainers()
    {
        for (int i = 0; i < skillContainers.Length; i++)
        {
            if (i < CharacterManager.Instance.skillCount)
            {
                skillContainers[i].SetActive(true);
            }
            else
            {
                skillContainers[i].SetActive(false);
            }
        }
    }
    
    void SetFilledSkills()
    {
        for (int i = 0; i < skillFills.Length; i++)
        {
            if (i < CharacterManager.Instance.SkillCount)
            {
                skillFills[i].fillAmount = 1;
            }
            else
            {
                skillFills[i].fillAmount = 0;
            }
        }
    }

    void InstantiateSkillContainers()
    {
        for (int i = 0; i < CharacterManager.Instance.skillCount; i++)
        {
            GameObject temp = Instantiate(skillContainerPrefab);
            temp.transform.SetParent(skillsParent, false);
            skillContainers[i] = temp;
            skillFills[i] = temp.transform.Find("Skillfill").GetComponent<Image>();
        }
    }

    void UpdateSkillHUD()
    {
        SetSkillContainers();
        SetFilledSkills();
    }
}
