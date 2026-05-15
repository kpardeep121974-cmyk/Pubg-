using UnityEngine;

public class BattleRoyaleVehicleController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Wheel Transforms (Visuals)")]
    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    [Header("Vehicle Settings")]
    public float maxMotorTorque = 1500f; // गाड़ी की ताकत (Speed)
    public float maxSteeringAngle = 30f; // गाड़ी कितना मुड़ेगी
    public float brakeForce = 3000f;    // ब्रेक की ताकत

    private float currentMotorTorque;
    private float currentSteerAngle;
    private float currentBrakeForce;
    private bool isDriverInside = false;
    private Transform currentDriver;

    void Update()
    {
        if (!isDriverInside) return;

        // 1. Get Input from Player (WASD / Arrow Keys)
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");
        bool isBraking = Input.GetKey(KeyCode.Space);

        // 2. Calculate Motor & Steer values
        currentMotorTorque = moveInput * maxMotorTorque;
        currentSteerAngle = steerInput * maxSteeringAngle;
        currentBrakeForce = isBraking ? brakeForce : 0f;

        ApplyVehiclePhysics();
        UpdateWheelVisuals();

        // 3. Exit Vehicle (Press 'F' to get out)
        if (Input.GetKeyDown(KeyCode.F))
        {
            ExitVehicle();
        }
    }

    private void ApplyVehiclePhysics()
    {
        // Front wheels handle steering
        frontLeftWheel.steerAngle = currentSteerAngle;
        frontRightWheel.steerAngle = currentSteerAngle;

        // Rear wheels handle power (Rear Wheel Drive)
        rearLeftWheel.motorTorque = currentMotorTorque;
        rearRightWheel.motorTorque = currentMotorTorque;

        // Apply brake force to all wheels
        frontLeftWheel.brakeTorque = currentBrakeForce;
        frontRightWheel.brakeTorque = currentBrakeForce;
        rearLeftWheel.brakeTorque = currentBrakeForce;
        rearRightWheel.brakeTorque = currentBrakeForce;
    }

    private void UpdateWheelVisuals()
    {
        // Sync 3D Wheel meshes with physics colliders
        UpdateSingleWheel(frontLeftWheel, frontLeftTransform);
        UpdateSingleWheel(frontRightWheel, frontRightTransform);
        UpdateSingleWheel(rearLeftWheel, rearLeftTransform);
        UpdateSingleWheel(rearRightWheel, rearRightTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        if (wheelTransform == null) return;
        
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    // Call this from player's interaction script to enter the car
    public void EnterVehicle(Transform player)
    {
        isDriverInside = true;
        currentDriver = player;

        // Disable player's character controller and movement script so they don't walk away
        player.gameObject.SetActive(false); 
        player.SetParent(this.transform); // Make player a child of the car
        Debug.Log("Player entered the vehicle as Driver.");
    }

    public void ExitVehicle()
    {
        if (currentDriver == null) return;

        isDriverInside = false;
        currentDriver.SetParent(null); // Unparent player
        
        // Spawn player slightly out to the left side of the vehicle
        currentDriver.position = transform.position + (-transform.right * 2f) + new Vector3(0, 0.5f, 0);
        currentDriver.gameObject.SetActive(true);

        // Reset vehicle physics so it stops moving on its own
        rearLeftWheel.motorTorque = 0;
        rearRightWheel.motorTorque = 0;
        
        currentDriver = null;
        Debug.Log("Player exited the vehicle.");
    }
}
