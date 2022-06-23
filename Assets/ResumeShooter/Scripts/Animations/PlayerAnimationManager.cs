using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationManager : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[Tooltip("Sets locomotion smoothness")]
	[SerializeField] private float dampTimeLocomotion = 0.15f;
	#endregion

	#region FIELDS
	public UnityAction OnEndedHolster;
	public UnityAction OnEndedReload;
	public UnityAction OnEjectCasing;
	public UnityAction OnAmmunitionFill;

	private Vector2 movementInput;

	private Animator characterAnimator;
	private int reloadingLayer;
	private int firingLayer;
	private int holsterLayer;
	private int hashMovement;
	#endregion

	private void Awake()
	{
		characterAnimator = GetComponent<Animator>();

		firingLayer = characterAnimator.GetLayerIndex("Firing Layer");
		reloadingLayer = characterAnimator.GetLayerIndex("Reloading Layer");
		holsterLayer = characterAnimator.GetLayerIndex("Layer Holster");
		hashMovement = Animator.StringToHash("Movement");
	}

	private void Update()
	{
		UpdateAnimator();
	}

	#region ANIMATIONS
	private void UpdateAnimator()
	{
		characterAnimator.SetFloat(hashMovement, Mathf.Clamp01(Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.y)), dampTimeLocomotion, Time.deltaTime);
	}

	public void ReceiveMovementInput(Vector2 movementInput)
	{
		this.movementInput = movementInput;
	}

	public void FireAnimation(bool isEmpty)
	{
		characterAnimator.CrossFade(isEmpty ? "Fire Empty" : "Fire", 0f, firingLayer, 0);
	}

	public void ReloadAnimation(bool isEmpty)
	{
		characterAnimator.CrossFade(isEmpty ? "Reload Empty" : "Reload", 0f, reloadingLayer, 0);
	}

	public void PlayerHolsterAnimation(bool isHolstered)
	{
		characterAnimator.CrossFade(isHolstered ? "Unholster" : "Holster", 0f, holsterLayer, 0);
	}

	public void ChangeAnimatorController(RuntimeAnimatorController newController)
	{
		characterAnimator.runtimeAnimatorController = newController;
	}
	#endregion

	#region CHARACTER EVENTS
	private void OnAnimationEndedHolster()
	{
		OnEndedHolster?.Invoke();
	}
	#endregion

	#region WEAPON EVENTS
	private void OnAnimationEndedReload()
	{
		OnEndedReload?.Invoke();
	}

	private void OnAnimationEjectCasing()
	{
		OnEjectCasing?.Invoke();
	}

	private void OnAnimationAmmunitionFill()
	{
		OnAmmunitionFill?.Invoke();
	}
	#endregion
}