using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed = 0.1f;

    [SerializeField] private Vector3 offset;

    public bool isBossStage = false;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "BossStage")
        {
            isBossStage = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBossStage)
        {
            if (CharacterManager.Instance)
            {
                transform.position = Vector3.Lerp(transform.position, CharacterManager.Instance.transform.position + offset, followSpeed);

            }
        }
        else
        {
            transform.position = new Vector3(0,-7.5f,-10);
            this.GetComponent<Camera>().orthographicSize = 24;

        }

    }
}
