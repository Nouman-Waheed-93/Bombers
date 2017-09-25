using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticStartupInstructions : MonoBehaviour {

    [SerializeField]
    private LocalizedInstructions[] InstructionGOs;

    [SerializeField]
    private Text InstructionLbl;

    private int currInstruction;

    void Start() {
        Time.timeScale = 0;
        InstructionLbl.text = GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic ?
            ArabicSupport.ArabicFixer.Fix("تعليمات", true, true) : GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French ?
            "instructions" : "instructions";
        InstructionGOs[currInstruction].InstructionGO.SetActive(true);
        SetInstructionText();
    }

    void SetInstructionText() {
        Text currInstructionTxt = InstructionGOs[currInstruction].InstructionGO.transform.GetComponentInChildren<Text>();
        if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
        {
            currInstructionTxt.text
                = ArabicSupport.ArabicFixer.Fix(InstructionGOs[currInstruction].ArabicInstruction, true, true);
            UILanguageHandler.instance.SetArabicLines(currInstructionTxt);
        }
        else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
        {
            currInstructionTxt.text
              = InstructionGOs[currInstruction].FrenchInstruction;
        }
        else
        {
            currInstructionTxt.text
              = InstructionGOs[currInstruction].EnglishInstruction;
        }
    }

    public void NextInstruction() {
       
        InstructionGOs[currInstruction].InstructionGO.SetActive(false);
        currInstruction++;
        if (currInstruction >= InstructionGOs.Length)
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            return;
        }
        InstructionGOs[currInstruction].InstructionGO.SetActive(true);
        SetInstructionText();
    }

    [System.Serializable]
    class LocalizedInstructions {
        public GameObject InstructionGO;
        public string ArabicInstruction, FrenchInstruction, EnglishInstruction;
    }

}
