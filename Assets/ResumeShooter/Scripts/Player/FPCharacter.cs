using UnityEngine;
using UnityEngine.Events;


public class FPCharacter : MonoBehaviour, IDamage
{
	#region SERIALIZE FIELDS
	[SerializeField] private float maxHealth = 100f;
	#endregion

	#region PROPERTIES
	private float GetHeatlhPercents { get { return currentHealth / maxHealth; } }
	#endregion

	#region FIELDS
	public UnityAction<float> OnPlayerDamaged;

	private Weapon currentWeapon;
	private float currentHealth;
	private bool isDead = false;
	#endregion

	private void Awake()
	{
		currentWeapon = GetComponentInChildren<Weapon>();
		currentHealth = maxHealth;
	}

	private void Update()
	{
		ProcessFireInput();
		ProcessReloadInput();
	}

	#region INPUT
	private void ProcessFireInput()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (!currentWeapon) { return; }

			currentWeapon.StartFire();
		}
		else if (Input.GetButtonUp("Fire1"))
		{
			if (!currentWeapon) { return; }

			currentWeapon.StopFire();
		}

		currentWeapon.ProcessAutomaticFire();
	}

	private void ProcessReloadInput()
	{
		if (Input.GetButtonDown("Reload"))
		{
			if (!currentWeapon) { return; }

			currentWeapon.TryReload();
		}
	}
	#endregion

	void IDamage.ReceiveDamage(float damage)
	{
		if (isDead) { return; }

		damage = Mathf.Clamp(damage, 0, maxHealth);
		currentHealth -= damage;

		OnPlayerDamaged.Invoke(GetHeatlhPercents);

		if (currentHealth == 0)
		{
			isDead = true;
			KillPlayer();
		}
	}

	private void KillPlayer()
	{
		Debug.Log("Player Killed");
	}
}
