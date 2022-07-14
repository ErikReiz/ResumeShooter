using UnityEngine;
using UnityEngine.Events;

namespace ResumeShooter.Animations
{

	public class PlayerAnimationManager : MonoBehaviour
	{
		#region SERIALIZE FIELDS
		[Tooltip("Sets locomotion smoothness")]
		[SerializeField] private float dampTimeLocomotion = 0.15f;
		#endregion

		#region FIELDS
		public UnityEvent OnWeaponSwitched;
		public UnityEvent OnHolsterStateSwitched;
		public UnityEvent OnEndedReload;
		public UnityEvent OnEjectCasing;
		public UnityEvent OnAmmunitionFill;

		private readonly string sprintingBoolName = "isSprinting";

		private Animator characterAnimator;
		private Vector2 movementInput;

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
			characterAnimator.Play(isEmpty ? "Fire Empty" : "Fire", firingLayer, 0f);
		}

		public void ReloadAnimation(bool isEmpty)
		{
			characterAnimator.Play(isEmpty ? "Reload Empty" : "Reload", reloadingLayer, 0f);
		}

		public void PlayerHolsterAnimation(bool isHolstered)
		{
			characterAnimator.CrossFade(isHolstered ? "Unholster" : "Holster", 0f, holsterLayer, 0);
		}

		public void ChangeAnimatorController(RuntimeAnimatorController newController)
		{
			characterAnimator.runtimeAnimatorController = newController;
		}

		public void ToogleSprint(bool toogle)
		{
			characterAnimator.SetBool(sprintingBoolName, toogle);
		}
		#endregion

		#region CHARACTER EVENTS
		private void OnAnimationSwitchWeapon()
		{
			OnWeaponSwitched?.Invoke();
		}

		private void OnAnimationSwitchHolsterState()
		{
			OnHolsterStateSwitched?.Invoke();
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
}