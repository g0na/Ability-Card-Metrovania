using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float curHealth;
    public float maxHealth;

    public Slider hpSlider;
    public float bossDamage = 2f;
    
    private Animator anim;

    private Transform playerPos;
    
    public GameObject stone;

    private int spawnTimes;

    public GameObject laser;

    public GameObject gameClearWindow;
    
    // Monsters using summon pattern
    public GameObject mushroom;
    public GameObject skeleton;
    public GameObject bat;
    
    private int nextPattern = 0;
    private static int IDLE = 0;
    private static int STONE = 1;
    private static int LASER = 2;
    private static int SUMMON = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        SetHp(30);
        anim = GetComponent<Animator>();
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        
        StartCoroutine(Stone());
        
        laserAttack();

        StartCoroutine(Summon());

        laserAttack();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void SetHp(float amount)
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }
    
    private void CheckHp()
    {
        if (hpSlider != null)
            hpSlider.value = curHealth / maxHealth;
    }

    public void Hit(float damage)
    {
        curHealth -= damage;
        CheckHp();
        if (curHealth <= 0)
        {
            //* 체력이 0 이하라 죽음
            anim.SetTrigger("Death");
            Destroy(gameObject, 3.5f);
            Invoke("ShowGameClearWindow", 3.5f);
        }
    }

    // void Patterns()
    // {
    //     switch (nextPattern)
    //     {
    //
    //     }
    // }
    
    // Stone Pattern
    void SpawnStone()
    {
        float xPos = Random.Range(transform.position.x - 20, transform.position.x + 20);
        Instantiate(stone, new Vector3(xPos, transform.position.y + 15, 0), Quaternion.Euler(0, 0, -90));
    }
    
    private IEnumerator Stone()
    {
        anim.SetTrigger("Stone");
        spawnTimes = 0;
        while (spawnTimes <= 10)
        {
            SpawnStone();
            yield return new WaitForSeconds(0.5f);
            spawnTimes++;
        }
    }
    // Monster Summon Pattern
    void MonsterSummon()
    {
        float xPos = Random.Range(playerPos.position.x - 10, playerPos.position.x + 10);
        Instantiate(mushroom, new Vector3(xPos, playerPos.position.y, 0), Quaternion.identity);
        Instantiate(skeleton, new Vector3(xPos, playerPos.position.y, 0), Quaternion.identity);
        Instantiate(bat, new Vector3(xPos, playerPos.position.y, 0), Quaternion.identity);
    }

    private IEnumerator Summon()
    {
        anim.SetTrigger("Summon");
        MonsterSummon();
        yield return new WaitForSeconds(1f);
    }

    public void laserAttack()
    {
        laser.SetActive(true);
        laser.transform.localPosition = new Vector3(0, 0, 0);
        laser.transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    public void ShowGameClearWindow()
    {
        Debug.Log("?");
        gameClearWindow.SetActive(true);
    }
}
