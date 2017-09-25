using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXPool : MonoBehaviour {

    public int PoolSize;
    public GameObject WaterSplash;
    public GameObject[] WaterSplashBag;
    public GameObject Explosion;
    public static FXPool instance;

    public GameObject[] ExplosionBag;

    private int currWSind, currExpFXind;

	// Use this for initialization
	void Start () {

        instance = this;

        WaterSplashBag = new GameObject[PoolSize];
        ExplosionBag = new GameObject[PoolSize];
        for (int i = 0; i < PoolSize; i++)
        {
            WaterSplashBag[i] = Instantiate(WaterSplash, transform);
            WaterSplashBag[i].gameObject.SetActive(false);
            ExplosionBag[i] = Instantiate(Explosion, transform);
            ExplosionBag[i].gameObject.SetActive(false);
            //BulletBag [i].transform.SetParent (transform);
            //      print(i);

        }
	}

    public void MakeSplash(Vector3 AtPos)
    {

        WaterSplashBag[currWSind].transform.position = AtPos;
        WaterSplashBag[currWSind].transform.rotation = Quaternion.identity;
        WaterSplashBag[currWSind].gameObject.SetActive(true);
        currWSind++;
        if (currWSind >= PoolSize)
            currWSind = 0;

    }

    public void Explode(Vector3 AtPos)
    {

        ExplosionBag[currExpFXind].transform.position = AtPos;
        ExplosionBag[currExpFXind].transform.rotation = Quaternion.identity;
        ExplosionBag[currExpFXind].gameObject.SetActive(true);
        currExpFXind++;
        if (currExpFXind >= PoolSize)
            currExpFXind = 0;

    }

}
