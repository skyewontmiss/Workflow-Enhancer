using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractItem : MonoBehaviour
{
	public ItemInfo itemInfo;
	public GameObject itemGameObject;


	public abstract void Use();
}