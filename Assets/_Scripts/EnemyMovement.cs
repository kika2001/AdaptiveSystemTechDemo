using System;
using System.Collections;
using System.Collections.Generic;
using AdaptiveS.System;
using AdaptiveSystemDemo.Health;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace AdaptiveSystemDemo.Enemy
{
    public class EnemyMovement : MonoBehaviour
{
    //Ratio Variables
    private float lookReactionTime;
    private float positionUpdateMultiplier;
    private float movementSpeed;
    //Ratio Variables
    
    
    private Rigidbody rb;
    [SerializeField] private TextMeshPro text;

    [Header("TestMovement")] [SerializeField]
    private NavMeshAgent agent;

    private Vector3 playerPos;
    private AdaptiveSystem adaptiveSystem;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Setup();
    }

    private void Setup()
    {
        adaptiveSystem=AdaptiveSystemManager.NewAdaptiveSystem("player");
        AdaptiveSystemManager.CalculateRatio(adaptiveSystem);
        lookReactionTime = AdaptiveSystemManager.CalculateValue(adaptiveSystem,180f, 70f, true);
        positionUpdateMultiplier = AdaptiveSystemManager.CalculateValue(adaptiveSystem,0.5f, 10f, true);
        movementSpeed = AdaptiveSystemManager.CalculateValue(adaptiveSystem,2.0f, 10f, true);
        
        playerPos = PlayerHealthSystem.instance.transform.position;
        
        text.text = $"Look Reaction Time:<color=#3aeb34>{lookReactionTime}</color>\n" +
                    $"Position Reaction Time:<color=#3aeb34>{positionUpdateMultiplier}</color>\n" +
                    $"Movement Speed:<color=#3aeb34>{movementSpeed}</color>" +
                    $"Ratio with multiplied:<color=#3aeb34>{AdaptiveSystemManager.GetCurrentWantedRatio(adaptiveSystem)}</color> ";
        agent.speed = movementSpeed;
        agent.acceleration = 8f+ movementSpeed;
        //agent.angularSpeed = aimReactionTime;
    }
    private void LateUpdate()
    {
        NewMovement();
    }

    private void NewMovement()
    {
        //Debug.LogWarning($"Player pos:{PlayerIdentifier.instance.transform.position}");
        playerPos = Vector3.Lerp(playerPos,PlayerHealthSystem.instance.transform.position,Time.deltaTime*positionUpdateMultiplier);
        agent.SetDestination(playerPos);
    }

   
}

}
