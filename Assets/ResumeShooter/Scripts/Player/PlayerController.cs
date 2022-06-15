using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	#region SERIALIZE FIELDS 
	[Header("Camera")]
	[SerializeField] private Transform playerCamera;
	[SerializeField] private float mouseSensitivity = 2f;
	[SerializeField] private Vector2 cameraRotationLimits = new Vector2(-80, 80);

	[Header("Walking")]
	[SerializeField] private float walkSpeed = 100f;
	[SerializeField] private float jumpPower = 5f;

	[Header("Crouching")]
	[SerializeField] private float crouchHeight = 0.75f;
	[Tooltip("Percents of original speed")]
	[SerializeField] private float speedReduction = 0.5f;

	[Header("Fall info")]
	[Tooltip("Min velocity by Y when player will receive damage from fall")]
	[SerializeField] private float minFallDamageVelocity = 15f;
	[Tooltip("Velocity at which the player will receive damage equal to 100 health units")]
	[SerializeField] private float damageFallVelocity = 40f;
	#endregion

	#region PROPERTIES

	#endregion

	#region FIELDS
	private Rigidbody playerRigidbody;
	private CharacterAnimationManager characterAnimationManager;

	private Vector3 originalScale;

	private bool isGrounded = false;
	private bool isCrouched = false;

	private float maxVelocityChange = 10f;
	private float cameraYaw = 0f;
	private float cameraPitch = 0f;
	private float yVelocity = 0f;
	#endregion

	private void Awake()
	{
		characterAnimationManager = GetComponent<CharacterAnimationManager>();
		playerRigidbody = GetComponent<Rigidbody>();
		originalScale = transform.localScale;
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
    {
		ProcessCameraInput();
		ProcessJump();
		//ProcessCrouch();

		if (!isGrounded)
			yVelocity = playerRigidbody.velocity.y;
	}

	private void FixedUpdate()
	{
		ProcessMovement();
	}

	private void OnCollisionEnter(Collision collision)
	{
		CheckGround();
		CalculateFallDamage();
	}

	private void OnCollisionExit(Collision collision)
	{
		CheckGround();
	}

	private void ProcessCameraInput()
	{
		float xInput = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float yInput = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		cameraYaw = transform.localEulerAngles.y + xInput;
		cameraPitch -= yInput;

		cameraPitch = Mathf.Clamp(cameraPitch, cameraRotationLimits.x, cameraRotationLimits.y);
		transform.localEulerAngles = new Vector3(0, cameraYaw, 0);
		playerCamera.localEulerAngles = new Vector3(cameraPitch, 0, 0);
	}

	private void ProcessJump()
	{
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			if(isCrouched)
			{
				Crouch();
			}
			else
			{
				playerRigidbody.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
				isGrounded = false;
			}

		}
	}

	private void ProcessCrouch()
	{
		if(Input.GetButtonDown("Crouch"))
		{
			Crouch();
		}
	}

	private void Crouch()
	{
		if (isCrouched)
		{
			transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
			walkSpeed /= speedReduction;

			isCrouched = false;
		}
		else
		{
			transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
			walkSpeed *= speedReduction;

			isCrouched = true;
		}
	}

	private void CheckGround()
	{
		Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
		Vector3 direction = transform.TransformDirection(Vector3.down);
		float distance = .75f;

		if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
		{		
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
	}

	private void ProcessMovement()
	{
		Vector3 inputVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		characterAnimationManager.UpdateMovement(inputVelocity.x, inputVelocity.z);

		inputVelocity = Vector3.Normalize(inputVelocity);
		inputVelocity = transform.TransformDirection(inputVelocity) * walkSpeed * Time.deltaTime;

		Vector3 velocity = playerRigidbody.velocity;
		Vector3 velocityChange = inputVelocity - velocity;
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;

		playerRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
	}

	private void CalculateFallDamage()
	{
		if (Mathf.Abs(yVelocity) < minFallDamageVelocity) { return; }
		float damageMultiplier = Mathf.Abs(yVelocity) / damageFallVelocity;

		Damager.ApplyDamage(gameObject, damageMultiplier * 100f);
		yVelocity = 0f;
	}

}
