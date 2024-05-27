using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


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



    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("StageManager");
        gm = GameObject.Find("GameManager");
        // cardSelectWindow = GameObject.Find("Card Select Window");
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnClickStartButton()
    {
        gm.GetComponent<GameManager>().cardSelectWindow.SetActive(true);
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


    public void OnClickCardSelectCloseButton()
    {
        gm.GetComponent<GameManager>().cardSelectWindow.SetActive(false);

    }

    public void OnClickGoStageButton()
    {
        gm.GetComponent<GameManager>().onStage = true;
        SceneManager.LoadScene("Stage");
    }


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

    public void OnClickCardManagementButton()
    {

    }


    public void OnstartReload()
    {

    }

    public void UpdateUI()
    {

    }


}
