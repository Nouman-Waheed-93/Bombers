using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotComments : MonoBehaviour {

    public static PilotComments instance;
    public Sprite[] PilotAvatars;
    [SerializeField]
    private LocalizedComments[] AComments;
    [SerializeField]
    private LocalizedComments[] GComments;

	// Use this for initialization
	void Start () {
        instance = this;
        ConverstionHandler.instance.PlayerFoto = PilotAvatars[GlobalVals.AvatarInd];
	}

    // Update is called once per frame
    public void AirTargetHitComment() {
        if (ConverstionHandler.instance) {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, AComments[Random.Range(0, AComments.Length)].ArabicComment, 1, false, null);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, AComments[Random.Range(0, AComments.Length)].FrenchComment, 1, false, null);
            }
            else {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, AComments[Random.Range(0, AComments.Length)].EnglishComment, 1, false, null);
            }
        }
        
    }

    public void GroundTargetHitComment() {
        print(GlobalVals.SelectedLanguage);
        if (ConverstionHandler.instance)
        {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, GComments[Random.Range(0, GComments.Length)].ArabicComment, 1, false, null);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
            //    print("French");
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, GComments[Random.Range(0, GComments.Length)].FrenchComment, 1, false, null);
            }
            else
            {
             //   print("English");
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, GComments[Random.Range(0, GComments.Length)].EnglishComment, 1, false, null);
            }
        }
    }

    [System.Serializable]
    class LocalizedComments {

        public string ArabicComment, EnglishComment, FrenchComment;

    }

}
