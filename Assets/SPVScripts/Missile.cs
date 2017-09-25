using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    public float SideArea;
    public float PropellingForce;
    public float TurnSpeed;
    private float effectiveness;

    private Rigidbody myRB;
    private Rigidbody TargetRB;
    private int myLauncherLayer;
    private TrailRenderer myTrail;
    private int MissileID;
    static Vector3 MissileForce = new Vector3(0, 0, 500);

    private Transform target;

    // Use this for initialization
    public void Setup(int id)
    {
        MissileForce = new Vector3(0, 0, PropellingForce);
        MissileID = id;
        myRB = GetComponent<Rigidbody>();
        myTrail = GetComponentInChildren<TrailRenderer>();
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider coll)
    {
        //Disable bullet
        if (coll.gameObject.layer == myLauncherLayer)
            return;

        if (coll.CompareTag(GlobalVals.PlaneTag))
        {
            coll.GetComponent<SPVHealth>().Damage(20);
            FXPool.instance.Explode(transform.position);
            if(Vector3.Distance(transform.position, Camera.main.transform.position) < 300)
                SPVCamera.Shake();
            gameObject.SetActive(false);
        }
        else if (coll.gameObject.CompareTag(GlobalVals.WaterTag))
        {
            FXPool.instance.MakeSplash(transform.position);
            gameObject.SetActive(false);
        }
        else if (coll.gameObject.CompareTag(GlobalVals.TargetTag) || coll.gameObject.CompareTag(GlobalVals.LockableGTTag))
        {
            coll.gameObject.GetComponent<GroundTarget>().TargetHit();
            gameObject.SetActive(false);
        }
     //   else
      //  {
        //    FXPool.instance.Explode(transform.position);
         //   gameObject.SetActive(false);
        //}
        
    }

    void Update() {
        if(!target)
            return;
        
        float DotVal = Vector3.Dot(transform.forward,  (target.position- transform.position).normalized);
        if (DotVal > 0.5f)
            transform.LookAt(Vector3.Lerp(transform.position + transform.forward, transform.position + (target.position + (TargetRB ? TargetRB.velocity * effectiveness : Vector3.zero) - transform.position), TurnSpeed * Time.deltaTime));
        else
        {
            Dethreat();
            target = null;
        }
            
    }

    void FixedUpdate() {
        Accelerate();
    }

    public void Accelerate()
    {

        float aAcceleration = 200f;
        float aSpeedMultiplier = 8f;

        myRB.AddForce(transform.forward * aAcceleration * aSpeedMultiplier);
        if (myRB.velocity.magnitude > 30 * aSpeedMultiplier)
        {
            myRB.velocity = GetComponent<Rigidbody>().velocity.normalized * 30 * aSpeedMultiplier;
        }


    }

    void OnDisable()
    {
        if (Time.time > 1)
            MissilePool.instance.HitAtPos(transform.position, MissileID);
        Dethreat();
 
    }

    void Dethreat() {
        if (target)
        {
            RWR targetRWR = target.GetComponent<RWR>();
            if (targetRWR)
                targetRWR.RemoveThreat(transform);
        }
    }

    public void StopTrail()
    {
        myTrail.Clear();
    }

    public void Shoot(Transform target, int launcherLayer, float effectiveness)
    {
        if (!target)
            return;
        this.effectiveness = Mathf.Clamp(effectiveness, 0, 0.5f);
        myLauncherLayer = launcherLayer;
        this.target = target;
        myRB.velocity = Vector3.zero;
        TargetRB = target.GetComponent<Rigidbody>();
        gameObject.SetActive(true);
        RWR targetRWR = target.GetComponent<RWR>();
        if (targetRWR)
            targetRWR.SetThreat(transform);
        myRB.AddRelativeForce(MissileForce * 0.2f, ForceMode.Impulse);
    }
		
}

