using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortBomberMission : MonoBehaviour {


    public PlaneHealth[] BmbrHlth;
    public string TowerControlDialog, PilotDialog, 
        ArabicWMDialog, ArabicPilotDialog, FrenchWMDialog, FrenchPilotDialog;
    public float StateStayTime = 2;
    float StateTime;
    private float interruptTime = 0;
    public int BomberTargets = 5;
    private enum SpeakState
    {
        Nothing,
        GroundAskAirSupport,
        PilotReply
    };

    private SpeakState currState;


    // Use this for initialization
    void Start()
    {
        currState = SpeakState.Nothing;
        SPVGM.instance.AddTargets(BomberTargets);
        for (int i = 0; i < BmbrHlth.Length; i++)
        {
            BmbrHlth[i].OnDamageTaken = UnderAttack;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (interruptTime > 0)
            interruptTime -= Time.deltaTime;
        else
            switch (currState)
            {
                case SpeakState.Nothing:
                    {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 4)
                        {
                            currState = SpeakState.GroundAskAirSupport;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, GlobalVals.PlayerName + ArabicWMDialog, 10, false, null);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, GlobalVals.PlayerName + FrenchWMDialog, 10, false, null);
                                }
                                else {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, GlobalVals.PlayerName + TowerControlDialog, 10, false, null);
                                }
                            }
                                
                            StateTime = 0;
                        }
                        break;
                    }
                case SpeakState.GroundAskAirSupport:
                    {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 4)
                        {
                            currState = SpeakState.PilotReply;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, ArabicPilotDialog, 10, false, null);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, FrenchPilotDialog, 10, false, null);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, PilotDialog, 10, false, null);
                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case SpeakState.PilotReply:
                    {
                        break;
                    }
            }
    }

    public void UnderAttack() {


        if (ConverstionHandler.instance) {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "أنا تحت الهجوم", 10, false, null);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Je suis attaqué", 10, false, null);
            }
            else
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "I am under attack", 10, false, null);
            }
        }       
       
    }

    void Interrupted()
    {

        switch (currState)
        {
            case SpeakState.GroundAskAirSupport:
                {
                    currState = SpeakState.Nothing;
                    StateTime = 20;
                    break;
                }
            case SpeakState.PilotReply:
                {
                    currState = SpeakState.GroundAskAirSupport;
                    StateTime = 20;
                    break;
                }
        }
        interruptTime = 3;
    }
}
