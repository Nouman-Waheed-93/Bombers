using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTCrossAssigner : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
        GameObject detailPointer = GameObject.Find("TCross(Clone)");
        if (detailPointer)
            GetComponent<TutorialDetailPointer>().DetailAbout = detailPointer.GetComponent<RectTransform>();
    }
	
	
}
