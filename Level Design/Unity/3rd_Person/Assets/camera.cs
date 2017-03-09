using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour {

    private Camera cam;
    private Vector3 defPos;

    private void Start()
    {
        defPos = transform.position;
    }
    // Update is called once per frame
    void Update () {

        transform.Translate(Vector3.right * Time.deltaTime * 4);
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Camera")
        {
            Reset();
        }
    }

    public void Reset()
    {
       
    }
}
