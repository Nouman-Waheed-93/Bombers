using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDColorSetter : MonoBehaviour {

    public static HUDColorSetter instance;

    void Awake() {
        instance = this;
        SetColors();
  }

    public Color HUDColor;
    public Color OutlineColor;

	// Update is called once per frame
	public void SetColors () {
        Image[] HUDImages = GetComponentsInChildren<Image>();
        for (int i = 0; i < HUDImages.Length; i++) {
            HUDImages[i].color = HUDColor;
        }

        Outline[] HUDOutlines = GetComponentsInChildren<Outline>();
        for (int i = 0; i < HUDOutlines.Length; i++)
        {
            HUDOutlines[i].effectColor = OutlineColor;
        }

        Text[] HUDTexts = GetComponentsInChildren<Text>();
        for (int i = 0; i < HUDTexts.Length; i++) {
            HUDTexts[i].color = HUDColor;
        }

	}
}
