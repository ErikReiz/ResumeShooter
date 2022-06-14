using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIPerception : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[Header("General")]
	[SerializeField] private bool hasVision = true;

	[Header("AI vision")]
	[SerializeField] private float visionRadius = 20f;
	[SerializeField] private float visionAngle = 60f;
	[Tooltip("Delay(seconds) for each vision check")]
	[SerializeField] private float visionCheckDelay = 0.2f;
	[SerializeField] private LayerMask playerLayerMask;
	[SerializeField] private LayerMask worldLayerMask;
	#endregion

	#region FIELDS
	public UnityAction<Vector3> OnPlayerSeen;
	public UnityAction OnLostVision;
	#endregion

	private void Start()
	{
		StartCoroutine(VisionCoroutine());
	}

	private IEnumerator VisionCoroutine()
	{
		while(hasVision)
		{
			yield return new WaitForSeconds(visionCheckDelay);
			VisionCheck();
		}
	}

	private void VisionCheck()
	{
		Collider[] rangeChecks = Physics.OverlapSphere(transform.position, visionRadius, playerLayerMask);
		if (rangeChecks.Length > 0)
		{
			Transform playerTransform = rangeChecks[0].transform;
			Vector3 directionToPlayer = Vector3.Normalize(playerTransform.position - transform.position);
			if(Vector3.Angle(transform.position, directionToPlayer) <= visionAngle / 2)
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
		bool isHit =  Physics.Raycast(transform.position, directionToPlayer, Vector3.Distance(transform.position, playerTransform.position), worldLayerMask);
		return isHit;
	}
}
