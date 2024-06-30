using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        //this.transform.rotation = Quaternion.identity;
        StartCoroutine(LaserAttack());
    }

    // Update is called once per frame
    void Update()
    {

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
