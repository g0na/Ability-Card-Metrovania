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
    
    // Start is called before the first frame update
    void Start()
    {
        SetHp(30);
        anim = GetComponent<Animator>();
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
            Destroy(gameObject, 2f);
        }
    }
}
