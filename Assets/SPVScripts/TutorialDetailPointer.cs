using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDetailPointer : MonoBehaviour {
    
    public RectTransform DetailAbout;

    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (DetailAbout)
        {
       
            transform.position = DetailAbout.position;

        }
    
    }
}
