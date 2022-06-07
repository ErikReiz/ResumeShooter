using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactManager : MonoBehaviour
{
	#region SERIALIZE FIELDS 
	[SerializeField] private SerializableDictionary<Surface, GameObject> impactEffects;
	[SerializeField] private GameObject defaultImpactParticle;
	#endregion

	public void SpawnImpactParticle(RaycastHit hitResult)
	{
		GameObject hitObject = hitResult.transform.gameObject;
		SurfaceManager hitObjectSurfaceManager = hitObject.GetComponent<SurfaceManager>();
		GameObject impactParticle = defaultImpactParticle;
		Dictionary<Surface, GameObject> particleDictionary = impactEffects.GetDictionary;

		if (hitObjectSurfaceManager)
		{
			if (particleDictionary.ContainsKey(hitObjectSurfaceManager.SurfaceType))
				impactParticle = particleDictionary[hitObjectSurfaceManager.SurfaceType];
		}

		Vector3 impactPosition = hitResult.point;
		Quaternion impactRotation = Quaternion.LookRotation(hitResult.normal);

		Instantiate(impactParticle, impactPosition, impactRotation);
	}
}
