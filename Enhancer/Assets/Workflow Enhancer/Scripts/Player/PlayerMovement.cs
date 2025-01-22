using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ShooterPlayer))]
public class PlayerMovement : MonoBehaviour
{

	[Header("Flags")]
	[SerializeField] bool Move;
	[SerializeField] bool Sprint;
	[SerializeField] bool Jump;
	[SerializeField] bool Slide;

	Rigidbody rb;

	[Header("Actual Movement")]

	[SerializeField] float speed = 5f;
	[SerializeField] float runSpeed = 12.5f;
	[SerializeField] float CrouchSpeed = 20f;

	[SerializeField] float jumpForce = 200;

	[Header("Animation")]
	[SerializeField] Animator animator;


	private bool _isGrounded;

	InputMaster Controls;
ShooterPlayer sp;
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		Controls = new InputMaster();
sp = GetComponent<ShooterPlayer>();

	}

	private void OnEnable()
	{
		Controls.Enable();
	}

	private void OnDisable()
	{
		Controls.Disable();
		
	}

	// Start is called before the first frame update
	void Start()
	{
		Controls.Movement.Jump.started += _ => DoJump();
	}

	void UpdateRunSpeed(float newSpeed) 
	{
		animator.SetFloat("RunCycleSpeed", newSpeed);
	}

	void SetState(int state, string animation)
	{
		if(sp.DoingSomething)
		return;

		if (animator.IsInTransition(0))
			return;

		if (animator.GetCurrentAnimatorStateInfo(0).IsName(animation)) //if its the same animation we WANNA play
			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) //if its simply not done playing
				return;

                animator.SetInteger("State", state);
		animator.CrossFadeInFixedTime(animation, .1f);
	}

	/*States
	 * 
	 * 0 = Idle
	 * 1 = Running
	 * 2 = Sprinting
	 * 3 = Sliding
	 * 4 = Jumping
	 * 
	 */

	// Update is called once per frame
	void Update()
	{
		float forward = Controls.Movement.Forward.ReadValue<float>();
		float right = Controls.Movement.Right.ReadValue<float>();
		Vector3 move = transform.right * right + transform.forward * forward;
		Vector3 SaveBeforePossibleUpdates = move;

        //Flag Check
        bool AmIMoving = Move;
        bool AmISprinting = Sprint;
        bool AmISliding = Slide;
bool AmIAiming = sp.aiming;
        //physics check
         AmIMoving = Controls.Movement.Run.ReadValue<float>() == 0 && move != Vector3.zero;
		 AmISprinting = Controls.Movement.Run.ReadValue<float>() != 0 && move != Vector3.zero;
		 AmISliding = Controls.Movement.Crouch.ReadValue<float>() != 0 && move != Vector3.zero;

		if (AmIMoving)
		{
			SetState(1, "Run");
			UpdateRunSpeed(0.6f);
			Debug.Log("walking with speed: " + speed);
			move = SaveBeforePossibleUpdates * speed;
		} 
		else if (AmISprinting)
		{
			SetState(2, "Run");
			UpdateRunSpeed(1.2f);

			Debug.Log("sprinting with speed: " + (Controls.Movement.Run.ReadValue<float>() == 0 ? speed : runSpeed));
			move = SaveBeforePossibleUpdates * (Controls.Movement.Run.ReadValue<float>() == 0 ? speed : runSpeed);
		}
		else if (AmISliding)
		{
			transform.localScale = new Vector3(1, 0.72618f, 1);
			move = SaveBeforePossibleUpdates * (CrouchSpeed);
			SetState(3, "Slide");
		} 
		else if(_isGrounded) //better be touching ground, or i assime you're jumping
		{
			SetState(0, "Idle");
		}

		if(AmIAiming)
		move *= 0.05f;

			rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
		
	}


	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag("Ground"))
		{
			_isGrounded = true;
		}
	}

	private void OnCollisionExit(Collision other)
	{
		if (other.transform.CompareTag("Ground"))
		{
			_isGrounded = false;
		}
	}

	void DoJump()
	{
		if (!Jump)
			return;

		if (_isGrounded)
		{
			rb.AddForce(Vector3.up * jumpForce);
			SetState(4, "Jump");
			Debug.Log("Jumping...");
		}
	}
}
