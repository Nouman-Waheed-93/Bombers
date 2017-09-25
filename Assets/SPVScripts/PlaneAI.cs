using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAI : MonoBehaviour
{
	public Transform WayPointP;
    [Range(0, 1)]
    public float PlaneTurn = 1;
    public int MissileFireFrequency = 1;

	private Transform[] AllWayPoints;
	private int currWP;
	private float DTilt = 0;
	private SPVPlaneController m_Plane;
	private SPVWeaponController m_WeaponCntrlr;
	private float StateChangeTime;
	private float SpeedChangeTime;
	private float ReachTime;
	private float MissileFireTime;
	private bool DirectionSet;
	private bool Bombard;
    private int missilesFired;
    public Transform TargetUI;
  
    void Start ()
	{

		m_Plane = GetComponent<SPVPlaneController> ();
		m_WeaponCntrlr = GetComponentInChildren<SPVWeaponController> ();
		AllWayPoints = WayPointP.GetComponentsInChildren<Transform> ();
        CreateUIEnemy();
	}

    void CreateUIEnemy() {
        if (gameObject.layer == 11)
            return;
        TargetUI = Instantiate(Resources.Load<GameObject>("TargetLoc"), GameObject.FindGameObjectWithTag(GlobalVals.HUDTag).transform).transform;
        TargetUI.localPosition = Vector3.zero;
        TargetUI.transform.localScale = new Vector3(2,2,2);

    }

    void UIBoxPosition() {
        if (gameObject.layer == 11)
            return;
        TargetUI.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

    }

    void OnDestroy() {
        if(TargetUI)
            Destroy(TargetUI.gameObject);
    }

    // Update is called once per frame
    void Update ()
	{

		HandleStates ();
        UIBoxPosition();
	}

	void HandleStates ()
	{
		StateChangeTime += Time.deltaTime;
		SpeedChangeTime += Time.deltaTime;
		MissileFireTime += Time.deltaTime;
        ReachTime -= Time.deltaTime;

		if (m_WeaponCntrlr.SelectedTarget && StateChangeTime > 1 && MissileFireTime > 5) {
            missilesFired = 0;
            Engage ();
		} else if (StateChangeTime > 1) {
			Patrol ();
		}

        if (Bombard)
			BombardTarget ();

	}

	void Patrol ()
	{
		if (!DirectionSet) {
            //Direction from plane to waypoint.
            Vector3 DirToWP = (AllWayPoints [currWP].position - transform.position).normalized;
            //Plane turn amount woulbe be the dot product of plane's right 
            //and the direction to waypoint. ie positive turn on right and negative on left.
            DTilt = Mathf.Clamp(-Vector2.Dot (transform.right, DirToWP), -PlaneTurn, PlaneTurn);
            //The plane's forward direction is parallel to it's direction to waypoint.
            if (Mathf.Abs (DTilt) < 0.1f) {
                //The plane is facing opposite to the direction to waypoint.
                if (Vector2.Dot(transform.up, DirToWP) < 0)
                {
                    //Keep turning
                    DTilt = 1;
                }
                else
                {
                    //Stop turning
                    DTilt = 0;
                    DirectionSet = true;
                    m_Plane.SetSpeed(1);
                    //Time the plane will reach the waypoint.
                    ReachTime = Vector2.Distance(AllWayPoints[currWP].position, transform.position) 
                        / m_Plane.GetSpeed();
                }
            }
            else
                DTilt = Mathf.Clamp(Mathf.Sign(DTilt),-PlaneTurn, PlaneTurn);	
		} 

		if (DirectionSet && ReachTime < 0) {
			currWP++;
			if (currWP >= AllWayPoints.Length)
				currWP = 0;
			DirectionSet = false;
			Bombard = AllWayPoints [currWP].CompareTag (GlobalVals.BombardmentPTag);
			m_Plane.SetSpeed (0);
		}

	}

	void BombardTarget ()
	{
        if (m_WeaponCntrlr.DropBomb())
        {
          if (gameObject.layer == 10)
                SPVGM.instance.FriendlyTargetDestroyed();
            else
                SPVGM.instance.RemoveTarget();
        }
	}

    void Engage()
    {

        Vector3 DirToEnmy = (m_WeaponCntrlr.SelectedTarget.position - transform.position);
        DTilt = -Vector2.Dot(transform.right, DirToEnmy.normalized);
        if (Mathf.Abs(DTilt) < 0.2f)
        {

            if (m_WeaponCntrlr.GetMissilesLoaded > 0)
            {
                m_WeaponCntrlr.FireMissile();
                missilesFired++;
                if (missilesFired >= MissileFireFrequency)
                {   
                    MissileFireTime = 0;
                    StateChangeTime = 0;
                }
            }
        }
        else
            DTilt = Mathf.Sign(DTilt);
	}

	void FixedUpdate ()
	{
		m_Plane.Turn (DTilt);
	
	}

}
