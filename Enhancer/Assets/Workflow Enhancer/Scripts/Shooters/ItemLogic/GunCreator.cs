using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FPS/New Gun")]
public class GunCreator : ItemInfo
{

	[Header("Gameplay Mechanics")]
	public float Damage;
	public float FireRate;
	public float ReloadTime;
	public bool RapidFire;
}