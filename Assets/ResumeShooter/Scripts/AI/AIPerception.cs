using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AIPerception : MonoBehaviour, IHearing
{
	#region SERIALIZE FIELDS
	[Header("General")]
	[SerializeField] private bool hasVisionTrigger = true;
	[SerializeField] private bool hasHearingTrigger = true;

	[Header("AI vision")]
	[SerializeField] private LayerMask playerLayerMask;
	[SerializeField] private LayerMask worldLayerMask;

	[SerializeField] private float visionRadius = 20f;
	[SerializeField] private float visionAngle = 60f;
	[Tooltip("Delay(seconds) for each vision check")]
	[SerializeField] private float visionCheckDelay = 0.2f;

	[Header("AI hearing")]
	[Tooltip("Method will create trigger with this radius")]
	[SerializeField] private float hearingRadius = 10f;

	[Header("Wave Game Mode")]
	[Tooltip("AI will detect player immediately")]
	[SerializeField] private bool useAlternativeLogic = true;
	#endregion

	#region FIELDS
	public UnityAction<Vector3> OnPlayerSeen;
	public UnityAction OnLostVision;
	public UnityAction<Vector3> OnHearedSomething;

	private SphereCollider hearingCollider;
	private FPCharacter player;
	#endregion

	private void Start()
	{
		EnableAlternativBehaviour();
	}

	private void EnableAlternativBehaviour()
	{
		if (!useAlternativeLogic) { return; }
		player = ServiceManager.GetPlayer();
	}

	private void OnEnable()
	{
		if (useAlternativeLogic)
			StartCoroutine(AlternativeLogicCoroutine());
		else
		{
			if (hasVisionTrigger)
				StartCoroutine(VisionCoroutine());
			if (hasHearingTrigger)
				CreateHearingCollider();
		}

	}

	private void OnDisable()
	{
		StopAllCoroutines();
		DestroyHearingCollider();
	}

	private IEnumerator AlternativeLogicCoroutine()
	{
		while (useAlternativeLogic)
		{
			yield return new WaitForSeconds(visionCheckDelay);
			OnPlayerSeen?.Invoke(player.transform.position);
		}
	}

	private IEnumerator VisionCoroutine()
	{
		while (hasVisionTrigger)
		{
			yield return new WaitForSeconds(visionCheckDelay);
			VisionCheck();
		}
	}

	private void VisionCheck()
	{
		Collider[] rangeChecks = Physics.OverlapSphere(transform.position, visionRadius, playerLayerMask);
		Vector3 directionToPlayer = new();
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
		bool isHit = Physics.Raycast(transform.position, directionToPlayer, out hit, Vector3.Distance(transform.position, playerTransform.position), worldLayerMask);

		return isHit;
	}

	private void CreateHearingCollider()
	{
		if (hasHearingTrigger)
		{
			hearingCollider = gameObject.AddComponent<SphereCollider>();
			hearingCollider.radius = hearingRadius;
			hearingCollider.isTrigger = true;
		}
	}

	private void DestroyHearingCollider()
	{
		if (!hearingCollider) { return; }

		Destroy(hearingCollider);
	}

	void IHearing.OnHeardSomething(Vector3 noisePosition)
	{
		if (hasHearingTrigger)
			OnHearedSomething?.Invoke(noisePosition);
	}
}