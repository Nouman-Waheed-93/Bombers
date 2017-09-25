using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlReportBogies : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if(GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "هناك طائرات في المنطقة التي تستهدفها. البقاء حادة.", 3, true, Interrupted);
        else if(GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "Il y a des avions dans votre zone cible. Reste ferme.", 3, true, Interrupted);
        else
            ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "There are bogies in your target area, Stay sharp.", 3, true, Interrupted);

    }

    void Interrupted (){
        Invoke("Start", 3);
    }
    	
}
