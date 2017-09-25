using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SPVWeaponController : MonoBehaviour
{
	public Transform BombInstPointP;
    public Transform GunBarrel;

	public float RadarViewAngle = 90;
	public float BombLoadSpeed = 3;
    public float MissileLoadSpeed = 3;
    [Range(0, 0.4f)]
    public float MissileEffectiveness = 0.15f;
    public float FireRate = 5f;
	public float BombRippleInterval = 0.2f;
    public float MissileRippleInterval = 0.5f;
    public int MaxBombs = 2;
	public int MaxMissiles = 0;
    

    public enum AttackMode { AttackAir, AttackGround };
    public AttackMode SelectedAttackMode;

    public delegate void AMChangeDel();
    public AMChangeDel OnAttackModeChange;

	private Rigidbody myRB;

    private Transform[] BombInstPoints;
    private int currBIP;
    private AudioSource MissileLaunchSound;
    private int BombsLoaded;

	public int GetBombsLoaded {
		get {
			return BombsLoaded;
		}
	}

	private int MissilesLoaded;

	public int GetMissilesLoaded {
		get {
			return MissilesLoaded;
		}
	}

    public void SwitchAttackMode() {
        if (SelectedAttackMode == AttackMode.AttackAir)
            SelectedAttackMode = AttackMode.AttackGround;
        else
            SelectedAttackMode = AttackMode.AttackAir;
        if (OnAttackModeChange != null)
            OnAttackModeChange();
        currTarget = null;
    }

	private float TimeSinceBombLoad;
	private float TimeSinceBombDrop;

    private float TimeSinceMissileLoad;
    private float TimeSinceMissileLaunch;

	private float LastFireTime = 0;
	private AudioSource GunSound;
	private List<Transform> TargetsInRange = new List<Transform> ();
	private Transform currTarget = null;
    public Transform SelectedTarget {
		get {
			return currTarget;	
		}
	}
    private Renderer TrgtRndr;
    public Renderer SelectedTargetRenderer {
        get {
            return TrgtRndr;
        }
    }

    void Start ()
	{
	
		myRB = GetComponentInParent<Rigidbody> ();
		GunSound = GunBarrel.GetComponent<AudioSource> ();
		GunBarrel.gameObject.SetActive (false);
		BombsLoaded = MaxBombs;
		MissilesLoaded = MaxMissiles;
        BombInstPoints = BombInstPointP.GetComponentsInChildren<Transform>();
        MissileLaunchSound = gameObject.AddComponent<AudioSource>();
        MissileLaunchSound.clip = Resources.Load<AudioClip>("MissileLaunchEngine");
	}
    
	public bool DropBomb ()
	{
		if (BombsLoaded < 1 || TimeSinceBombDrop < BombRippleInterval)
			return false;
        
        if (BombsLoaded == MaxBombs)
            TimeSinceBombLoad = 0;
		
        BombPool.instance.DropBomb(BombInstPoints[currBIP].position, BombInstPoints[currBIP].rotation, myRB.velocity, gameObject);
        currBIP++;
        if (currBIP == BombInstPoints.Length)
            currBIP = 0;
        BombsLoaded--;
        TimeSinceBombDrop = 0;
        return true;

	}

    public void FireMissile() {
        if (!currTarget || MissilesLoaded < 1 || TimeSinceMissileLaunch < MissileRippleInterval)
            return;

        if (MissilesLoaded == MaxMissiles)
            TimeSinceMissileLoad = 0;

        MissilePool.instance.ShootMissile(BombInstPoints[currBIP].position, BombInstPoints[currBIP].rotation, currTarget, gameObject.layer, MissileEffectiveness);
        MissilesLoaded--;
        TimeSinceMissileLaunch = 0;
        
    }

	public void FireCannon ()
	{
		if (Time.time - LastFireTime > 1 / FireRate) { 
			BulletPool.instance.ShootBullet (GunBarrel.position, GunBarrel.rotation, transform.root.gameObject);
			LastFireTime = Time.time;
			GunBarrel.gameObject.SetActive (true);
			Invoke ("HideMuzzle", 0.2f);	
		}

	}

	void Update ()
	{

		HandleTargetAcquisition ();
		HandleBombLoading ();
        HandleMissileLoading();

	}

    void HandleMissileLoading()
    {
        TimeSinceMissileLoad += Time.deltaTime;
        TimeSinceMissileLaunch += Time.deltaTime;
       
        if (MissilesLoaded == MaxMissiles)
            return;

        if (TimeSinceMissileLoad > MissileLoadSpeed)
        {
            MissilesLoaded++;
            TimeSinceMissileLoad = 0;
        }

    }

	void HandleBombLoading ()
	{
		TimeSinceBombLoad += Time.deltaTime;
		TimeSinceBombDrop += Time.deltaTime;
  
		if (BombsLoaded == MaxBombs)
			return;
		
		if (TimeSinceBombLoad > BombLoadSpeed) {
			BombsLoaded++;
			TimeSinceBombLoad = 0;
		}

	}

	void HandleTargetAcquisition ()
	{
		if (!currTarget)
			SelectTarget ();
		else
			TargetInfrontOfRadar ();
	}

	void HideMuzzle ()
	{
		GunBarrel.gameObject.SetActive (false);
	}

	void OnTriggerEnter (Collider other)
	{
		if (//!other.isTrigger && 
           // other.gameObject.CompareTag (SelectedAttackMode == AttackMode.AttackGround? GrndTargetTag : PlaneTag) &&
           (other.gameObject.CompareTag(GlobalVals.LockableGTTag) || other.gameObject.CompareTag(GlobalVals.PlaneTag)) && 
           other.gameObject.layer != gameObject.layer) {
			if (!TargetsInRange.Contains (other.transform)) {
				TargetsInRange.Add (other.transform);
                SelectTarget();
                //            if (!currTarget && 
    //                other.CompareTag(SelectedAttackMode == AttackMode.AttackGround ? GlobalVals.LockableGTTag : GlobalVals.PlaneTag))
    //            {
				//	if (Vector3.Angle (transform.up, (other.transform.position - transform.position).normalized) < RadarViewAngle) {
				//		currTarget = other.transform;
				//	}
				//}
			}
		}
	}

	void SelectTarget ()
	{
		for (int i = 0; i < TargetsInRange.Count; i++) {
			if (!TargetsInRange [i]) {
				TargetsInRange.RemoveAt (i);
				continue;
			}
            if (TargetsInRange[i].CompareTag(SelectedAttackMode == AttackMode.AttackGround ? GlobalVals.PlaneTag : GlobalVals.LockableGTTag))
                continue;

			float currTempAngle = Vector3.Angle (TargetsInRange [i].position - transform.position, transform.up);
			if (currTempAngle < RadarViewAngle) {
                //if (currTarget) {
                //	if (currTempAngle < Vector3.Angle (currTarget.position - transform.position, transform.up))
                //	currTarget = TargetsInRange [i];
                //	} else {
                if (!currTarget)
                {
                    currTarget = TargetsInRange[i];
                    TrgtRndr = currTarget.GetComponentInChildren<Renderer>();
                    if (!TrgtRndr)
                        TrgtRndr = currTarget.GetComponentInParent<Renderer>();
                }
				//}
			}
		} 

	}

	void TargetInfrontOfRadar ()
	{
		if (Vector3.Angle (currTarget.position - transform.position, transform.up) > RadarViewAngle)
			currTarget = null;
	}

	void OnTriggerExit (Collider other)
	{
        if (//!other.isTrigger &&
            //other.gameObject.CompareTag (SelectedAttackMode == AttackMode.AttackGround? GrndTargetTag : PlaneTag) &&
            (other.gameObject.CompareTag(GlobalVals.LockableGTTag) || other.gameObject.CompareTag(GlobalVals.PlaneTag)) &&
            other.gameObject.layer != gameObject.layer) {
            if (TargetsInRange.Contains (other.transform)) {
                TargetsInRange.Remove (other.transform);
				if (currTarget == other.transform) {
                    currTarget = null;
					SelectTarget ();
				}
			}
		}
	}

    int count;

    public Vector3 CCIP()
    {
        
        Vector3 position = transform.position;
        Vector3 planeVelocity = myRB.velocity;
        Vector3 velocity = planeVelocity;
        count = 1;
        Vector3 lastPos = transform.position;
        int layerMask = (1 << 16) | (1 << 17) | (1 << 4) | (1 << 20);
        bool hitFound = false;
        while (!hitFound)
        {
            velocity += Physics.gravity * 0.01f;
            velocity = velocity * 1 / (1 + 0.01f * 0.8f);
            position += velocity * 0.01f;

            if (position.z > 10 * count)
            {
                count++;
                RaycastHit hit = new RaycastHit();
                if (Physics.SphereCast(lastPos, 10.4f, position - lastPos, out hit, (position - lastPos).magnitude, layerMask))
                {
                    hitFound = true;

                    position = lastPos + ((position - lastPos).normalized * ((hit.point.z - lastPos.z) / (position.z - lastPos.z)));
                    position.z += 5;
                    position.z -= 0.5f;
                }

                lastPos = position;
            }
            else if (position.z >= 121.5f)
            {
                hitFound = true;
                position.z = 121.5f;
            }
        }

        return position;

    }

}
