using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPool : MonoBehaviour {

    public int PoolSize;
    public GameObject BombPrefab;
   // public GameObject BombDropPE;

  //  public GameObject[] BombFX;
    public SPVBomb [] BombBag;

    private int currBomb;

    public static BombPool instance;

    // Use this for initialization
    void Start()
    {
        instance = this;
        BombBag = new SPVBomb[PoolSize];
   //     BombFX = new GameObject[PoolSize];

        for (int i = 0; i < PoolSize; i++)
        {

            BombBag[i] = Instantiate(BombPrefab, transform).GetComponent<SPVBomb>();
            BombBag[i].GiveID(i);
            BombBag[i].gameObject.SetActive(false);
       //     BombFX[i] = Instantiate(BombDropPE, transform);
     //       BombFX[i].gameObject.SetActive(false);
    
       
            //BulletBag [i].transform.SetParent (transform);
            //      print(i);

        }

    }

    public void DropBomb(Vector3 startPos, Quaternion startRot, Vector3 velocity, GameObject launcher)
    {
     //   BombFX[currBomb].SetActive(false);
      //  BombFX[currBomb].transform.position = startPos;
      //  BombFX[currBomb].transform.rotation = startRot;
      //  BombFX[currBomb].SetActive(true);
        BombBag[currBomb].transform.position = startPos;
        BombBag[currBomb].transform.rotation = startRot;
        BombBag[currBomb].gameObject.SetActive(true);
        BombBag[currBomb].Drop(velocity, launcher);
        currBomb++;
        if (currBomb >= PoolSize)
            currBomb = 0;

    }

}
