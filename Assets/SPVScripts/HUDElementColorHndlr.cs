using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUDElementColorHndlr : MonoBehaviour {



	// Use this for initialization
	void Start () {
        GetComponentInChildren<Image>().color = HUDColorSetter.instance.HUDColor;
        GetComponentInChildren<Outline>().effectColor = HUDColorSetter.instance.OutlineColor;
    }
    
}
