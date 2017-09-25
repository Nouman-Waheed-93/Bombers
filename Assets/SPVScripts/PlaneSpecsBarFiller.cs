using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneSpecsBarFiller : MonoBehaviour {

    public int TheVal;

    private Slider mySlider;
    
    void OnEnable() {
        if (!mySlider)
            mySlider = GetComponent<Slider>();
        mySlider.value = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if(mySlider.value < TheVal)
            mySlider.value += Time.unscaledDeltaTime * mySlider.maxValue;
	}
}
