using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    public bool moving = false;
    public float speed = 10;
    private Rigidbody myRGB;
    public Camera cam;
    private Vector3 mousePos;
    public GunController thegun;
    public int CurrentHealth = 10;
    public int MaxHealth = 10;
    public Text health;
    void Start()
    {
        myRGB = GetComponent<Rigidbody>();
       // cam = Camera.main;
    }

    void Update()
    {
        health.text = "Health: " + CurrentHealth + "/" + MaxHealth;
        Move();
        Turn();

        if (Input.GetMouseButtonDown(0))
        {
            thegun.isFiring = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            thegun.isFiring = false;
        }
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime,Space.World);
            moving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
            moving = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
            moving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
            moving = true;
        }
        if (Input.GetKey(KeyCode.D) != true && Input.GetKey(KeyCode.A) != true && Input.GetKey(KeyCode.W) != true && Input.GetKey(KeyCode.S) != true)
        {
            moving = false;
        }
    }

    void Turn()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLenght;
        if (groundPlane.Raycast(ray, out rayLenght))
        {
            Vector3 point = ray.GetPoint(rayLenght);
            Debug.DrawLine(ray.origin,point,Color.blue);
            transform.LookAt(new Vector3(point.x,transform.position.y,point.z));
        }
    }
}
