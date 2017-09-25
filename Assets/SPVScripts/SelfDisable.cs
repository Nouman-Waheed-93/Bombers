using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDisable : MonoBehaviour {

    public float LifeSpan = 2;

    private float LifeStart = 0;

	// Use this for initialization
	void OnEnable () {
        LifeStart = 0;
	}

	// Update is called once per frame
	void Update () {
		LifeStart += Time.deltaTime;
        if(LifeStart > LifeSpan){
            gameObject.SetActive(false);
        }
	}
}
