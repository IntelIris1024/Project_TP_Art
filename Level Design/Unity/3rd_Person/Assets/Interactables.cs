using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactables : MonoBehaviour
{

    public Rect windowRect = new Rect(20, 20, 120, 50);
    public string stringToEdit = "";

    public Image icon;
    private bool asshole = false;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Pause();
            asshole = true;
            DestroyObject(icon);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            asshole = false;
        }
    }

    void OnGUI()
    {
        if (asshole == true) {
            windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "New Bio!");
        }
        }
    void DoMyWindow(int windowID)
    {
        stringToEdit = GUILayout.TextArea(stringToEdit, 400);

    }
    public static void Pause()
    {
        Console.Write("Press any key to continue . . . ");
        Console.Read();
    }
}