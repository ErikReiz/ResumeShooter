using System.Collections;
using UnityEngine;

namespace ResumeShooter.FX
{

	public class ShellComponent : MonoBehaviour
	{
		#region SERIALIZE FIELDS
		[Header("General")]
		[Tooltip("How fast the casing spins over time")]
		[SerializeField] private float speed = 2500.0f;

		[Tooltip("How long after spawning that the casing is destroyed")]
		[SerializeField] private float destroyDelay;

		[Header("Force")]
		[SerializeField] private float minimumXForce;
		[SerializeField] private float maximumXForce;
		[SerializeField] private float minimumYForce;
		[SerializeField] private float maximumYForce;
		[SerializeField] private float minimumZForce;
		[SerializeField] private float maximumZForce;

		[Header("Rotation Force")]
		[SerializeField] private float minimumRotation;
		[SerializeField] private float maximumRotation;

		[Header("Audio")]
		[SerializeField] private AudioClip[] casingSounds;
		#endregion

		#region FIELDS
		private AudioSource audioSource;
		#endregion

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();

			GetComponent<Rigidbody>().AddRelativeTorque(
				Random.Range(minimumRotation, maximumRotation),
				Random.Range(minimumRotation, maximumRotation),
				Random.Range(minimumRotation, maximumRotation)
				* Time.deltaTime);

			GetComponent<Rigidbody>().AddRelativeForce(
				Random.Range(minimumXForce, maximumXForce),
				Random.Range(minimumYForce, maximumYForce),
				Random.Range(minimumZForce, maximumZForce));
		}

		private void Start()
		{
			StartCoroutine(DestroyShell());
			transform.rotation = Random.rotation;

			StartCoroutine(PlaySound());
		}

		private void FixedUpdate()
		{
			transform.Rotate(Vector3.right, speed * Time.deltaTime);
			transform.Rotate(Vector3.down, speed * Time.deltaTime);
		}

		private IEnumerator PlaySound()
		{
			yield return new WaitForSeconds(Random.Range(0.25f, 0.85f));

			audioSource.clip = casingSounds[Random.Range(0, casingSounds.Length)];
			audioSource.Play();
		}

		private IEnumerator DestroyShell()
		{
			yield return new WaitForSeconds(destroyDelay);
			Destroy(gameObject);
		}
	}
}