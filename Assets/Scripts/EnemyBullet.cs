using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Transform playerPos;
    private Vector2 bulletDir;
    
    
    public float distance;
    public LayerMask isLayer;
    public bool dir;
    [SerializeField] public int damage;

    public float bulletSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        bulletDir = playerPos.position - transform.position;
        
        Invoke("DestroyBullet", 0.8f);

        isLayer = LayerMask.GetMask("Ground","Player");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, bulletDir.normalized, distance, isLayer);
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Player")
            {
                Debug.Log("Player Hit!");
                ray.collider.GetComponent<CharacterManager>().Hurt(damage, transform.position);
            }
            DestroyBullet();
        }
        transform.Translate(bulletDir.normalized * bulletSpeed * Time.deltaTime);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
