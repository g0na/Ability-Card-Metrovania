using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone : MonoBehaviour
{
    public int damage;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            _other.GetComponent<CharacterManager>().Hurt(damage, transform.position);
            Destroy(gameObject);
        }

        if (_other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
