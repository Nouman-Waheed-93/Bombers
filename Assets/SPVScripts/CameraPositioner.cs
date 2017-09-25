using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositioner : MonoBehaviour {

    public static Transform CameraInstance;

    void Awake() {

        CameraInstance = transform;
    }

    public void SetCameraPosition(float Speed) {

        transform.localPosition = new Vector3(0, Mathf.Clamp(Speed * 1.5f * 10, 10, 80),0);

    }

    void OnDestroy()
    {
        if(SPVGM.instance)
            SPVGM.instance.gameObject.AddComponent<AudioListener>();
    }

}
