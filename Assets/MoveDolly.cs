using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MoveDolly : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    private CinemachineTrackedDolly cinemachineTrackedDolly;
    //[Range(0.5f,100f)]
    public float timeForOneLoop;

    [SerializeField] private float moveUnitsPerSecond;

    private float currentPosition;
    // Start is called before the first frame update
    private void OnValidate()
    {
        if (timeForOneLoop<=0)
        {
            timeForOneLoop = 0.01f;
        }
    }

    void Start()
    {
        cinemachineTrackedDolly = cinemachineCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition += (1/timeForOneLoop) * Time.deltaTime;
        cinemachineTrackedDolly.m_PathPosition = currentPosition;
    }
}
