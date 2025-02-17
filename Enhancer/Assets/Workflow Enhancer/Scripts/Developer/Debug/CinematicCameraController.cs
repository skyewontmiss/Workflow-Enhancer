using UnityEngine;
using UnityEngine.InputSystem;


public class CinematicCameraController : MonoBehaviour
{

	[Header("Look Settings")]
[SerializeField] float  HorizontalSensitivity  = .02f;
[SerializeField] float  VerticalSensitivity  = .25f;
	private Vector2 lookInput; // Store the look input
	InputMaster.CameraLookActions Controls;
	[SerializeField] GameObject Camera;

	void Awake()
	{
		Controls = new InputMaster().CameraLook;
		
		if(Gamepad.all.Count > 0) 
		{
			HorizontalSensitivity = HorizontalSensitivity * 100;
			VerticalSensitivity = VerticalSensitivity * 30;
		}
	}


	private void OnEnable()
	{
		// Register the look action callback
		Controls.Enable();
	}

	private void OnDisable()
	{
		// Unregister the look action callback
		Controls.Disable();
	}

	private void OnLook(InputAction.CallbackContext context)
	{
		lookInput = context.ReadValue<Vector2>();
	}

    float verticalLookRotation;
    private void Update()
	{
Cursor.visible = false;
Cursor.lockState = CursorLockMode.Locked;

transform.Rotate(Vector3.up * Controls.MouseX.ReadValue<float>() * HorizontalSensitivity);

		verticalLookRotation +=  Controls.MouseY.ReadValue<float>() * VerticalSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

		Camera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	}
}
