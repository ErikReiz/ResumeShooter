using UnityEngine;

public class ZombieAI : MonoBehaviour, IDamageable
{
	#region SERIALIZE FIELDS 
	[SerializeField] private float maxHealth = 100f;
	[Header("Damage")]
	[SerializeField] private float attackDamage = 10f;
	[Tooltip("Sphere radius around zombie hand, when attacking")]
	[SerializeField] private float attackRadius = 2f;
	[Tooltip("Hand around which the sphere will be created")]
	[SerializeField] private Transform armSocket;
	[SerializeField] private LayerMask playerLayerMask;
	#endregion

	#region PROPERTIES
	public bool IsDead { get { return isDead; } }
	#endregion

	#region FIELDS
	private float currentHealth;
	private bool isDead = false;
	#endregion

	private void Awake()
	{
		currentHealth = maxHealth;
	}

	void IDamageable.ReceiveDamage(float damage)
	{
		if (isDead) { return; }

		currentHealth -= damage;

		if (currentHealth <= 0)
		{
			isDead = true;
			KillEnemy();
		}
	}

	private void KillEnemy()
	{
		GameModeBase gameMode = ServiceManager.GetGameMode();
		gameMode.CharacterKilled(this);

		GameObject rootObject = transform.root.gameObject;
		Destroy(rootObject);
	}

	public void OnApplyDamage()
	{
		Collider[] overlappingObjects = Physics.OverlapSphere(armSocket.position, attackRadius, playerLayerMask);
		if(overlappingObjects.Length == 0) { return; }

		if (overlappingObjects[0])
			Damager.ApplyDamage(overlappingObjects[0].gameObject, attackDamage);
	}
}
