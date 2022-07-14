using UnityEngine;

namespace ResumeShooter.Surface
{

	public class SurfaceManager : MonoBehaviour
	{
		#region SERIALIZE FIELDS
		[SerializeField] private SurfaceMaterial surfaceType;
		#endregion

		public SurfaceMaterial SurfaceType { get { return surfaceType; } }
	}
}