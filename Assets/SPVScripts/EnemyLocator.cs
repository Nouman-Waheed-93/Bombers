using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocator : MonoBehaviour {

    private GameObject HUDEnemyDir;

	// Use this for initialization
	void Start () {
        HUDEnemyDir = Instantiate(Resources.Load<GameObject>("HUDEnemy"), GameObject.FindGameObjectWithTag(GlobalVals.HUDTag).transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
