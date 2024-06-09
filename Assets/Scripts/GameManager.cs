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

    // exact count
    public int mainAttackCount = 3;
    public int abilityCount = 3;
    public int passiveSkillCount = 3;
    /// <summary>
    /// 캐릭터의 기본 공격을 정하는 수치
    /// 0: 근접 공격 1: 원거리 공격(약) 2: 원거리 공격(강)
    /// </summary>
    public int mainAttack = 0; 
    /// <summary>
    /// 캐릭터의 능력을 정하는 수치
    /// 0: 대쉬 1: 더블 점프 2: 막기
    /// </summary>
    public int ability = 0;
    /// <summary>
    /// 캐릭터의 패시브 스킬을 정하는 수치
    /// 0: 공격력이랑 최대체력이 2배
    /// 1: 공격속도와 이동속도가 2배 빨라진다
    /// 2: 스킬 구슬 갯수가 늘어나고 빨리 회복한다.
    /// </summary>
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
