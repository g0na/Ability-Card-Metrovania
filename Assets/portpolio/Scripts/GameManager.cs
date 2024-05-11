using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject cardSelectWindow;

    // [Header("Character Ability")]
    public bool activeRangedAttack;
    public bool activeDash;
    public bool onStage;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        onStage = false;
    
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!onStage)
        {
            if (cardSelectWindow == null)
            {
                cardSelectWindow = GameObject.Find("Canvas").transform.Find("Card Select Window").gameObject;

            }
        }

    }



}
