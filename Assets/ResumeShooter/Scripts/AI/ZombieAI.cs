using UnityEngine;
using ResumeShooter.Animations;

namespace ResumeShooter.AI
{

	public class ZombieAI : MonoBehaviour
	{
		#region SERIALIZE FIELDS 
		[Header("Damage")]
		[SerializeField] private Transform armSocket;
		[SerializeField] private LayerMask playerLayerMask;

		[SerializeField] private float attackDamage = 10f;
		[Tooltip("Sphere radius around zombie hand, when attacking")]
		[SerializeField] private float attackRadius = 2f;
		[Tooltip("Hand around which the sphere will be created")]
		#endregion

		#region FIELDS
		private IDamageable playerDamagableComponent;
		private ZombieAnimationEventsReceiver zombieAnimationEventsReceiver;
		#endregion

		private void Awake()
		{
			zombieAnimationEventsReceiver = GetComponentInChildren<ZombieAnimationEventsReceiver>();
		}

		private void OnEnable()
		{
			zombieAnimationEventsReceiver.OnAttackTriggered.AddListener(OnApplyDamage);
		}

		private void OnDisable()
		{
			zombieAnimationEventsReceiver.OnAttackTriggered.RemoveListener(OnApplyDamage);
		}

		private void OnApplyDamage()
		{
			Collider[] overlappingObjects = Physics.OverlapSphere(armSocket.position, attackRadius, playerLayerMask);
			if (overlappingObjects.Length == 0) { return; }

			playerDamagableComponent.ReceiveDamage(attackDamage);
		}
		
		public void Initialize(IDamageable playerDamagableComponent)
		{
			this.playerDamagableComponent = playerDamagableComponent;
		}
	}
}