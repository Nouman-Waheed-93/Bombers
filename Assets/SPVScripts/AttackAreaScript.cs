using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaScript : MonoBehaviour {

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
                            if (ConverstionHandler.instance)
                                ConverstionHandler.instance.Speak(ConverstionHandler.Character.GroundLeaderC, "", 6, true, Interrupted);
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
                            if (ConverstionHandler.instance)
                                ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, "", 10, false, Interrupted);
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


