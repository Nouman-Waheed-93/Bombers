﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberApproachingDialog : MonoBehaviour {


    public string TowerControlDialog, PilotDialog, ArabicControlDialog, ArabicPilotDialog, FrenchControlDialog, FrenchPilotDialog;
    public float StateStayTime = 2;
    float StateTime;
    private float interruptTime = 0;
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
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic) {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, GlobalVals.PlayerName + ArabicControlDialog, 6, true, null);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, GlobalVals.PlayerName + FrenchControlDialog, 6, true, null);
                                }
                                else   {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, GlobalVals.PlayerName + TowerControlDialog, 6, true, null);
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
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC,FrenchPilotDialog, 10, false, null);
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