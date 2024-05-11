using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{


    GameObject sm;
    GameObject gm;

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
            Time.timeScale = 0f;

        }
        else
        {
            sm.GetComponent<StageManager>().isPaused = true;
            Time.timeScale = 1f;
        }       

    }
    public void OnClicMainButton()
    {
        SceneManager.LoadScene("Main");
    }


    public void OnClickCardSelectCloseButton()
    {
        gm.GetComponent<GameManager>().cardSelectWindow.SetActive(false);

    }

    public void OnClickGoStageButton()
    {
        SceneManager.LoadScene("Stage");
    }



}
