using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPVBomb : MonoBehaviour
{

    private Rigidbody myRB;
    private GameObject launcher;
   
    private int BombId;

    public void GiveID(int i) {
        BombId = i;
        myRB = GetComponent<Rigidbody>();
   }

    public void Drop(Vector3 velocity, GameObject launcher) {
        gameObject.SetActive(true);
        myRB.velocity = velocity;
        this.launcher = launcher;
    }
    
	// Update is called once per frame
	void Update ()
	{
		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.FromToRotation (transform.forward, GetComponent<Rigidbody> ().velocity), Time.deltaTime);
	}

	void OnCollisionEnter (Collision other)
	{
        if (other.transform.root.gameObject == launcher)
            return;
		if (other.gameObject.CompareTag (GlobalVals.WaterTag)) {
            FXPool.instance.MakeSplash(other.contacts[0].point);
		} else if (other.gameObject.CompareTag (GlobalVals.TargetTag) || other.gameObject.CompareTag(GlobalVals.LockableGTTag)) {
			other.gameObject.GetComponent<GroundTarget> ().TargetHit ();
		} else {
            FXPool.instance.Explode(other.contacts[0].point);
		}
        gameObject.SetActive(false);

	}

}
