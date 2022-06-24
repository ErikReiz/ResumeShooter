using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[Header("Movement")]
	[SerializeField] private float walkSpeed = 5f;
	[SerializeField] private float sprintSpeed = 15f;
	[SerializeField] private float groundedGravity = -0.05f;
	[SerializeField] private float airGravity = -3f;

	[Header("Mouse")]
	[SerializeField] private Transform playerArms;
	[SerializeField] private float mouseSensitivity = 5f;
	[SerializeField] private Vector2 cameraRotationLimits = new Vector2(-90, 90);
	#endregion

	#region FIELDS
	private CharacterController characterController;

	private Vector2 movementInput;
	private Vector3 currentMovement;
	private float currentSpeed;

	private float verticalRotation = 0;
	private float horizontalRotation = 0;
	private bool isSprinting = false;
	#endregion

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		currentSpeed = walkSpeed;
	}

	private void Update()
	{
		MoveCharacter();

		if(isSprinting)
		{
			if (currentSpeed != sprintSpeed)
			{
				currentSpeed = Mathf.Lerp(currentSpeed, sprintSpeed, 0.04f);
			}

		}
		else
		{
			if (currentSpeed != walkSpeed)
				currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, 0.08f);
		}
	}

	private void MoveCharacter()
	{
		currentMovement = transform.right * movementInput.x + transform.forward * movementInput.y;
		CalculateGravity();
		characterController.Move(currentMovement * currentSpeed * Time.deltaTime);
	}

	private void CalculateGravity()
	{
		if (characterController.isGrounded)
			currentMovement.y = groundedGravity;
		else
			currentMovement.y = airGravity;
	}

	public void ReceiveMovementInput(Vector2 movementInput)
	{
		this.movementInput.x = movementInput.x;
		this.movementInput.y = movementInput.y;
	}

	public void ReceiveSprintingInput(bool isSprinting)
	{
		this.isSprinting = isSprinting;
	}

	public void ReceiveMouseInput(Vector2 mouseInput)
	{
		mouseInput *= mouseSensitivity * Time.deltaTime;

		verticalRotation = transform.localEulerAngles.y + mouseInput.x;
		horizontalRotation -= mouseInput.y;

		horizontalRotation = Mathf.Clamp(horizontalRotation, cameraRotationLimits.x, cameraRotationLimits.y);
		transform.localEulerAngles = new Vector3(0, verticalRotation, 0);
		playerArms.localEulerAngles = new Vector3(horizontalRotation, 0, 0);
	}
}