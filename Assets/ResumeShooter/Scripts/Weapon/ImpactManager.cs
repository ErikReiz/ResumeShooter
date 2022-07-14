using UnityEngine;
using ResumeShooter.Surface;
using ResumeShooter.Services;

namespace ResumeShooter.Weaponary
{

	public class ImpactManager : MonoBehaviour
	{
		#region SERIALIZE FIELDS 
		[SerializeField] private SerializableDictionary<SurfaceMaterial, GameObject> impactEffects;
		[SerializeField] private GameObject defaultImpactParticle;
		#endregion

		public void SpawnImpactParticle(RaycastHit hitResult)
		{
			GameObject hitObject = hitResult.transform.gameObject;
			SurfaceManager hitObjectSurfaceManager = hitObject.GetComponent<SurfaceManager>();
			GameObject impactParticle = defaultImpactParticle;

			if (hitObjectSurfaceManager)
			{
				if (impactEffects.ContainsKey(hitObjectSurfaceManager.SurfaceType))
				{
					impactParticle = impactEffects[hitObjectSurfaceManager.SurfaceType];
				}

			}

			Vector3 impactPosition = hitResult.point;
			Quaternion impactRotation = Quaternion.LookRotation(hitResult.normal);

			Instantiate(impactParticle, impactPosition, impactRotation, hitObject.transform);
		}
	}
}