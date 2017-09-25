using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArabicSupport;
using UnityEngine.UI;

public class UILanguageHandler : MonoBehaviour {

    public bool AutoSetTexts = false;
    public bool tashkeel = true;
    public bool hinduNumbers = true;
    public Font ArabicFont, EnglishFont, FrenchFont;
    
    public static UILanguageHandler instance;

    [System.Serializable]
    public class LocalizedText {
        public Text ElementText;
        public string ArabicText;
        public bool resetLines = false;
        public int ArabicFontSize;
        public string EnglishText;
        public int EnglishFontSize;
        public string FrenchText;
        public int FrenchFontSize;

    }

    public LocalizedText[] allTexts;


    void Start() {
        instance = this;
        if (AutoSetTexts)
            HandleText();
    }
    
    public void HandleText() {
        foreach (LocalizedText CurrText in allTexts)
        {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                CurrText.ElementText.text = ArabicFixer.Fix(CurrText.ArabicText, tashkeel, hinduNumbers);
                if(CurrText.resetLines)
                    SetArabicLines(CurrText.ElementText);
                CurrText.ElementText.font = ArabicFont;
                CurrText.ElementText.fontSize = CurrText.ArabicFontSize;
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.English)
            {
                CurrText.ElementText.text = CurrText.EnglishText;
                CurrText.ElementText.font = EnglishFont;
                CurrText.ElementText.fontSize = CurrText.EnglishFontSize;
            }
            else
            {
                CurrText.ElementText.text = CurrText.FrenchText;
                CurrText.ElementText.font = FrenchFont;
                CurrText.ElementText.fontSize = CurrText.FrenchFontSize;
            }
        }
    }

    public void SetArabicLines(Text myText) {
        Canvas.ForceUpdateCanvases();
        string newTxt = "";
        for (int i = myText.cachedTextGenerator.lines.Count - 1; i >= 0; i--)
        {
            int startIndex = myText.cachedTextGenerator.lines[i].startCharIdx;
            int endIndex = (i == myText.cachedTextGenerator.lines.Count - 1) ?
                myText.text.Length
                : myText.cachedTextGenerator.lines[i + 1].startCharIdx;
            int length = endIndex - startIndex;
            newTxt += myText.text.Substring(startIndex, length);
            if (i != 0)
                newTxt += "\n";
          //  Debug.Log(myText.text.Substring(startIndex, length));
        }
        myText.text = newTxt;
    }

}
