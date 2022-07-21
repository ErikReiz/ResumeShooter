using UnityEngine;

namespace ResumeShooter.Services
{
	public static class NoiseMaker
	{
		public static void MakeNoise(Vector3 noisePosition, float maxRange, LayerMask layerMask)
		{
			Collider[] overlappingCollieders = Physics.OverlapSphere(noisePosition, maxRange, layerMask);
			foreach (Collider collider in overlappingCollieders)
			{
				IHearing hearingObject = collider.GetComponent<IHearing>();
				if (hearingObject != null)
					hearingObject.OnHeardSomething(noisePosition);
			}
		}
	}
}