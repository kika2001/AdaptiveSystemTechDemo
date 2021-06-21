using System;
using UnityEngine;

public class PlayerIdentifier : MonoBehaviour
{
    public static PlayerIdentifier instance;

    private void Awake()
    {
        instance = this;
    }
}
