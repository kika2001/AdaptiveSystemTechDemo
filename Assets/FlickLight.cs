using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickLight : MonoBehaviour
{
    private Light light;
    [SerializeField] float timeFlick;
    private void Awake()
    {
        light = GetComponent<Light>();
    }

    public void Flick()
    {
        StartCoroutine(FlickEffect());
    }

    IEnumerator FlickEffect()
    {
        
        light.enabled = true;
        yield return new WaitForSeconds(timeFlick);
        light.enabled = false;
        
        
    }
}
