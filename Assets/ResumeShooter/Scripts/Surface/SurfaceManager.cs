using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceManager : MonoBehaviour
{
	[SerializeField] private Surface surfaceType;

	public Surface SurfaceType { get { return surfaceType; } }
}
