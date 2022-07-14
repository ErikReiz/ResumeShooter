using System.Collections;
using UnityEngine;

namespace ResumeShooter.Services
{

	public class DestroyTimer : MonoBehaviour
	{
		#region SERIALIZE FIELDS
		[SerializeField] private float destroyDelay = 5f;
		#endregion

		private void Start()
		{
			StartCoroutine(DestroyCoroutine());
		}

		private IEnumerator DestroyCoroutine()
		{
			yield return new WaitForSeconds(destroyDelay);

			Destroy(gameObject);
		}

	}
}