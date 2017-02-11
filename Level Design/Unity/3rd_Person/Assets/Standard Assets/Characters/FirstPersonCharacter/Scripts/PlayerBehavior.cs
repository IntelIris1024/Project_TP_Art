using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    public float speed = 6f;       

    private Vector3 _movement;
    private Vector3 _movementVelocity;         
   
    private Rigidbody _playerRigidbody; 
    public Camera camera;

    void Awake()
    {
       _playerRigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
      
        Move();
        Turning();
    }

    void FixedUpdate()
    {
        _playerRigidbody.velocity = _movementVelocity;
    }

    void Move()
    {
       _movement = new Vector3(Input.GetAxisRaw("Horizontal"),0f,Input.GetAxisRaw("Vertical"));
        _movementVelocity = _movement*speed ;
       
    }
    void Turning()
    {
        Ray camerRay = camera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up,Vector3.zero);
        float rayLenght;
        if (plane.Raycast(camerRay, out rayLenght))
        {
            Vector3 pointToLook = camerRay.GetPoint(rayLenght);
            Debug.DrawLine(camerRay.origin, pointToLook,Color.blue);
            transform.LookAt(new Vector3(pointToLook.x,transform.position.y,pointToLook.z));
        }
    }
}
