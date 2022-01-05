using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public bool active;
    public GameObject gO;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public void Show()
    {
        active = true;
        lastShown = Time.time;
        gO.SetActive(active);
    }

    public void Hide()
    {
        active = false;
        gO.SetActive(active);
    }

    public void UpdateFloatingText()
    {
        if(!active) return;
        
        if(Time.time - lastShown > duration) Hide();

        gO.transform.position += motion * Time.deltaTime;
    }

}
