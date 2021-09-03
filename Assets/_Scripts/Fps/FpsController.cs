using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AdaptiveSystemDemo.Character
{
	public class FpsController : MonoBehaviour {

		public float mouseSensitivityX = 20f;
		public float mouseSensitivityY = 20f;

		public float walkSpeed = 10.0f;
		Vector3 moveAmount;
		Vector3 smoothMoveVelocity;

		[SerializeField]Transform cameraT;
		float verticalLookRotation;

		Rigidbody rigidbodyR;

		float jumpForce = 250.0f;
		bool grounded;
		public LayerMask groundedMask;
		private InputMaster controls;
		[SerializeField] private AudioSource movementAudioSource;
		[SerializeField] private AudioClip leftStepSound, rightStepSound,groundImpactSound;

		private void Awake()
		{
			controls = new InputMaster();
			controls.Player.Movement.Enable();
			controls.Player.Look.Enable();
			controls.Player.Jump.Enable();
			controls.Player.Jump.performed += Jump;
		}

		private void Start()
		{
			SetupController();
		}

		void Update()
		{
			MovementRotate();
		}

		private void FixedUpdate()
		{
			MovePlayer();
		}

		#region Movement And Mouse Look
		private void Jump(InputAction.CallbackContext obj)
		{
			#region Base
			if (grounded) {
				rigidbodyR.AddForce (transform.up * jumpForce);
			}
			#endregion
		}
		private void SetupController()
		{
			rigidbodyR = GetComponent<Rigidbody> ();
			LockMouse ();
		}
		private void MovementRotate()
		{
			// rotation
			transform.Rotate (Vector3.up * controls.Player.Look.ReadValue<Vector2>().x  * mouseSensitivityX * Time.deltaTime);
			verticalLookRotation += controls.Player.Look.ReadValue<Vector2>().y  * mouseSensitivityY * Time.deltaTime;
			verticalLookRotation = Mathf.Clamp (verticalLookRotation, -90, 90);
			cameraT.localEulerAngles = Vector3.left * verticalLookRotation;
			
			
			// movement
			//Vector3 moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;
			Vector3 moveDir = new Vector3 (controls.Player.Movement.ReadValue<Vector2>().x, 0, controls.Player.Movement.ReadValue<Vector2>().y).normalized;
			Vector3 targetMoveAmount = moveDir * walkSpeed;
			moveAmount = Vector3.SmoothDamp (moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

			

			Ray ray = new Ray (transform.position, -transform.up);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask)) {
				if (!grounded)
				{
					movementAudioSource.PlayOneShot(groundImpactSound,0.5f);
				}
				grounded = true;
			}
			else {
				grounded = false;
			}
			
			if (moveDir!= Vector3.zero && grounded)
			{
				PlaySteps();
			}
		}

		private bool isLeft;
		private float lastTimePlayed=0;
		private const float eachStepTravelDistance = 2f;
		private void PlaySteps()
		{
			var timeBetweenSteps = 1/(walkSpeed/eachStepTravelDistance);
			if (Time.time>=lastTimePlayed+timeBetweenSteps)
			{
				isLeft = !isLeft;
				lastTimePlayed = Time.time;
				movementAudioSource.PlayOneShot((isLeft)? leftStepSound: rightStepSound,0.25f);
			}
		}

		private void MovePlayer()
		{
			rigidbodyR.MovePosition (rigidbodyR.position + transform.TransformDirection (moveAmount) * Time.fixedDeltaTime);
		}

		#region Unlock/LockMouse

		void UnlockMouse() {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		void LockMouse() {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		#endregion
		
		#endregion

		
	}
}