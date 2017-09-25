using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundVehicle : MonoBehaviour {

    public Transform[] WayPoints;
    private int CurrWp;
    public float TurnSpeed = 1, Speed = 10, StayTime = 1;
    public bool Patrol;
    public float stoppingDistance;
    private float TimeStayed;
    private bool DirSet;
    private float ReachTime;
    private bool Stop;
	// Update is called once per frame
	void Update () {

        if (Stop)
            return;

        if (WayPoints.Length > 0 && TimeStayed > StayTime)
        {
            ReachTime -= Time.deltaTime;
            Move();
            if (!DirSet) {
                Vector2 dirToWP = (WayPoints[CurrWp].position - transform.position).normalized;
                float TurnDir = Vector2.Dot(-transform.right, dirToWP);
                if (Mathf.Abs(TurnDir) < 0.1f)
                {
                    if (Vector2.Dot(transform.up,dirToWP) < 0)
                    {
                        TurnDir = 1;
                    }
                    else
                    {
                        TurnDir = 0;
                        DirSet = true;
                        ReachTime = (Vector2.Distance(WayPoints[CurrWp].position, transform.position) - stoppingDistance) / Speed;
                    }
                }
                Turn(TurnDir);
            }


            if (DirSet && ReachTime < 0)
            {
                TimeStayed = 0;
                DirSet = false;
                CurrWp++;
                if (CurrWp >= WayPoints.Length)
                {
                    if (Patrol)
                        CurrWp = 0;
                    else
                        Stop = true;
                }
            }
        }
        else
            TimeStayed += Time.deltaTime;

    }

    void Move() {
        transform.position += transform.up * Speed * Time.deltaTime;
    }

    void Turn(float dir) {
        transform.Rotate(0, 0, dir * TurnSpeed * Time.timeScale);
    }
    
}
