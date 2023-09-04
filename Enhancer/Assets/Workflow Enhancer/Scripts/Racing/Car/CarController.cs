using UnityEngine.InputSystem;
using UnityEngine;

public class CarController : MonoBehaviour
{
   
        public WheelCollider wheelFrontLeft;
    public WheelCollider wheelFrontRight;
    public WheelCollider wheelBackLeft;
    public WheelCollider wheelBackRight;

    public float steerMax;
    public float motorMax;
    public float brakeMax;

    float steer = 0.0f;
    float motor = 0.0f;
    float brake = 0.0f;
    public float shiftMultiplier = 3f;
    public float controlMultiplier = 0.7f;

    // Use this for initialization
    InputMaster master;
    InputMaster.MovementActions moveActions;

    private void Awake()
    {
        master = new InputMaster();
        moveActions = master.Movement;
    }

    private void OnEnable()
    {
        master.Enable();
    }

    private void OnDisable()
    {
        master.Disable();
    }

    // Update is called once per frame

    void Update()
    {
        wheelFrontLeft.gameObject.transform.Rotate(0, wheelFrontLeft.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelFrontRight.gameObject.transform.Rotate(0, wheelFrontRight.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelBackLeft.gameObject.transform.Rotate(0, wheelBackLeft.rpm / 60 * 360 * Time.deltaTime, 0);
        wheelBackRight.gameObject.transform.Rotate(0, wheelBackRight.rpm / 60 * 360 * Time.deltaTime, 0);
    }

    void FixedUpdate()
    {
        motor = moveActions.Forward.ReadValue<float>();
        steer = moveActions.Right.ReadValue<float>();

        /*determines whether we should reverse or not!
        If we do click the specific key to reverse, then our car will gradually slow down, i think. */

        wheelBackLeft.motorTorque = moveActions.Forward.ReadValue<float>() * motorMax * motor;
        wheelBackRight.motorTorque = moveActions.Forward.ReadValue<float>() * motorMax * motor;
        Debug.Log("motorTorque:" + wheelBackRight.motorTorque);
        if (moveActions.Run.ReadValue<bool>())
        {
            Debug.Log("shift");
            motor = motor * shiftMultiplier;
        }
        else if (moveActions.Crouch.ReadValue<bool>())
        {
            Debug.Log("ctrl");
            motor = motor * controlMultiplier;
        }



        if (moveActions.Jump.ReadValue<bool>())
        {
            Debug.Log("braking");
            //we brake if we press [Space]
            wheelBackLeft.brakeTorque = brakeMax;
            wheelBackRight.brakeTorque = brakeMax;
        }
        else
        {
            Debug.Log("no brake");
            wheelBackLeft.brakeTorque = 0;
            wheelBackRight.brakeTorque = 0;
        }
        Debug.Log("steer");
        wheelFrontLeft.steerAngle = steerMax * steer;
        wheelFrontRight.steerAngle = steerMax * steer;



    }
}