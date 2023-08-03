using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

	[Header("Flags")]
	[SerializeField] bool Move;
	[SerializeField] bool Run;
	[SerializeField] bool Jump;
	[SerializeField] bool Crouch;

	Rigidbody rb;

	[Header("Actual Movement")]

	[SerializeField] float speed = 5f;
	[SerializeField] float runSpeed = 12.5f;
	[SerializeField] float CrouchSpeed = 20f;

	[SerializeField] float jumpForce = 200;


	private bool _isGrounded;

	InputMaster Controls;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		Controls = new InputMaster();
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

	// Update is called once per frame
	void Update()
	{
		float forward = Controls.Movement.Forward.ReadValue<float>();
		float right = Controls.Movement.Right.ReadValue<float>();
		Vector3 move = transform.right * right + transform.forward * forward;

		if (Controls.Movement.Crouch.ReadValue<float>() == 0)
		{
			if (!Run)
			{
				Debug.Log("walking with speed: " + speed);
				move *= Controls.Movement.Run.ReadValue<float>() * speed;
			}
			else
			{
				Debug.Log("sprinting with speed: " + (Controls.Movement.Run.ReadValue<float>() == 0 ? speed : runSpeed));
				move *= Controls.Movement.Run.ReadValue<float>() == 0 ? speed : runSpeed;
			}
		}


		if (Crouch)
		{
			transform.localScale = new Vector3(1, Controls.Movement.Crouch.ReadValue<float>() == 0 ? 1f : 0.72618f, 1);
				
				if(Controls.Movement.Crouch.ReadValue<float>() != 0)
			move *= Controls.Movement.Crouch.ReadValue<float>() == 0 ? speed : CrouchSpeed;
		}

		if (!Move)
			move = Vector3.zero;
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
		}
	}
}
