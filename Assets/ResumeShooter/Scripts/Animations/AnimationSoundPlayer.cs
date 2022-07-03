using UnityEngine;

public class AnimationSoundPlayer : StateMachineBehaviour
{
	#region SERIALIZE FIELDS
	[SerializeField] private AudioManager.Audio sound;
	#endregion

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		ServiceManager.AudioSpawner.PlaySound(sound);
	}
}