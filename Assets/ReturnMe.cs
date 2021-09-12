using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReturnMe : MonoBehaviour
{
    public float timeBeforeDie;

    private float timeSpawned;
    private void OnEnable()
    {
        timeSpawned = Time.time;
    }

    void Update()
    {
        if (timeSpawned+timeBeforeDie<Time.time)
        {
            gameObject.ReturnToPool();
        }
    }
}
