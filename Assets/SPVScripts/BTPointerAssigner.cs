using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTPointerAssigner : MonoBehaviour {
    
	// Use this for initialization
	void OnEnable () {
        Pointer detailPointer = FindObjectOfType<Pointer>();
        if (detailPointer)
            GetComponent<TutorialDetailPointer>().DetailAbout = detailPointer.GetComponent<RectTransform>();
    }
    
}
