using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Enemy : MonoBehaviour
{

    [SerializeField] float enemyHp = 10;
    [SerializeField] public float enemyDamage = 1f;

    Rigidbody2D rb;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        if (enemyHp <= 0) 
        {
            Destroy(gameObject);
        }


    }



    public void Hit(float _damage)
    {
        enemyHp -= _damage;

        float x = transform.position.x - player.GetComponent<Transform>().position.x;
        if (x < 0)
            x = 1;
        else
            x = -1;

        StartCoroutine(EnemyKnockback(x));
        
        // rb.AddForce(Vector2.right * 10f * Time.deltaTime, ForceMode2D.Force);

    }


    IEnumerator EnemyKnockback(float dir)
    {
        float ctime = 0;
        while (ctime < 0.2f)
        {
            if (transform.rotation.y == 0)
            {
                transform.Translate(Vector2.left * 10 * Time.deltaTime * dir);
            }
            else
            {
                transform.Translate(Vector2.left * 10 * Time.deltaTime * -1f * dir);
            }

            ctime += Time.deltaTime;
            yield return null;
        }
    }

}
