using UnityEngine;

public class MouseController : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[SerializeField] private float mouseSensitivity = 100f;
	[SerializeField] private Vector2 cameraRotationLimits = new Vector2(-60, 60);
	#endregion

	#region FIELDS
	private float cameraXRotation = 0f;
	private Transform playerBody;
	#endregion

	private void Awake()
	{
		playerBody = GetComponentInParent<Transform>();
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;

		playerBody = transform.parent;
		cameraXRotation = transform.localRotation.x;
	}

	private void Update()
	{
		ProcessMouseInput();
	}

	private void ProcessMouseInput()
	{
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		cameraXRotation -= mouseY;

		cameraXRotation = Mathf.Clamp(cameraXRotation, cameraRotationLimits.x, cameraRotationLimits.y);
		transform.localRotation = Quaternion.Euler(cameraXRotation, 0, 0);

		playerBody.Rotate(Vector3.up * mouseX);
	}
}
