using UnityEngine;

[ExecuteAlways]
public class SpawnPoint : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[SerializeField] private float playerHeight = 2f;
	[Tooltip("Capsule radius to check collision contacts")]
	[SerializeField] private float checkRadius = 0.5f;
	[SerializeField] private bool isPlayerSpawn = false;
	#endregion

	#region PROPERTIES
	public bool IsPlayerSpawn { get { return isPlayerSpawn; } }
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

		firstPoint.y -= playerHeight / 2;
		lastPoint.y += playerHeight / 2;

		Gizmos.DrawLine(firstPoint, lastPoint);
	}

	private void SetupCollider()
	{
		Vector3 firstPoint, lastPoint;
		firstPoint = lastPoint = transform.position;

		firstPoint.y = (firstPoint.y - playerHeight / 2) + checkRadius;
		lastPoint.y = (lastPoint.y + playerHeight / 2) - checkRadius;
		if (Physics.OverlapCapsule(firstPoint, lastPoint, checkRadius).Length > 0)
			isSpawnBlocked = true;
	}

	public FPCharacter SpawnCharacter(FPCharacter player)
	{
		if(isSpawnBlocked)
		{
			Debug.Log("blocked");
			throw new System.Exception($"Spawn blocked on {gameObject}");
		}

		return Instantiate(player, transform);
	}

}
