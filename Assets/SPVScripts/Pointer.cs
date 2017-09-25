using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.localPosition = new Vector3(0, transform.root.GetComponent<RectTransform>().rect.height * 0.4f, 0);	
	}
	
}
