using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTutorial : MonoBehaviour {


    public GameObject CannonControlTGO;
    
    public float StateStayTime = 2;
    float StateTime;
    float interruptTime;
    private enum TutorialState
    {
        Nothing,
        ExplainCannon,
        BriefMission, 
        Finished
    };

    private TutorialState currState;

    // Use this for initialization
    void Awake()
    {

        currState = TutorialState.Nothing;
        if (ConverstionHandler.instance)
            ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, GlobalVals.PlayerName + "!", 6, false, Interrupted);
  
    }


    // Update is called once per frame
    void Update()
    {
        if (SPVGM.instance.GetGameState == SPVGM.GameState.MissionComplete)
            return;
        if (interruptTime > 0)
            interruptTime -= Time.deltaTime;
        else
            switch (currState)
            {
                case TutorialState.Nothing:
                    {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 2)
                        {
                            currState = TutorialState.ExplainCannon;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic) {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "يمكنك أيضا استخدام مدافع لاسقاط الطائرات دون استهداف الرادار.", 10, true, Interrupted);

                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Vous pouvez également utiliser des canons pour abattre des avions sans ciblage par radar.", 10, true, Interrupted);

                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "You can also use cannons to shoot down airplanes without radar locking.", 10, true, Interrupted);

                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.ExplainCannon:
                    {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 4)
                        {
                            currState = TutorialState.BriefMission;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "لذلك، في هذه المهمة سيكون لديك لتدمير تلك الطائرة دون استخدام الصواريخ.", 10, true, Interrupted);

                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Alors, dans cette mission, il faudra détruire cet avion sans utiliser de missiles.", 10, true, Interrupted);

                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "So, in this mission you will have to kill that bandit without using missiles.", 6, true, Interrupted);

                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.BriefMission:
                    {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 2)
                        {
                            CannonControlTGO.SetActive(true);
                            currState = TutorialState.Finished;
                            StateTime = 0;
                        }
                      
                        break;
                    }
                case TutorialState.Finished:
                    {
                        StateTime += Time.deltaTime;
                        if (CannonControlTGO.activeSelf && StateTime > StateStayTime * 2) {
                            CannonControlTGO.SetActive(false);
                        } 
                        break;
                    }
            }

    }

   

    void Interrupted()
    {

        switch (currState)
        {
            case TutorialState.Nothing:
                {
                    StateTime = 0;
                    break;
                }
            case TutorialState.ExplainCannon:
                {
                    currState = TutorialState.Nothing;
                    StateTime = 20;
                    break;
                }
            case TutorialState.BriefMission:
                {
                    currState = TutorialState.BriefMission;
                    StateTime = 20;
                    break;
                }
           
        }

        interruptTime = 3;

    }

   
}
