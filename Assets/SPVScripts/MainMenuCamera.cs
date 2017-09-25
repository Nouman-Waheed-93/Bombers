using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour {
    
    private Transform LookAtTarget;

    private Vector3 distanceVect = new Vector3(0, 0, 122);

    public Transform LATarget {
        set {
            LookAtTarget = value;
        }
    }

    Vector3 rotVect = new Vector3(0, 0, 0.1f);
    
	void Update () {

        if (!LookAtTarget)
            return;

        if (LookAtTarget.position != transform.position) {
            transform.position = Vector3.Lerp(transform.position, LookAtTarget.position - distanceVect, 0.1f);
        }

        transform.GetChild(0).LookAt(transform.position + distanceVect, -Vector3.forward);
        transform.Rotate(rotVect);

	}
}
