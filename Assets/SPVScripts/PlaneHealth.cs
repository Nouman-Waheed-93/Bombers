using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneHealth : SPVHealth {

    public ParticleSystem[] DamagedFX;

    public delegate void VMWNP();
    public VMWNP OnDamageTaken;
    GameObject DestroyedExplosion;

    void Start()
    {
        currHealth = maxHealth;
        DestroyedExplosion = Instantiate(Resources.Load<GameObject>("PlayerExplosion"), transform.position, Quaternion.identity);
        DestroyedExplosion.transform.SetParent(transform);
        DestroyedExplosion.SetActive(false);
    }


    public override void Damage(int amount) {

        currHealth -= amount;

        if (OnDamageTaken != null)
            OnDamageTaken();
        if (currHealth < 80 && !DamagedFX[0].isPlaying) {

            DamagedFX[0].Play();
      
        }
        if (currHealth < 60 && !DamagedFX[1].isPlaying)
        {

            DamagedFX[1].Play();

        }
        if (currHealth < 20 && !DamagedFX[2].isPlaying)
        {

            DamagedFX[2].Play();

        }
        if (currHealth < 1) {
            Explode();
            if (gameObject.layer == 10)
            {
                SPVGM.instance.AirTargetDestroyed();
            }
            else {
                SPVGM.instance.FriendlyPlaneDestroyed();
            }
        }

    }

    void OnCollisionEnter(Collision coll) {
        Explode();
    }

    void Explode() {
        DestroyedExplosion.transform.SetParent(null);
        DestroyedExplosion.SetActive(true);
        Destroy(gameObject);
    }

}
