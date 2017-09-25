using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAM : MonoBehaviour {

    private SPVWeaponController myWep;
    // Use this for initialization
    void Start()
    {
        myWep = GetComponent<SPVWeaponController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (myWep.SelectedTarget)
        {
            myWep.BombInstPointP.LookAt(myWep.SelectedTarget.position);
            myWep.FireMissile();
        }

    }

}
