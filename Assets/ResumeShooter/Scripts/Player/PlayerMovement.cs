using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[Header("Movement")]    
    [SerializeField] private float walkSpeed = 200f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float jumpForce;
	#endregion

	#region FIELDS
	private Rigidbody playerRigidbody;

    private Vector3 moveDirection;
    private float horizontalInput;
    private float verticalInput;

    private bool isGrounded = false;
    #endregion

    private void Awake()
    {
        //playerRigidbody = GetComponent<Rigidbody>();
       // playerRigidbody.freezeRotation = true;
    }

	private void Update()
	{
		Debug.Log(playerRigidbody.velocity.magnitude);
		CheckGround();
		SpeedControl();
	}

	private void CheckGround()
	{
		Debug.DrawLine(transform.position, Vector3.down * transform.localScale.y + transform.position, Color.red);
		isGrounded = Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * 1f + 0.2f);

		if (isGrounded)
			playerRigidbody.drag = groundDrag;
		else
			playerRigidbody.drag = 0f;
	}

	public void MovePlayer(float horizontalInput, float verticalInput)
	{
		moveDirection = Vector3.Normalize(transform.forward * verticalInput + transform.right * horizontalInput);
		playerRigidbody.AddForce(moveDirection * walkSpeed * 10f, ForceMode.Force);
	}

	private void SpeedControl()
	{

	}
}
