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

        if (ntime % 240 == 0)
        {
            StartCoroutine(MeeleeAttack());

        }

    }



    public override void Hit(float _damage)
    {

        base.Hit(_damage);

    }

    IEnumerator MeeleeAttack()
    {
        aw.SetActive(true);
        Debug.Log("meeleeeeeeeeattack! ready!");
        yield return new WaitForSeconds(0.5f);
        {
            Attack();
        }
    }

    public void Attack()
    {
        Debug.Log("ataaaaak!");
        aw.SetActive(false);
    }
}
