using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PrimaryTarget : MonoBehaviour
{
    
    private Transform TargetUI;
    private Transform HUDEnemyDir;
    private Camera cam;
    private Renderer myRndr;

    void Start()
    {
        if (gameObject.layer == 10)
        {
            TargetUI = Instantiate(Resources.Load<GameObject>("TCross"), GameObject.FindGameObjectWithTag(GlobalVals.HUDTag).transform).transform;
            HUDEnemyDir = Instantiate(Resources.Load<GameObject>("HUDEnemy"), GameObject.FindGameObjectWithTag(GlobalVals.HUDTag).transform).transform;
            TargetUI.localPosition = Vector3.zero;
            HUDEnemyDir.localPosition = Vector3.zero;
            cam = Camera.main;
            myRndr = GetComponentInChildren<Renderer>();
            if (!myRndr)
                myRndr = GetComponentInParent<Renderer>();
            TargetUI.localScale = Vector3.one;
            HUDEnemyDir.localScale = Vector3.one;
            SPVGM.instance.AddTargets(1);
        }
        else {
            SPVGM.instance.AddFriendlyTargets(1);
        }
    }

    public void OnDestroy()
    {
        if (gameObject.layer == 10)
        {
            SPVGM.instance.RemoveTarget();
            if (TargetUI)
                Destroy(TargetUI.gameObject);
            if (HUDEnemyDir)
                Destroy(HUDEnemyDir.gameObject);
        }
        else {
            SPVGM.instance.FriendlyTargetDestroyed();
        }
    }

    void Update()
    {
        if (CameraPositioner.CameraInstance == null || gameObject.layer == 11)
            return;
        if(myRndr.isVisible)
        {
            if (!TargetUI.gameObject.activeSelf)
                TargetUI.gameObject.SetActive(true);
            if (HUDEnemyDir.gameObject.activeSelf)
                HUDEnemyDir.gameObject.SetActive(false);
            TargetUI.position = RectTransformUtility.WorldToScreenPoint(cam, transform.position);
        }
        else
        {
            if (TargetUI.gameObject.activeSelf)
                TargetUI.gameObject.SetActive(false);
            if (!HUDEnemyDir.gameObject.activeSelf)
                HUDEnemyDir.gameObject.SetActive(true);
            Vector3 dir = transform.position - CameraPositioner.CameraInstance.position;
            float angle = -Mathf.Rad2Deg * Mathf.Atan2(dir.x, dir.y);
            HUDEnemyDir.eulerAngles = new Vector3(0, 0, angle);
        }

        TargetUI.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
    }
    
}