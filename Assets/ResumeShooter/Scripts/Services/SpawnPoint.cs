using UnityEngine;
using ResumeShooter.Player;
using ResumeShooter.AI;

namespace ResumeShooter.Services
{

	public class SpawnPoint : MonoBehaviour
	{
		#region SERIALIZE FIELDS
		[SerializeField] private float characterHeight = 2f;
		[Tooltip("Capsule radius to check collision contacts")]
		[SerializeField] private float checkRadius = 0.5f;
		#endregion

		#region FIELDS
		private bool isSpawnBlocked = false;
		#endregion

		private void Awake()
		{
			SetupCollider();
		}

		private void OnDrawGizmosSelected()
		{
			Vector3 firstPoint, lastPoint;
			firstPoint = lastPoint = transform.position;

			firstPoint.y -= characterHeight / 2;
			lastPoint.y += characterHeight / 2;

			Gizmos.DrawLine(firstPoint, lastPoint);
		}

		private void SetupCollider()
		{
			Vector3 firstPoint, lastPoint;
			firstPoint = lastPoint = transform.position;

			firstPoint.y = (firstPoint.y - characterHeight / 2) + checkRadius;
			lastPoint.y = (lastPoint.y + characterHeight / 2) - checkRadius;
			if (Physics.OverlapCapsule(firstPoint, lastPoint, checkRadius).Length > 0)
				isSpawnBlocked = true;
		}

		public ZombieAI SpawnCharacter(ZombieAI character)
		{
			if (isSpawnBlocked)
			{
				throw new System.Exception($"Spawn blocked on {gameObject}");
			}

			return Instantiate(character, transform);
		}

	}
}