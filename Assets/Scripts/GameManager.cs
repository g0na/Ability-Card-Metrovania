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
    /// ĳ������ �⺻ ������ ���ϴ� ��ġ
    /// 0: ���� ���� 1: ���Ÿ� ����(��) 2: ���Ÿ� ����(��)
    /// </summary>
    public int mainAttack = 0; 
    /// <summary>
    /// ĳ������ �ɷ��� ���ϴ� ��ġ
    /// 0: �뽬 1: ���� ���� 2: ����
    /// </summary>
    public int ability = 0;
    /// <summary>
    /// ĳ������ �нú� ��ų�� ���ϴ� ��ġ
    /// 0: ���ݷ��̶� �ִ�ü���� 2��
    /// 1: ���ݼӵ��� �̵��ӵ��� 2�� ��������
    /// 2: ��ų ���� ������ �þ�� ���� ȸ���Ѵ�.
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
