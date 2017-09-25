using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    	

	}

    

    public void SetEnglish() {
        PlayerPrefs.SetString("Language", "English");
        GlobalVals.SelectedLanguage = GlobalVals.SupportedLanguages.English;
    }

    public void SetArabic() {
        PlayerPrefs.SetString("Language", "Arabic");
        GlobalVals.SelectedLanguage = GlobalVals.SupportedLanguages.Arabic;
    }

    public void SetFrench() {
        PlayerPrefs.SetString("Language", "French");
        GlobalVals.SelectedLanguage = GlobalVals.SupportedLanguages.French;
    }

}
