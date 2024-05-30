using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public static int cardCursor;
    public static int cardType; // 0: main attack, 1: ability, 2: passive skill;

    public static GameObject cardDescription;


    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("StageManager");
        gm = GameObject.Find("GameManager");
        
        mainAttackCardMax = gm.GetComponent<GameManager>().mainAttackCount;
        abilityCardMax = gm.GetComponent<GameManager>().abilityCount;
        passiveCardMax = gm.GetComponent<GameManager>().passiveSkillCount;
        
        mainAttackCards = new GameObject[mainAttackCardMax];
        abilityCards = new GameObject[abilityCardMax];
        passiveSkillCards = new GameObject[passiveCardMax];
        
        if (!gm.GetComponent<GameManager>().onStage)
        {
            cardConfirmWindow = GameObject.Find("Canvas").transform.Find("Card Confirm Window").gameObject;
            cardManagementWindow = GameObject.Find("Canvas").transform.Find("Card Management Window").gameObject;          
        
        }
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
        Debug.Log("exit game");
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

        cardDescription = cardManagementWindow.transform.Find("Card Description").gameObject;

        ChangeCardColor();

    }

    public void ChangeCardColor()
    {
        for (int i = 0; i < mainAttackCardMax; i++)
        {
            mainAttackCards[i].GetComponent<Image>().color = Color.white;
        }
        for (int i = 0; i < abilityCardMax; i++)
        {
            abilityCards[i].GetComponent<Image>().color = Color.white;
        }
        for (int i = 0; i < passiveCardMax; i++)
        {
            passiveSkillCards[i].GetComponent<Image>().color = Color.white;
        }
        mainAttackCards[gm.GetComponent<GameManager>().mainAttack].GetComponent<Image>().color = new Color(95 / 255f, 143 / 255f, 255 / 255f, 255 / 255f);
        abilityCards[gm.GetComponent<GameManager>().ability].GetComponent<Image>().color = new Color(95 / 255f, 143 / 255f, 255 / 255f, 255 / 255f);
        passiveSkillCards[gm.GetComponent<GameManager>().passiveSkill].GetComponent<Image>().color = new Color(95 / 255f, 143 / 255f, 255 / 255f, 255 / 255f);
    }

    // all cards are button
    public void OnClickMainAttackCard0()
    {
        cardCursor = 0;
        cardType = 0;

        // Debug.Log(cardDescription);
        cardDescription.GetComponent<TextMeshProUGUI>().text = "Main Attack : bash" + "\n" + "\n" + "slash a sword to attack enemy";

    }
    public void OnClickMainAttackCard1()
    {
        cardCursor = 1;
        cardType = 0;

        cardDescription.GetComponent<TextMeshProUGUI>().text = "Main Attack : fireball" + "\n" + "\n" + "shot a small fire ball to attack enemy";

    }
    public void OnClickMainAttackCard2()
    {
        cardCursor = 2;
        cardType = 0;

        cardDescription.GetComponent<TextMeshProUGUI>().text = "Main Attack : sword aura" + "\n" + "\n" + "slash a sword and shot a sword aura to attack enemy";

    }
    public void OnClickMainAttackCard3()
    {
        cardCursor = 3;
        cardType = 0;
        Debug.Log(cardCursor);
        Debug.Log(cardType);

        cardDescription.GetComponent<TextMeshProUGUI>().text = "Main Attack : 4th main attack " + "\n" + "\n" + "TBD";


    }
    public void OnClickAbilityCard0()
    {
        cardCursor = 0;
        cardType = 1;

        cardDescription.GetComponent<TextMeshProUGUI>().text = "Main Attack : bash" + "\n" + "\n" + "slash a sword to attack enemy";

    }
    public void OnClickAbilityCard1()
    {
        cardCursor = 1;
        cardType = 1;

        cardDescription.GetComponent<TextMeshProUGUI>().text = "Main Attack : bash" + "\n" + "\n" + "slash a sword to attack enemy";

    }
    public void OnClickAbilityCard2()
    {
        cardCursor = 2;
        cardType = 1;

        cardDescription.GetComponent<TextMeshProUGUI>().text = "Main Attack : bash" + "\n" + "\n" + "slash a sword to attack enemy";

    }
    public void onClickPassiveSkillCard0()
    {
        cardCursor = 0;
        cardType = 2;

        cardDescription.GetComponent<TextMeshProUGUI>().text = "Main Attack : bash" + "\n" + "\n" + "slash a sword to attack enemy";

    }
    public void onClickPassiveSkillCard1()
    {
        cardCursor = 1;
        cardType = 2;

        cardDescription.GetComponent<TextMeshProUGUI>().text = "Main Attack : bash" + "\n" + "\n" + "slash a sword to attack enemy";

    }
    public void onClickPassiveSkillCard2()
    {
        cardCursor = 2;
        cardType = 2;

        cardDescription.GetComponent<TextMeshProUGUI>().text = "Main Attack : bash" + "\n" + "\n" + "slash a sword to attack enemy";

    }

    public void OnClickCardSelectButton()
    {
        if (cardType == 0)
        {
            gm.GetComponent<GameManager>().mainAttack = cardCursor;
        }
        if (cardType == 1)
        {
            gm.GetComponent<GameManager>().ability = cardCursor;

        }
        if (cardType == 2)
        {
            gm.GetComponent<GameManager>().passiveSkill = cardCursor;

        }
        Debug.Log(cardCursor);
        Debug.Log(cardType);
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
