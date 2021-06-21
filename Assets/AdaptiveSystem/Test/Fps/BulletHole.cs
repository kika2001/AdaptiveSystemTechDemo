using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    public int timeAlive = 1;
    private void OnEnable()
    {
        StartCoroutine(Return());
    }

    private IEnumerator Return()
    {
        yield return new WaitForSeconds(timeAlive);
        transform.gameObject.ReturnToPool();
    }

    /*
    private void Update()
    {
        if (timeStarted+timeAlive<Time.time)
        {
            transform.gameObject.ReturnToPool();
        }
    }
    */
}
