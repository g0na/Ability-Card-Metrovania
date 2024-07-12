using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public float damage = 30f;
    // Start is called before the first frame update
    void Start()
    {
        
        //this.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {

    }
    

    public void StartLaserAttack()
    {
        StartCoroutine(LaserAttack());
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            _other.GetComponent<CharacterManager>().Hurt(damage, transform.position);
            Debug.Log("character hit");
        }
    }
    IEnumerator LaserAttack()
    {
        // 45
        int yRt = 0;
        float xPos = -0.68f;
        float yPos = 0.67f;
        while (yRt != -180f)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, (float)yRt);
            this.transform.position += new Vector3(xPos, 0, 0);
            if (yRt >= -90)
            {
                this.transform.position += new Vector3(0, -yPos, 0);

            }
            else
            {
                this.transform.position += new Vector3(0, yPos, 0);

            }
            yRt -= 4;
            yield return new WaitForSeconds(0.1f);
        }
        this.gameObject.SetActive(false);

    }
}
