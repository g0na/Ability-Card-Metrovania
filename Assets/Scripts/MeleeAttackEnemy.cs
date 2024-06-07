using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackEnemy : Enemy
{

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

        ntime += 1;

        // ntime은 프레임마다 ++ 600프레임마다 meleeAttack 코루틴 실행 
        if (ntime % 600 == 0)
        {
            StartCoroutine(MeeleeAttack());

        }

    }



    public override void Hit(float _damage)
    {

        base.Hit(_damage);

    }

    IEnumerator MeeleeAttack() // 작동하고 느낌표, 10초뒤에 Attack()함수 실행 
    {
        aw.SetActive(true);
        Debug.Log("meeleeeeeeeeattack! ready!");
        yield return new WaitForSeconds(0.8f);
        {
            Attack();
        }
    }

    // 애니메이션 변경되면서 공격 하는 부분. 
    public void Attack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("ataaaaak!");
        aw.SetActive(false);
    }
}
