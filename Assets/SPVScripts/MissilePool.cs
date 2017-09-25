using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePool : MonoBehaviour {



    public int MissilePoolSize;
    public GameObject MissilePrefab;
    public GameObject HitFX;
    private Missile[] MissileBag;
    private GameObject[] MissileHitFxBag;

    private int currMissile;

    public static MissilePool instance;

    // Use this for initialization
    void Start()
    {
        instance = this;
        MissileBag = new Missile[MissilePoolSize];
        MissileHitFxBag = new GameObject[MissilePoolSize];
        for (int i = 0; i < MissilePoolSize; i++)
        {

            MissileBag[i] = Instantiate(MissilePrefab, transform).GetComponent<Missile>();
            MissileBag[i].Setup(i);
            MissileHitFxBag[i] = Instantiate(HitFX, transform);
            MissileHitFxBag[i].gameObject.SetActive(false);
            //BulletBag [i].transform.SetParent (transform);
            //      print(i);

        }

    }

    public void HitAtPos(Vector3 startPos, int i)
    {
        if (startPos == null || MissileHitFxBag[i] == null)
            return;

        MissileHitFxBag[i].transform.position = startPos;
        MissileHitFxBag[i].gameObject.SetActive(true);
    }

    public void ShootMissile(Vector3 startPos, Quaternion startRot, Transform target, int launcherLayer, float effectiveness)
    {
        MissileBag[currMissile].StopTrail();
        MissileBag[currMissile].transform.position = startPos;
        MissileBag[currMissile].transform.rotation = startRot;
        MissileBag[currMissile].Shoot(target, launcherLayer, effectiveness);
        currMissile++;
        if (currMissile >= MissilePoolSize)
            currMissile = 0;

    }

	
}
