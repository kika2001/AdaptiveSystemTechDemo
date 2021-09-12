using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AdaptiveSystemDemo.Character
{
	public class FpsController : MonoBehaviour
	{
		public static FpsController instance { get; private set; }

		[Header("Look")]
		public float mouseSensitivityX = 20f;
		public float mouseSensitivityY = 20f;
		[SerializeField]Transform cameraT;
		[SerializeField] private Transform headTransform;
		private Vector3 targetCameraPosition;
		float verticalLookRotation;
		
		[Header("Movement")]
		public float walkSpeed = 10.0f;
		public float jumpForce = 250.0f;
		bool grounded;
		public LayerMask groundedMask;
		Vector3 moveAmount;
		Vector3 smoothMoveVelocity;
		private bool isLeft;
		private float stepLastTimePlayed=0;
		private const float eachStepTravelDistance = 2f;
		private bool isWalking = false;
		private float walkingTime=0;
		
		[Header("Audio")]
		[SerializeField] private AudioSource movementAudioSource;
		[SerializeField] private AudioClip leftStepSound, rightStepSound,groundImpactSound;

		Rigidbody rigidbodyR;
		private InputMaster controls;

		[Header("Head Bobbing")] 
		private float bobFrequency = 5f;
		[SerializeField] private float bobHorizontalAmplitude = 0.1f;
		[SerializeField] private float bobVerticalAmplitude = 0.1f;
		[Range(0, 1)] [SerializeField] private float headBobSmoothing = 0.1f;
		

		private void Awake()
		{
			if (instance!=null)
			{
				Destroy(this.gameObject);
			}

			instance = this;
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
				isWalking = true;
				walkingTime += Time.deltaTime;
			}

			if (isWalking)
			{
				PlaySteps();
				HeadBobbing();
			}
			
			
		}

		
		private void PlaySteps()
		{
			var timeBetweenSteps = 1/(walkSpeed/eachStepTravelDistance);
			if (Time.time>=stepLastTimePlayed+timeBetweenSteps)
			{
				isLeft = !isLeft;
				stepLastTimePlayed = Time.time;
				movementAudioSource.PlayOneShot((isLeft)? leftStepSound: rightStepSound,0.25f);
				
			}
		}

		private void MovePlayer()
		{
			rigidbodyR.MovePosition (rigidbodyR.position + transform.TransformDirection (moveAmount) * Time.fixedDeltaTime);
		}

		private void HeadBobbing()
		{
			targetCameraPosition = headTransform.position + CalculateHeadBobOffset(walkingTime);
			cameraT.position = Vector3.Lerp(cameraT.position, targetCameraPosition, headBobSmoothing);
			if ((cameraT.position-targetCameraPosition).magnitude<=0.001f) 
				cameraT.position = targetCameraPosition;
			
		}

		private Vector3 CalculateHeadBobOffset(float t)
		{
			bobFrequency = walkSpeed;
			float horizontalOffset = 0;
			float verticalOffset = 0;
			Vector3 offset = Vector3.zero;
			if (t>0)
			{
				horizontalOffset = Mathf.Cos(t * bobFrequency) * bobHorizontalAmplitude;
				verticalOffset = Mathf.Sin(t * bobFrequency * 2) * bobVerticalAmplitude;

				offset = cameraT.right * horizontalOffset + cameraT.up * verticalOffset;
			}

			return offset;
		}

		public void PushPlayer(Vector3 pos, float pushAmount)
		{
			var dir = transform.position - pos;
			rigidbodyR.AddForce(dir.normalized * pushAmount,ForceMode.Impulse);
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