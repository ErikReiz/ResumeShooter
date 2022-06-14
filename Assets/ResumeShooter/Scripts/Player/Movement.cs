using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Movement : MonoBehaviour
{
	#region SERIALIZE FIELDS 
	[Header("Fall info")]
	[Tooltip("Min velocity by Y when player will receive damage from fall")]
	[SerializeField] private float minFallDamageVelocity = 15f;
	[Tooltip("Velocity at which the player will receive damage equal to 100 health units")]
	[SerializeField] private float damageFallVelocity = 40f;

	[Header("Audio Clips")]
	[Tooltip("The audio clip that is played while walking.")]
	[SerializeField] private AudioClip audioClipWalking;

	[Header("General settings")]
	[SerializeField] private float walkSpeed = 5.0f;
	[SerializeField] private float jumpHeight = 5.0f;
	[SerializeField] private float maxWalkableAngle = 45f;
	#endregion

	#region PROPERTIES

	private Vector3 Velocity
	{
		get => rigidBody.velocity;
		set => rigidBody.velocity = value;
	}

	#endregion

	#region FIELDS
	private bool grounded;
	private Rigidbody rigidBody;
	private CapsuleCollider capsule;
	private AudioSource audioSource;
	private float yVelocity = 0;
	private Vector3 surfaceNormal;
	private readonly RaycastHit[] groundHits = new RaycastHit[8];
	#endregion

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

		capsule = GetComponent<CapsuleCollider>();

		audioSource = GetComponent<AudioSource>();
		audioSource.clip = audioClipWalking;
		audioSource.loop = true;		
	}

	private void FixedUpdate()
	{
		MoveCharacter();

		grounded = false;
	}

	private void Update()
	{
		ProcessJump();
		PlayFootstepSounds();
	}

	private void MoveCharacter()
	{
		if(grounded && yVelocity < 0)
		{
			yVelocity = 0;
		}

		Vector2 frameInput = new Vector2();

		frameInput.x = Input.GetAxis("Horizontal");
		frameInput.y = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(frameInput.x, 0.0f, frameInput.y);
		movement *= walkSpeed * Time.deltaTime;
		movement = transform.TransformDirection(movement);

		CalculateGravity();

		Velocity = new Vector3(movement.x, yVelocity, movement.z);
		Velocity = ModifyVelocityWithDirection(Velocity);
	}

	private Vector3 ModifyVelocityWithDirection(Vector3 currentMovement)
	{
		if(grounded)
			return currentMovement - Vector3.Dot(currentMovement, surfaceNormal) * surfaceNormal;
		else
			return currentMovement;
	}

	private void CalculateGravity()
	{
		if (!grounded)
			yVelocity += Physics.gravity.y * rigidBody.mass * Time.deltaTime;
	}

	private void ProcessJump()
	{
		if(Input.GetButtonDown("Jump"))
		{
			if(grounded)
				yVelocity = Mathf.Sqrt(jumpHeight * -Physics.gravity.y);
		}
	}

	private void PlayFootstepSounds()
	{
		if (grounded && rigidBody.velocity.sqrMagnitude > 0.1f)
		{
			if (!audioSource.isPlaying)
				audioSource.Play();
		}
		else if (audioSource.isPlaying)
			audioSource.Pause();
	}


	private void OnCollisionStay()
	{

		Bounds bounds = capsule.bounds;
		Vector3 extents = bounds.extents;
		float radius = extents.x - 0.01f;

		Physics.SphereCastNonAlloc(bounds.center,
			radius,
			Vector3.down,
			groundHits,
			extents.y - radius * 0.5f,
			~0,
			QueryTriggerInteraction.Ignore);

		if (!groundHits.Any(hit => hit.collider != null && hit.collider != capsule))
			return;

		for (var i = 0; i < groundHits.Length; i++)
			groundHits[i] = new RaycastHit();

		grounded = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		GetSurfaceNormal(collision);
		CalculateFallDamage();
	}

	private void GetSurfaceNormal(Collision collision)
	{
		Vector3 normal = collision.contacts[0].normal;
		
		float floorAngle = Vector3.Angle(normal, Vector3.up);
		if(floorAngle > maxWalkableAngle) { return; }

		surfaceNormal = collision.contacts[0].normal;
	}

	private void CalculateFallDamage()
	{
		if (Mathf.Abs(yVelocity) < minFallDamageVelocity) { return; }
		float damageMultiplier = Mathf.Abs(yVelocity) / damageFallVelocity;

		Damager.ApplyDamage(gameObject, damageMultiplier * 100f);
	}
}