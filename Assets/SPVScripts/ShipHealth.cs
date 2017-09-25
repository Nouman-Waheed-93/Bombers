using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : SPVHealth {

    public GameObject ShipDebris;


    void Start()
    {
        currHealth = maxHealth;
        if (!ShipDebris)
        {
            ShipDebris = Instantiate(Resources.Load<GameObject>("WeakPointExplosion"), transform, false);
            ShipDebris.transform.localPosition = Vector3.zero;
            ShipDebris.SetActive(false);
        }
    }

	// Use this for initialization
    public override void Damage(int amount) {
        currHealth -= amount;
        if (currHealth < 10)
        {
            ShipDebris.transform.SetParent(null);
            ShipDebris.SetActive(true);
            Destroy(gameObject);
        }
    }
}
