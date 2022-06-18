using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[Tooltip("Sets locomotion smoothness")]
	[SerializeField] private float dampTimeLocomotion = 0.15f;
	[SerializeField] public Camera cam; //TODO ������
	#endregion

	#region FIELDS
	private Animator characterAnimator;
	private FPCharacter character;

	private int reloadingLayer;
	private int firingLayer;

	private int hashMovement;
	#endregion

	private void Awake()
	{
		characterAnimator = GetComponentInChildren<Animator>();
		character = GetComponent<FPCharacter>();

		firingLayer = characterAnimator.GetLayerIndex("Firing Layer");
		reloadingLayer = characterAnimator.GetLayerIndex("Reloading Layer");
		hashMovement = Animator.StringToHash("Movement");
	}

	private void OnEnable()
	{
		character.OnWeaponChanged += OnWeaponChanged;
	}

	private void OnDisable()
	{
		character.OnWeaponChanged -= OnWeaponChanged;
	}

	private void OnWeaponChanged(Weapon currentWeapon)
	{
		characterAnimator.runtimeAnimatorController = currentWeapon.AnimatorController;
	}

	public void UpdateMovement(float horizontal, float vertical)
	{
		characterAnimator.SetFloat(hashMovement, Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical)), dampTimeLocomotion, Time.deltaTime);
	}

	public void FireAnimation(bool isEmpty)
	{
		characterAnimator.CrossFade(isEmpty ? "Fire Empty" : "Fire", 0.0f, firingLayer, 0);
	}

	public void ReloadAnimation(bool isEmpty)
	{
		characterAnimator.CrossFade(isEmpty ? "Reload Empty" : "Reload", 0.0f, reloadingLayer, 0);
	}

}
