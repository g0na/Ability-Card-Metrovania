using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private GameObject tempMushroom;
    public GameObject skeleton;
    private GameObject tempSkeleton;
    public GameObject bat;
    private GameObject tempBat;
    
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
        
        StartCoroutine(Patterns());
        

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
            StopCoroutine("Patterns");
            //* 체력이 0 이하라 죽음
            anim.SetTrigger("Death");
            Destroy(gameObject, 3.5f);
            StartCoroutine(showClearWindowAfterSeconds(3f));
            laser.GetComponent<Laser>().damage = 0;
        }
    }

    private IEnumerator Patterns()
    {
        while (curHealth > 0)
        {
            nextPattern = Random.Range(0, 4);

            switch (nextPattern)
            {
                case (0):
                    anim.SetTrigger("Idle");
                    break;
                case (1):
                    StartCoroutine(Stone());
                    break;
                case (2):
                    laserAttack();
                    break;
                case (3):
                    StartCoroutine(Summon());
                    break;
            }
            yield return new WaitForSeconds(5f);
        }
    }
    
    // Stone Pattern
    void SpawnStone()
    {
        float xPos = Random.Range(transform.position.x - 20, transform.position.x + 20);
        Instantiate(stone, new Vector3(xPos, transform.position.y + 15, 0), Quaternion.Euler(0, 0, -90));
    }
    
    private IEnumerator Stone()
    {
        anim.SetTrigger("Stone");
        yield return new WaitForSeconds(1.1f);
        spawnTimes = 0;
        while (spawnTimes < 10)
        {
            SpawnStone();
            yield return new WaitForSeconds(0.3f);
            spawnTimes++;
        }
    }
    
    // Monster Summon Pattern
    private IEnumerator Summon()
    {
        anim.SetTrigger("Summon");
        yield return new WaitForSeconds(1.2f);
        MushroomSummon();
        yield return new WaitForSeconds(0.5f);
        SkeletonSummon();
        yield return new WaitForSeconds(0.5f);
        BatSummon();
    }
    
    void BatSummon()
    {
        float xPos = Random.Range(playerPos.position.x - 10, playerPos.position.x + 10);
        tempBat = Instantiate(bat, new Vector3(xPos, playerPos.position.y + 10, 0), Quaternion.identity);
        Destroy(tempBat, 8f);
    }

    void MushroomSummon()
    {
        tempMushroom = Instantiate(mushroom, new Vector3(-26f, -7.682882f, 0), Quaternion.identity);
        Destroy(tempMushroom, 8f);
    }

    void SkeletonSummon()
    {
        tempSkeleton = Instantiate(skeleton, new Vector3(17.65732f, -13.98288f, 0), Quaternion.identity);
        Destroy(tempSkeleton, 8f);
    }
    
    public void laserAttack()
    {
        laser.SetActive(true);
        laser.transform.localPosition = new Vector3(0, 0, 0);
        laser.transform.rotation = Quaternion.Euler(0, 0, 0);
        laser.GetComponent<Laser>().StartLaserAttack();
        anim.SetTrigger("Idle");
    }

    public void ShowGameClearWindow()
    {
        gameClearWindow.SetActive(true);
    }

    IEnumerator showClearWindowAfterSeconds(float s)
    {
        while (true)
        {
            yield return new WaitForSeconds(s);
            ShowGameClearWindow();
        }        
    }


}
