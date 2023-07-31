using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGun : AbstractItem
{
	public abstract override void Use();

	public GameObject bulletImpactPrefab;
    
}