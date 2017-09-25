using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArabicSupport;
using UnityEngine.UI;

public class InputFieldHandler : MonoBehaviour {

    Text myTxt;

	// Use this for initialization
	void Awake () {
        myTxt = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TextEntered(string newTxt) {

        if (!myTxt)
            return;
        if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
        {
            myTxt.text = ArabicFixer.Fix(newTxt, true, true);
        }
        else {
            myTxt.text = newTxt;
        }

    }

}
