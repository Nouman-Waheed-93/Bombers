using UnityEngine;
using System.Collections;

public class BulletPool : MonoBehaviour
{

	public int BulletPoolSize;
	public GameObject BulletPrefab;
    public GameObject HitFX;
	private SPVBullet[] BulletBag;
    private GameObject[] BulletHitFxBag;

	private int currBullet;

	public static BulletPool instance;

	// Use this for initialization
	void Start ()
	{
		instance = this;
		BulletBag = new SPVBullet[BulletPoolSize];
        BulletHitFxBag = new GameObject[BulletPoolSize];
		for (int i = 0; i < BulletPoolSize; i++) {

			BulletBag [i] = Instantiate (BulletPrefab, transform).GetComponent<SPVBullet> ();
            BulletBag[i].Setup(i);
            BulletHitFxBag[i] = Instantiate(HitFX, transform);
            BulletHitFxBag[i].gameObject.SetActive(false);
			//BulletBag [i].transform.SetParent (transform);
			//      print(i);

		}

	}

    public void HitAtPos(Vector3 startPos, int i) {
        if (startPos == null || BulletHitFxBag[i] == null)
            return;
        BulletHitFxBag[i].transform.position = startPos;
        BulletHitFxBag[i].gameObject.SetActive(true);
    }

	public void ShootBullet (Vector3 startPos, Quaternion startRot, GameObject launcher)
	{
		BulletBag [currBullet].StopTrail ();
		BulletBag [currBullet].transform.position = startPos;
		BulletBag [currBullet].transform.rotation = startRot;
		BulletBag [currBullet].Shoot (launcher);
		currBullet++;
		if (currBullet >= BulletPoolSize)
			currBullet = 0;
		
	}

}
