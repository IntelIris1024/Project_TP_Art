using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHUDdiplay : MonoBehaviour {


    public bool Detected = false;

    void Start()
    {
        Detected = false;
    } 

     void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Detected = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            Detected = false;
        }
    }
    private void OnGUI()
    {
        if (Detected == true) {
            GUI.Box(new Rect(100, 100, 300, 100), ("Hey, you are awake! Use WASD to move around."));
        }
        }
}
