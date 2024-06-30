using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone : MonoBehaviour
{
    public LayerMask isLayer;

    // Start is called before the first frame update
    void Start()
    {
        isLayer = LayerMask.GetMask("Ground", "Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
