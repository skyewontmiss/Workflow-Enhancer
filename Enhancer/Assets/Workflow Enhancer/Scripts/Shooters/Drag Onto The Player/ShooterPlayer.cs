using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class ShooterPlayer : MonoBehaviour
{
	Animator animator;
	ItemHandler handler;
	PlayerMovement pm;
	CameraLooker cl;
	void Awake()
	{
		animator = GetComponent<Animator>();
		handler = new ItemHandler();
		pm = GetComponent<PlayerMovement>();
		cl = GetComponent<CameraLooker>();
	}

	void Start() {
		handler.Usage.Shoot.started += _ => ShootGun();
	}

	public void ShootGun() {
		DoingSomething = true;
		animator.Play("Shoot", -1, 0f);
	DoingSomething = false;
	}

public bool DoingSomething = false;
	public bool aiming = false;
	public bool HasIdledBefore = true;

	float t = 0f;
	void Update()
{
	t += Time.deltaTime;

    bool wasAimingBefore = aiming;
    aiming = handler.Usage.Aim.ReadValue<float>() > .5f;
DoingSomething = aiming;
    if (aiming)
    {
		cl.camera.fieldOfView = 
		if(!wasAimingBefore) {
        animator.CrossFadeInFixedTime("Aim", 0.1f, -1);
        HasIdledBefore = false;
		
		}
    }
	
    //if (!aiming && !HasIdledBefore)
    //{
    //   animator.CrossFadeInFixedTime("Idle", 0.2f, -1);
    //    HasIdledBefore = true;
    //}
}


	private void OnEnable()
	{
		handler.Enable();
	}

	private void OnDisable()
	{
		handler.Disable();
	}
}