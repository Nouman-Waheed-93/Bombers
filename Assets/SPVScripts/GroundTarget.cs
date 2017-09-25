using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTarget : MonoBehaviour
{
    public GameObject ObjectOnDestroy;
	public int totalTargetSiblings = 0;
    public Transform TargetUI;
    private static Camera cam;

    void Start() {
        if (totalTargetSiblings > 1 && ObjectOnDestroy == null)
        {
            ObjectOnDestroy = Instantiate(Resources.Load<GameObject>("WeakpointExplosion"), transform.position, Quaternion.identity, transform.parent);
            ObjectOnDestroy.SetActive(false);
        }
        if (!cam)
            cam = Camera.main;
        if (gameObject.layer == 11)
            return;
        TargetUI = Instantiate(Resources.Load<GameObject>("TargetLoc"), GameObject.FindGameObjectWithTag(GlobalVals.HUDTag).transform).transform;
        TargetUI.localPosition = Vector3.zero;
        TargetUI.transform.localScale = Vector3.one;
    
    }

	public void TargetHit ()
	{
        if (ObjectOnDestroy)
            ObjectOnDestroy.SetActive(true);
       
        GetComponentInParent<SPVHealth> ().Damage (100/totalTargetSiblings);
        if(Vector3.Distance(transform.position, cam.transform.position) < 300)
            SPVCamera.Shake();
        if (gameObject.layer == 10)
        {
            SPVGM.instance.GroundTargetDestroyed();
            Destroy(TargetUI.gameObject);
        }
        Destroy (gameObject);

	}

    void Update() {
        if (gameObject.layer == 11)
            return;
        TargetUI.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
    }


}
