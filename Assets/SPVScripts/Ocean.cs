using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour {

    public float XSpeed = 0;
    public float YSpeed = 1;

    private Material myMat;

	// Use this for initialization
	void Start () {
        myMat = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        myMat.mainTextureOffset = new Vector2(myMat.mainTextureOffset.x + XSpeed * Time.deltaTime, 
            myMat.mainTextureOffset.y + YSpeed * Time.deltaTime);
	}
}
