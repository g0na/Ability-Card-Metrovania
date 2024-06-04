using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackEnemy : Enemy
{

    public GameObject attackWarning;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(attackWarning, this.transform);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        StartCoroutine(MeeleeAttack());
    }



    public override void Hit(float _damage)
    {

        base.Hit(_damage);

    }

    IEnumerator MeeleeAttack()
    {
        Debug.Log("meeleeeeeeeeattack!");
        yield return new WaitForSeconds(10f);
    }

}
