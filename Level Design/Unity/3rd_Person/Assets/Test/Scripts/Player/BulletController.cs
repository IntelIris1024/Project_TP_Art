using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
	// Update is called once per frame
    public float speed = 5f;
    public float lifeTime = 1;

	void Update () {
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
	    lifeTime -= Time.deltaTime;

	    if (lifeTime <= 0)
	    {
	        Destroy(gameObject);
	    }
        LineAim();
	}

    void LineAim()
    {
        RaycastHit _hitInfo;
        Ray _ray = new Ray(transform.position, Vector3.forward);
        if (Physics.Raycast(_ray, out _hitInfo,10))
        {
            if (_hitInfo.collider.gameObject.tag == "Enemy")
            {
                Debug.Log("enemy hit");
                Destroy(_hitInfo.collider.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
