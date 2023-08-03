using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwapper : MonoBehaviour
{
	int itemIndex;
	int previousItemIndex = -1;

	[Header("Guns")]
	[SerializeField] AbstractGun[] Guns;

	ItemHandler handler;

	WaitForSeconds shootWait;

	void Awake()
	{
		ForceEquipItem(0);

		handler = new ItemHandler();
		
		handler.Scroll.Scroll.performed += _ => SwapGun(handler.Scroll.Scroll.ReadValue<float>());
		handler.Usage.Shoot.started += _ => UseItem();
		handler.Usage.Shoot.canceled += _ => UnuseItem();
	}

	private void OnEnable()
	{
		handler.Enable();
	}

	private void OnDisable()
	{
		handler.Disable();
	}


	void SwapGun(float ScrollDirection)
	{

		if (ScrollDirection > 0f)
		{
			if (itemIndex >= Guns.Length - 1)
			{
				EquipItem(0);
			}
			else
			{
				EquipItem(itemIndex + 1);
			}
		}
		else if (ScrollDirection < 0f)
		{
			if (itemIndex <= 0)
			{
				EquipItem(Guns.Length - 1);
			}
			else
			{
				EquipItem(itemIndex - 1);
			}
		}

	}

	void EquipItem(int _index)
	{
		if (_index == previousItemIndex)
			return;

		itemIndex = _index;

		Guns[itemIndex].itemGameObject.SetActive(true);
                shootWait = new WaitForSeconds(1 / ((GunCreator)Guns[itemIndex].itemInfo).FireRate);

		if (previousItemIndex != -1)
		{
			Guns[previousItemIndex].itemGameObject.SetActive(false);
		}

		previousItemIndex = itemIndex;
	}

void ForceEquipItem(int _index)
	{
		if (_index == previousItemIndex)
			return;

		itemIndex = _index;
foreach(AbstractGun item in Guns)
{
			item.itemGameObject.SetActive(false);
		}
		Guns[itemIndex].itemGameObject.SetActive(true);
		shootWait = new WaitForSeconds(1 / ((GunCreator)Guns[itemIndex].itemInfo).FireRate);


		previousItemIndex = itemIndex;
	}

	void Update()
	{
			
	}

	Coroutine coroutine;
	void UseItem() 
	{
		coroutine = StartCoroutine(RapidFire((GunCreator)Guns[itemIndex].itemInfo, (Gun)Guns[itemIndex]));
	}
	
	void UnuseItem() 
	{
		if(coroutine != null)
            StopCoroutine(coroutine);
    }
	
	IEnumerator RapidFire(GunCreator gc, Gun gun) 
	{
		if(gc.RapidFire)
		
		{
			while(true) 
			{
				gun.Use();
				yield return shootWait;
			}
		} else 
		{
			gun.Use();
		}
	}
	
	


}
