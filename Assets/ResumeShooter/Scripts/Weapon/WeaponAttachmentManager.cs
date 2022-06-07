using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachmentManager : MonoBehaviour
{
    #region SERIALIZE FIELDS 
    [SerializeField] private Transform muzzle;
	#endregion

	#region PROPERTIES
	public Transform Muzzle { get { return muzzle; } }
	#endregion
}
