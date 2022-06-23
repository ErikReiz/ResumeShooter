using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[Header("Movement")]
	[SerializeField] private float walkSpeed = 20f;
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
	Vector3 currentMovement;

	float verticalRotation = 0;
	float horizontalRotation = 0;
	#endregion

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		MoveCharacter();
	}

	private void MoveCharacter()
	{
		currentMovement = transform.right * movementInput.x + transform.forward * movementInput.y;
		CalculateGravity();
		characterController.Move(currentMovement * walkSpeed * Time.deltaTime);
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