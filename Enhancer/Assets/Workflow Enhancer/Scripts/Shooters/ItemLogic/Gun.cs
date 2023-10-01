using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : AbstractGun
{
	[SerializeField] Camera cam;
	[SerializeField] AudioClip sound;
	[SerializeField] AudioSource SFXPlayer;

	public override void Use()
	{
		Shoot();
	}

	void Shoot()
	{
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		ray.origin = cam.transform.position;
		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunCreator)itemInfo).Damage);
			Collider[] colliders = Physics.OverlapSphere(hit.point, 0.3f);

			if (colliders.Length != 0)
			{
				if (colliders[0].tag == "Impact")
					return;

				GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal, Vector3.up) * bulletImpactPrefab.transform.rotation);
				bulletImpactObj.name = "Impact VFX";
				
				if (SFXPlayer != null)
					if (sound != null)
						SFXPlayer.PlayOneShot(sound);

				Destroy(bulletImpactObj, 2f);
				bulletImpactObj.transform.SetParent(colliders[0].transform);
			}
		}
	}

}