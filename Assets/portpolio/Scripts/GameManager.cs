using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // [Header("Character Ability")]
    public bool activeRangedAttack;
    public bool activeDash;
    public bool onStage;

    // character main attack 
    // character active skill
    // character passive skill

    /*
    public const int mainAttackCount = 3;
    public const int abilityCount = 3;
    public const int passiveSkillCount = 3;
    */

    public int mainAttackCount = 3;
    public int abilityCount = 3;
    public int passiveSkillCount = 3;

    public int mainAttack = 0;
    public int ability = 0;
    public int passiveSkill = 0;
   
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


    }



}
