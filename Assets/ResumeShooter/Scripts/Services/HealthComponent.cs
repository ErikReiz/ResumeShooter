using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ResumeShooter.Services
{

	public class HealthComponent : MonoBehaviour, IDamageable
	{
		#region SERIALIZE FIELDS
		[Header("General")]
		[Tooltip("This components will be turned off after character death. For example you can pass movement component")]
		[SerializeField] private List<Behaviour> componentsToDisable;
		[SerializeField] private bool isPlayer = false;
		[SerializeField] private float maxHealth = 100f;

		[Header("Destroy")]
		[Tooltip("This object will be destroyed after death")]
		[SerializeField] private GameObject objectToDestroy;
		[Tooltip("After this amount of time(in sec) enemy will be destroyed")]
		[SerializeField] private float destroyDelay = 2f;
		#endregion

		#region PROPERTIES
		public bool IsDead { get { return isDead; } }
		public bool IsPlayer { get { return isPlayer; } }

		public float HealthPercents { get { return currentHealth / maxHealth; } }
		public float CurrentHealth
		{
			get { return currentHealth; }
			set
			{
				currentHealth = Mathf.Clamp(value, 0, maxHealth);
			}
		}
		#endregion

		#region FIELDS
		[HideInInspector]
		public UnityEvent OnDamaged;
		[HideInInspector]
		public UnityEvent OnDeath;

		private bool isDead = false;

		private float currentHealth;
		#endregion

		private void Awake()
		{
			currentHealth = maxHealth;
		}

		void IDamageable.ReceiveDamage(float damage)
		{
			if (isDead) { return; }
			damage = Mathf.Clamp(damage, 0, maxHealth);
			currentHealth -= damage;
			OnDamaged?.Invoke();

			if (currentHealth <= 0)
				CharacterDied();
		}

		private void CharacterDied()
		{
			isDead = true;
			OnDeath?.Invoke();
			GameModeBase gameMode = ServiceManager.GetGameMode();

			gameMode.CharacterKilled(isPlayer);

			foreach (var component in componentsToDisable)
			{
				component.enabled = false;
			}

			StartCoroutine(DestroyCoroutine());
		}

		private IEnumerator DestroyCoroutine()
		{
			yield return new WaitForSeconds(destroyDelay);

			Destroy(objectToDestroy ?? gameObject);
		}
	}
}