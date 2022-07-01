using UnityEngine;

[ExecuteAlways]
public class PlayerStart : MonoBehaviour
{
	#region
	private CapsuleCollider collisionCollider;
	private GameModeBase gameMode;
	private uint numOfCollision = 0;
	#endregion

	private void Awake()
	{
		gameMode = ServiceManager.GetGameMode();

		collisionCollider = GetComponent<CapsuleCollider>();
		if (!collisionCollider)
			collisionCollider = gameObject.AddComponent<CapsuleCollider>();

		SetupCollider();
	}

	private void OnTriggerEnter(Collider other)
	{
		numOfCollision++;
	}

	private void OnTriggerExit(Collider other)
	{
		numOfCollision--;
	}

	private void SetupCollider()
	{
		collisionCollider.radius = 0.5f;
		collisionCollider.height = 2f;
		collisionCollider.isTrigger = true;
	}

	public FPCharacter SpawnPlayer()
	{// Разобраться с numOfCollisions
		return Instantiate(ServiceManager.GetPlayer(), transform);
	}

}
