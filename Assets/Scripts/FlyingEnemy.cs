using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    private Transform playerPos;
    private float enemyDir;
    
    // 발사체의 원본 프리팹
    public GameObject bulletPrefab;

    // 공격 주기
    public float attackRate = 2f;

    // 발사할 대상
    private Transform target;

    // 최근 공격 시점에서 지난 시간
    private float timeAfterAttack;

    // Start is called before the first frame update
    void Start()
    {
        // 최근 공격 이후의 누적 시간을 0으로 초기화
        timeAfterAttack = 0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (isAlive)
        {
            EnemyFlip();
        
            timeAfterAttack += Time.deltaTime;

            if (timeAfterAttack >= attackRate)
            {
                // 누적된 시간 리셋
                timeAfterAttack = 0f;

                anim.SetTrigger("Attack");
            
                Instantiate(bulletPrefab, transform.position, transform.rotation);
            }
        }
    }

    private void EnemyFlip()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        enemyDir = playerPos.position.x - transform.position.x;
        
        if (enemyDir > 0)
        {
            transform.localScale = new Vector2(4, transform.localScale.y);
        }
        else if (enemyDir < 0)
        {
            transform.localScale = new Vector2(-4, transform.localScale.y);
        }
    }

    protected override void Die()
    {
        if (enemyHp <= 0)
        {
            isAlive = false;
            anim.SetTrigger("Dead");
            rb.gravityScale = 2.5f;
            Destroy(gameObject, 1);
        }
    }
}
