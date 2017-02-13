using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject player;
    public float cameraDistance = 10;

    [Range(0, 1)]
    public float smoothness = 0f;


    // Update is called once per frame
    void Update()
    {

        Vector3 desiredPos = player.transform.position - new Vector3(0, -cameraDistance, 0);
        Vector3 currentPos = this.transform.position;

        Vector3 newPos = ((desiredPos - currentPos) * (1 - smoothness)) + currentPos;
        this.transform.position = newPos;
    }
}
