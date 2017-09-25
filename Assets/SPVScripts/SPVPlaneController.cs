using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SPVPlaneController : MonoBehaviour
{

	public float PlaneAcceleration, PlaneMinSpeed, PlaneMaxSpeed;
    [HideInInspector]
    public bool SpeedChanging = true;
	private float currSpeed;
	private float targetSpeed;
	private Rigidbody myRB;
	private GameObject PlaneModel;
	private CameraPositioner myCam;
    private float Throttleage;
	void Start ()
	{
		targetSpeed = PlaneMinSpeed;
		myRB = GetComponent<Rigidbody> ();
		PlaneModel = transform.GetComponentInChildren <PlaneGraphic> ().gameObject;
		myCam = GetComponentInChildren<CameraPositioner> ();
		if (myCam)
			myCam.SetCameraPosition (currSpeed);
	}

	void Update ()
	{
        SpeedChanging  = currSpeed != targetSpeed;

        if (SpeedChanging) {
			currSpeed = Mathf.MoveTowards(currSpeed, targetSpeed, Time.deltaTime);
			if (myCam)
				myCam.SetCameraPosition (currSpeed);
		}

	}

	void FixedUpdate ()
	{

		Accelerate (PlaneAcceleration, currSpeed);

	}

	void Accelerate (float aAcceleration, float aSpeedMultiplier)
	{
		myRB.AddForce (transform.up * aAcceleration * aSpeedMultiplier);
		if (myRB.velocity.magnitude > 30 * aSpeedMultiplier) {
			myRB.velocity = GetComponent<Rigidbody> ().velocity.normalized * 30 * aSpeedMultiplier;
		}
	}
    
	public void SetSpeed (float amount)
	{
        Throttleage = Mathf.Clamp(amount, 0, 1);
		targetSpeed = Mathf.Clamp (PlaneMaxSpeed * amount, PlaneMinSpeed, PlaneMaxSpeed);
	}

    public float GetThrottleAge() {
        return Throttleage;
    }

	public float GetSpeed ()
	{
		return 30 * targetSpeed;
	}

	public void Turn (float turnVal)
	{
        
		Vector3 eulerAngles = transform.localEulerAngles;
		eulerAngles.z += turnVal;
		transform.localEulerAngles = eulerAngles;
		eulerAngles = PlaneModel.transform.localEulerAngles;
		eulerAngles.y = Mathf.LerpAngle (eulerAngles.y, 180 + turnVal * 90, 0.1f);
		PlaneModel.transform.localEulerAngles = eulerAngles;
	}

}
