using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackModeHUD : MonoBehaviour {

    public static AttackModeHUD instance;
    
    public Sprite GMSprite, AMSprite;

    public Image AMImg;

    void Awake() {
        instance = this;
     }

    public void SetGMode() {
        AMImg.sprite = GMSprite;    
    }

    public void SetAMode() {
        AMImg.sprite = AMSprite;
    }

}
