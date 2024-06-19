using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackEnemy : Enemy
{
    private Transform playerPos;
    private float enemyDir;
    public GameObject attackWarning;

    private GameObject aw;

    private int ntime;

    // Start is called before the first frame update
    void Start()
    {
        aw = Instantiate(attackWarning, this.transform);
        aw.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        EnemyFlip();

        ntime += 1;

        // ntime is counter, when it become 
        // every 600 frames, start coroutine.
        if (ntime % 600 == 0 && isAlive)
        {
            StartCoroutine(MeeleeAttack());

        }

    }



    public override void Hit(float _damage)
    {

        base.Hit(_damage);

    }

    IEnumerator MeeleeAttack() // mark apear when coroutine start, and attack after 1.3f seconds.
    {
        aw.SetActive(true);
        //Debug.Log("meeleeeeeeeeattack! ready!");
        yield return new WaitForSeconds(1.3f);
        {
            Attack();
        }
    }

    // this is attack part. 
    public void Attack()
    {
        anim.SetTrigger("Attack");
        //Debug.Log("ataaaaak!");
        aw.SetActive(false);
    }
    
    private void EnemyFlip()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        enemyDir = playerPos.position.x - transform.position.x;
        
        if (enemyDir > 0)
        {
            transform.localScale = new Vector2(-5, transform.localScale.y);
        }
        else if (enemyDir < 0)
        {
            transform.localScale = new Vector2(5, transform.localScale.y);
        }
    }
}
