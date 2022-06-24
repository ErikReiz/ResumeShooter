using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseMaker
{
	public static void MakeNoise(Vector3 noisePosition, float maxRange)
	{
		Collider[] overlappingCollieders = Physics.OverlapSphere(noisePosition, maxRange);
		foreach(Collider collider in overlappingCollieders)
		{
			IHearing hearingObject = collider.GetComponent<IHearing>();
			if(hearingObject != null)
				hearingObject.OnHeardSomething(noisePosition);
		}
	}
}
