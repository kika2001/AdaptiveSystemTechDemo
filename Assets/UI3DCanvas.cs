using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI3DCanvas : MonoBehaviour
{
    public static bool clickingSomething;
    private Button3D currentButton;
    private InputMaster controls;
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask layerMask;
    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Enable();
        controls.Player.MousePosition.Enable();
        controls.Player.Fire.Enable();
        controls.Player.Fire.performed += OnClick;
    }

    private void OnClick(InputAction.CallbackContext obj)
    {
        if (currentButton!=null)
        {
            currentButton.PlayEvent();
        }
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(controls.Player.MousePosition.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out hit,layerMask))
        {
            currentButton = hit.transform.GetComponent<Button3D>();
        }
    }
}
