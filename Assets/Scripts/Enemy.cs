using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Enemy : MonoBehaviour
{
    public float enemyMaxHp = 100;
    [SerializeField] float enemyHp;
    [SerializeField] public float enemyDamage = 1f;
    float starthp;

    GameObject player;
    protected Animator anim;

    GameObject hpbar;
    public GameObject monsterHPbar;

    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");        
        anim = GetComponent<Animator>();

        enemyHp = enemyMaxHp;
        hpbar = Instantiate(monsterHPbar, this.transform);
        starthp = hpbar.transform.localScale.y;

    }


    // Update is called once per frame
    protected virtual void Update()
    {

        if (enemyHp <= 0)
        {
            Destroy(gameObject);
        }

        hpbar.transform.localScale = new Vector3(hpbar.transform.localScale.x, starthp * enemyHp / enemyMaxHp);


    }

    public virtual void Hit(float _damage)
    {
        anim.SetTrigger("Hit");
        float x = transform.position.x - player.GetComponent<Transform>().position.x;
        if (x < 0)
            x = 1;
        else
            x = -1;
        StartCoroutine(EnemyKnockback(x));        
        enemyHp -= _damage;       

    }


    IEnumerator EnemyKnockback(float dir)
    {
        float ctime = 0;
        float knockbacktime = 0.3f;
        float knockbackpower = 20f;
        while (ctime <knockbacktime)
        {
            if (transform.rotation.y == 0)
            {
                transform.Translate(Vector2.left * knockbackpower * Time.deltaTime * dir);
            }
            else
            {
                transform.Translate(Vector2.left * knockbackpower * Time.deltaTime * -1f * dir);
            }

            ctime += Time.deltaTime;
            yield return null;
        }
    }

}
