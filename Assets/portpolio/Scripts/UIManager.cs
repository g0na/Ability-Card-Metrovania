using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    // 
    GameObject sm;
    GameObject gm;

    // card buttons

    GameObject mainAttackCardMeelee;
    GameObject mainAttackCardRangedAttack1;
    GameObject mainAttackCardRangedAttack2;

    GameObject abilityCardDash;
    GameObject abilityCardDoubleJump;
    GameObject abilityCardBlocking;
    
    // passive cards need to add more
    GameObject passiveCard1; // relate with main attack
    GameObject passiveCard2; // relate with ability
    GameObject passiveCard3; // need to consor


    // for card confirm window
    GameObject cardConfirmWindow;
    
    GameObject currentMainAttackCard;
    GameObject currentAbilityCard;
    GameObject currentPassiveSkillCard;

    // for card management window
    GameObject cardManagementWindow;

    GameObject[] mainAttackCards;
    GameObject[] abilityCards;
    GameObject[] passiveSkillCards;

    int mainAttackCardMax;
    int abilityCardMax;
    int passiveCardMax;


    // Start is called before the first frame update
    void Start()
    {
        // sm = GameObject.Find("StageManager");
        // gm = GameObject.Find("GameManager");
        //
        // mainAttackCardMax = gm.GetComponent<GameManager>().mainAttackCount;
        // abilityCardMax = gm.GetComponent<GameManager>().abilityCount;
        // passiveCardMax = gm.GetComponent<GameManager>().passiveSkillCount;
        //
        // mainAttackCards = new GameObject[mainAttackCardMax];
        // abilityCards = new GameObject[abilityCardMax];
        // passiveSkillCards = new GameObject[passiveCardMax];
        //
        // if (!gm.GetComponent<GameManager>().onStage)
        // {
        //     cardConfirmWindow = GameObject.Find("Canvas").transform.Find("Card Confirm Window").gameObject;
        //     cardManagementWindow = GameObject.Find("Canvas").transform.Find("Card Management Window").gameObject;          
        //
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnClickStartButton()
    {
        cardConfirmWindow.SetActive(true);
    }

    public void OnClickExitButton()
    {
        Debug.Log("onclickButton");
        Application.Quit();
    }

    public void OnClickPauseButton()
    {
        if (sm.GetComponent<StageManager>().isPaused)
        {
            sm.GetComponent<StageManager>().isPaused = false;
            Time.timeScale = 1f;

        }
        else
        {
            sm.GetComponent<StageManager>().isPaused = true;
            Time.timeScale = 0f;
        }       

    }
    public void OnClicMainButton()
    {
        gm.GetComponent<GameManager>().onStage = false;
        SceneManager.LoadScene("Main");
    }


    public void OnClickCardConfirmWindowCloseButton()
    {
        cardConfirmWindow.SetActive(false);

    }

    public void OnClickGoStageButton()
    {
        gm.GetComponent<GameManager>().onStage = true;
        SceneManager.LoadScene("Stage");
    }

    /*
    public void OnClickActivateRangedAttack()
    {
        if (gm.GetComponent<GameManager>().activeRangedAttack)
        {
            gm.GetComponent<GameManager>().activeRangedAttack = false;
            this.GetComponent<Image>().color = Color.white;
        }
        else
        {
            gm.GetComponent<GameManager>().activeRangedAttack = true;
            this.GetComponent<Image>().color = Color.blue;

        }
    }

    public void OnClickActivateDash()
    {
        if (gm.GetComponent<GameManager>().activeDash)
        {
            gm.GetComponent<GameManager>().activeDash = false;
            this.GetComponent<Image>().color = Color.white;
        }
        else
        {
            gm.GetComponent<GameManager>().activeDash = true;
            this.GetComponent<Image>().color = Color.blue;

        }
    }
    */

    // for card management window setting

    public void OnClickCardManagementButton()
    {
        cardManagementWindow.SetActive(true);
        OnLoadCardManagementWindow();
    }

    public void OnClickCardManagementWindowCloseButton()
    {
        cardManagementWindow.SetActive(false);
    }
    public void OnLoadCardManagementWindow()
    {
        for (int i = 0; i < mainAttackCardMax; i++)
        {
            // Debug.Log(i);
            mainAttackCards[i] = cardManagementWindow.transform.Find("Main Attacks").Find("Main Attack Card" + i).gameObject;
            // Debug.Log(mainAttackCards[i].ToString());
        }
        for (int i = 0; i < abilityCardMax; i++)
        {
            abilityCards[i] = cardManagementWindow.transform.Find("Abilities").Find("Ability Card" + i).gameObject;

        }
        for (int i = 0; i < passiveCardMax; i++)
        {
            passiveSkillCards[i] = cardManagementWindow.transform.Find("Passive Skills").Find("Passive Skill Card" + i).gameObject;

        }

        mainAttackCards[gm.GetComponent<GameManager>().mainAttack].GetComponent<Image>().color = Color.blue;
        abilityCards[gm.GetComponent<GameManager>().ability].GetComponent<Image>().color = Color.blue;
        passiveSkillCards[gm.GetComponent<GameManager>().passiveSkill].GetComponent<Image>().color = Color.blue;

    }

    // all cards are button
    public void OnClickMainAttackCard0()
    {
        gm.GetComponent<GameManager>().mainAttack = 0;
        OnLoadCardManagementWindow();
    }


    public void OnLoadCardConfirmWindow()
    {
       // currentMainAttackCard = GameObject.Find("");
    }

    public void OnstartReload()
    {

    }

    public void UpdateUI()
    {

    }


}
