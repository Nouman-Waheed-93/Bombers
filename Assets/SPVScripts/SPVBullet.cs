using UnityEngine;
using System.Collections;

public class SPVBullet : MonoBehaviour
{

	private Rigidbody myRB;
	private TrailRenderer myTrail;
	private float shotTime = -1;
	private int BulletID;
    private GameObject myLauncher;
    static Vector3 BulletForce = new Vector3 (0, 0, 800);

	// Use this for initialization
	public void Setup (int id)
	{
		BulletID = id;
		myRB = GetComponent<Rigidbody> ();
		myTrail = GetComponentInChildren<TrailRenderer> ();
		gameObject.SetActive (false);
	}

	void OnTriggerEnter (Collider coll)
	{
        if (coll.transform.root.gameObject == myLauncher)
            return;
        //Disable bullet
        if (coll.CompareTag(GlobalVals.PlaneTag))
        {
            coll.GetComponent<SPVHealth>().Damage(5);
            gameObject.SetActive(false);
        }
        else if (coll.gameObject.CompareTag(GlobalVals.TargetTag) || coll.gameObject.CompareTag(GlobalVals.LockableGTTag))
        {
            coll.gameObject.GetComponent<GroundTarget>().TargetHit();
            gameObject.SetActive(false);
        }
        
    }

	void OnDisable ()
	{
		if (Time.time > 1)
			BulletPool.instance.HitAtPos (transform.position, BulletID);
	}

	public void StopTrail ()
	{
		myTrail.Clear ();
	}

	public void Shoot (GameObject launcher)
	{
        myLauncher = launcher;
		myRB.velocity = Vector3.zero;
		gameObject.SetActive (true);
		shotTime = Time.time;
		myRB.AddRelativeForce (BulletForce, ForceMode.Impulse);

	}
		
}
