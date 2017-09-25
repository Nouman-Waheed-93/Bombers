using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponryHUD : MonoBehaviour
{
    Rigidbody myRB;
	Transform ImpactPointHUD;
	Transform TargetBox;
    Transform OutOfViewTargetBox;
    AudioSource LockAud;
    bool LockAudPlayed;
	SPVWeaponController myWeaponry;
    Text DistText;
    Camera cam;
    Plane[] planes;
    RWR myRWR;
    Text BombsText;
    Text MissilesText;
    Text SpeedText;
    
    int RememberedBombQty;
    int RememberedMissilesQty;
	// Use this for initialization
	void Start ()
	{
		myRB = GetComponentInParent<Rigidbody> ();
		ImpactPointHUD = GameObject.FindGameObjectWithTag ("CCIP").transform;
		TargetBox = GameObject.FindGameObjectWithTag ("TBox").transform;
        OutOfViewTargetBox = GameObject.FindGameObjectWithTag("OVTBox").transform;
        LockAud = GameObject.FindGameObjectWithTag("HUDCanvas").GetComponent<AudioSource>();
        BombsText = GameObject.FindGameObjectWithTag("BombsText").GetComponent<Text>();
        MissilesText = GameObject.FindGameObjectWithTag("MissilesText").GetComponent<Text>();
        SpeedText = GameObject.FindGameObjectWithTag("SpeedTxt").GetComponent<Text>();
        DistText = OutOfViewTargetBox.GetComponentInChildren<Text>();
		myWeaponry = GetComponent<SPVWeaponController> ();
        myWeaponry.OnAttackModeChange = DisplayAttackMode;
        DisplayAttackMode();
        myRWR = GetComponentInParent<RWR>();
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        BombsText.transform.parent.gameObject.SetActive(myWeaponry.MaxBombs > 0);
        MissilesText.transform.parent.gameObject.SetActive(myWeaponry.MaxMissiles > 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		DrawCCIP ();
		PositionTargetBox ();
        DrawRWR();
        DisplayWeaponStock();
        DisplaySpeed();
	}

    void DisplayAttackMode() {
        if (myWeaponry.SelectedAttackMode == SPVWeaponController.AttackMode.AttackAir)
            AttackModeHUD.instance.SetAMode();
        else
            AttackModeHUD.instance.SetGMode();
    }

    void DisplaySpeed() {

        if(GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            SpeedText.text = ArabicSupport.ArabicFixer.Fix(((int)(myRB.velocity.magnitude * 6)).ToString(), true, true);
        else
            SpeedText.text = ((int)(myRB.velocity.magnitude * 6 )).ToString() ;

    }

    void DisplayWeaponStock() {
        if (RememberedBombQty != myWeaponry.GetBombsLoaded)
        {
            RememberedBombQty = myWeaponry.GetBombsLoaded;
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                BombsText.text = ArabicSupport.ArabicFixer.Fix(RememberedBombQty.ToString(), true, true);
            else
                BombsText.text = RememberedBombQty.ToString();
        }
        if (RememberedMissilesQty != myWeaponry.GetMissilesLoaded)
        {
            RememberedMissilesQty = myWeaponry.GetMissilesLoaded;
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                MissilesText.text = ArabicSupport.ArabicFixer.Fix(RememberedMissilesQty.ToString(), true, true);
            else
                MissilesText.text = RememberedMissilesQty.ToString();
        }
    }

    void DrawRWR() {

        for (int i = 0; i < myRWR.threatHUDLines.Count; i++) { 
             Vector3 dir = myRWR.threats[i].position - transform.position;
             float angle = -Mathf.Rad2Deg * Mathf.Atan2(dir.x, dir.y);
             myRWR.threatHUDLines[i].transform.position = RectTransformUtility.WorldToScreenPoint(cam, myRB.transform.position);
             myRWR.threatHUDLines[i].transform.eulerAngles = new Vector3(0, 0, angle);
             if (Vector3.Distance(myRWR.threats[i].transform.position, myRB.transform.position) < 100)
                 myRWR.threatHUDLines[i].gameObject.SetActive(false);
        }

    }
    
	void PositionTargetBox ()
	{
		if (myWeaponry.GetMissilesLoaded < 1) {
            if(TargetBox.gameObject.activeSelf)
			    TargetBox.gameObject.SetActive (false);
            if(OutOfViewTargetBox.gameObject.activeSelf)
                OutOfViewTargetBox.gameObject.SetActive(false);
			return;
		}
		if (myWeaponry.SelectedTarget) {

            if (!LockAudPlayed && !LockAud.isPlaying) {
                LockAud.Play();
                LockAudPlayed = true;
            }

            if(myWeaponry.SelectedTargetRenderer.isVisible)
            {
                if (!TargetBox.gameObject.activeSelf)
                    TargetBox.gameObject.SetActive(true);
                if (OutOfViewTargetBox.gameObject.activeSelf)
                    OutOfViewTargetBox.gameObject.SetActive(false);
                TargetBox.position = RectTransformUtility.WorldToScreenPoint(cam, myWeaponry.SelectedTarget.position);
            }
            else
            {
                if (TargetBox.gameObject.activeSelf)
                    TargetBox.gameObject.SetActive(false);
                if (!OutOfViewTargetBox.gameObject.activeSelf)
                    OutOfViewTargetBox.gameObject.SetActive(true);
                DistText.text = Mathf.RoundToInt(Vector3.Distance(myRB.transform.position, myWeaponry.SelectedTarget.position)).ToString();
                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                    DistText.text = ArabicSupport.ArabicFixer.Fix(DistText.text, true, true);
                Vector3 dir = myWeaponry.SelectedTarget.position - transform.position;
                float angle = -Mathf.Rad2Deg * Mathf.Atan2(dir.x, dir.y);
                OutOfViewTargetBox.eulerAngles = new Vector3(0, 0, angle);
            }
        }
        else
        {
            LockAudPlayed = false;
            if(TargetBox.gameObject.activeSelf)
			    TargetBox.gameObject.SetActive (false);
            if(OutOfViewTargetBox.gameObject.activeSelf)
                OutOfViewTargetBox.gameObject.SetActive(false);
		}
	}

    void DrawCCIP() {

        if (myWeaponry.GetBombsLoaded < 1)
        {
            ImpactPointHUD.gameObject.SetActive(false);
            return;
        }

        ImpactPointHUD.gameObject.SetActive(true);
        ImpactPointHUD.position = RectTransformUtility.WorldToScreenPoint(cam, myWeaponry.CCIP());

    }

	
}
