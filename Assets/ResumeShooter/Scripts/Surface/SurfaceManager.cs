using UnityEngine;

public class SurfaceManager : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[SerializeField] private Surface surfaceType;
	#endregion

	public Surface SurfaceType { get { return surfaceType; } }
}
