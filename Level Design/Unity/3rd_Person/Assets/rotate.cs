using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

    private GameObject obj;


	// Use this for initialization
	void Update () {
		
        transform.Rotate(Vector3.up * Time.deltaTime, 0.5f);
    }
	
}
