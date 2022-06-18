using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIPerception : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[Header("General")]
	[SerializeField] private bool hasVisionTrigger = true;
	[SerializeField] private bool hasDamageTrigger = true;

	[Header("AI vision")]
	[SerializeField] private float visionRadius = 20f;
	[SerializeField] private float visionAngle = 60f;
	[Tooltip("Delay(seconds) for each vision check")]
	[SerializeField] private float visionCheckDelay = 0.2f;
	[SerializeField] private LayerMask playerLayerMask;
	[SerializeField] private LayerMask worldLayerMask;

	[Header("On damaged")]
	[Tooltip("Radius in which enemy detects player when receiving damage")]
	[SerializeField] private float damageDetectionRadius = 100f;
	#endregion

	#region FIELDS
	public UnityAction<Vector3> OnPlayerSeen;
	public UnityAction OnLostVision;
	#endregion

	private void Start()
	{
		StartCoroutine(VisionCoroutine());
	}

	private void OnEnable()
	{
		GetComponentInChildren<Enemy>().OnDamaged += ReceiveDamage;
	}

	private void OnDisable()
	{
		GetComponentInChildren<Enemy>().OnDamaged -= ReceiveDamage;
	}

	private IEnumerator VisionCoroutine()
	{
		while(hasVisionTrigger)
		{
			yield return new WaitForSeconds(visionCheckDelay);
			VisionCheck();
		}
	}

	private void VisionCheck()
	{
		Collider[] rangeChecks = Physics.OverlapSphere(transform.position, visionRadius, playerLayerMask);
		Vector3 directionToPlayer = new Vector3();
		if (rangeChecks.Length > 0)
		{
			Transform playerTransform = rangeChecks[0].transform;
			directionToPlayer = Vector3.Normalize(playerTransform.position - transform.position);
			if (Vector3.Angle(transform.forward, directionToPlayer) <= visionAngle / 2)
			{
				bool isHit = ObstacleCheck(playerTransform, directionToPlayer);
				if (!isHit)
				{
					OnPlayerSeen?.Invoke(playerTransform.position);
					return;
				}
			}
		}

		OnLostVision?.Invoke();
	}

	private bool ObstacleCheck(Transform playerTransform, Vector3 directionToPlayer)
	{
		RaycastHit hit;
		bool isHit =  Physics.Raycast(transform.position, directionToPlayer, out hit, Vector3.Distance(transform.position, playerTransform.position), worldLayerMask);

		return isHit;
	}

	void ReceiveDamage()
	{
		if(!hasDamageTrigger) { return; }

		Collider[] rangeChecks = Physics.OverlapSphere(transform.position, damageDetectionRadius, playerLayerMask);
		if (rangeChecks.Length > 0)
		{
			Transform playerTransform = rangeChecks[0].transform;
			OnPlayerSeen?.Invoke(playerTransform.position);
		}
	}
}