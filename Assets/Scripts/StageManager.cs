using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{


    // when game start, instantiate player object on startpoint.
    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject player;

    [SerializeField] GameObject gameOverWindow;
    [SerializeField] GameObject gameClearWindow;

    public bool isPaused;

    public float mapSize;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate(player, startPoint.transform.position, Quaternion.identity);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowGameOverWindow()
    {
        gameOverWindow.SetActive(true);
    }

    public void ShowGameClearrWindow()
    {
        gameClearWindow.SetActive(true);
        // Time.timeScale = 0f;
    }
}
