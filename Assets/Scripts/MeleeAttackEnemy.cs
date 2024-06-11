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

        // ntime�� �����Ӹ��� ++ 600�����Ӹ��� meleeAttack �ڷ�ƾ ���� 
        if (ntime % 600 == 0)
        {
            StartCoroutine(MeeleeAttack());

        }

    }



    public override void Hit(float _damage)
    {

        base.Hit(_damage);

    }

    IEnumerator MeeleeAttack() // �۵��ϰ� ����ǥ, 10�ʵڿ� Attack()�Լ� ���� 
    {
        aw.SetActive(true);
        Debug.Log("meeleeeeeeeeattack! ready!");
        yield return new WaitForSeconds(1.3f);
        {
            Attack();
        }
    }

    // �ִϸ��̼� ����Ǹ鼭 ���� �ϴ� �κ�. 
    public void Attack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("ataaaaak!");
        aw.SetActive(false);
    }
}
