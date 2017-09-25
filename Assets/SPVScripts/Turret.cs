using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public int ErrorFactor = 20;

    private SPVWeaponController myWep;
	// Use this for initialization
	void Start () {
        myWep = GetComponent<SPVWeaponController>();
	}
	
	// Update is called once per frame
	void Update () {

        if (myWep.SelectedTarget) {
            myWep.GunBarrel.LookAt(myWep.SelectedTarget.position + new Vector3(Random.Range(-ErrorFactor, ErrorFactor),
                Random.Range(-ErrorFactor, ErrorFactor), Random.Range(-ErrorFactor, ErrorFactor)));
            myWep.FireCannon();
        }

	}
}
