using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAirTutorial : MonoBehaviour {

    public GameObject AttackModeControlsTGO;
    
    public GameObject AMSymbolIntroTGO;
    
    public float StateStayTime = 2;
    float StateTime;
    float interruptTime;
    private enum TutorialState
    {
        Nothing,
        ExplainAirAttack,
        IntroduceAMSymbolHUD,
        GroundAMSymbol,
        ChangeAMode,
        AirAMSymbol,
        Finished
    };

    private TutorialState currState;

    // Use this for initialization
    void Start()
    {

        currState = TutorialState.Nothing;
        if (ConverstionHandler.instance)
            ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, GlobalVals.PlayerName + "!", 6, true, Interrupted);

        SPVGM.instance.CallOnMissionComplete = OnMissionComplete;

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
                        if (StateTime > StateStayTime * 3)
                        {
                            currState = TutorialState.ExplainAirAttack;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic) {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "يمكنك أيضا إطلاق الصواريخ على الطائرات.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Vous pouvez également tirer des missiles dans des avions.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "You can also fire missiles at airplanes.", 10, true, Interrupted);
                                }

                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.ExplainAirAttack:
                    {
                        StateTime += Time.deltaTime;
                       if (StateTime > StateStayTime * 3)
                        {
                            currState = TutorialState.IntroduceAMSymbolHUD;
                            Time.timeScale = 0;
                            AMSymbolIntroTGO.SetActive(true);
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "يشير هذا الرمز إلى وضع الهجوم الحالي.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Ce symbole indique votre mode d'attaque actuel.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "This symbol indicates your current attack mode.", 10, true, Interrupted);
                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.IntroduceAMSymbolHUD:
                    {
                        StateTime += Time.unscaledDeltaTime;
                        if (StateTime > StateStayTime * 3)
                        {
                           currState = TutorialState.GroundAMSymbol;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "يشير رمز الخزان إلى وضع الهجوم الأرضي.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Le symbole du réservoir indique le mode d'attaque au sol.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "The tank symbol indicates ground attack mode.", 10, true, Interrupted);
                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.GroundAMSymbol:
                    {
                        StateTime += Time.unscaledDeltaTime;
                      if (StateTime > StateStayTime * 3)
                        {
                            Time.timeScale = 1;
                            AMSymbolIntroTGO.SetActive(false);
                            AttackModeControlsTGO.SetActive(true);
                            currState = TutorialState.ChangeAMode;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "لإطلاق الصواريخ على دوريات الطائرات في المنطقة، والتحول إلى وضع الهجوم الجوي.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Pour tirer un missile sur l'avion qui patrouille dans la zone, passer au mode attaque aérienne.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "To lock and fire missile at the aircraft patrolling in the area, switch to air attack mode.", 10, true, Interrupted);
                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }

                case TutorialState.ChangeAMode:
                    {
                        StateTime += Time.unscaledDeltaTime;
                        if (AttackModeHUD.instance.AMImg.sprite == AttackModeHUD.instance.AMSprite)
                        {
                            Time.timeScale = 0;
                            currState = TutorialState.AirAMSymbol;

                            AMSymbolIntroTGO.SetActive(true);
                            AttackModeControlsTGO.SetActive(false);
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "جيد! لاحظ أنه الآن قد تغيرت إلى رمز الطائرة مما يعني أنه يمكنك الآن مهاجمة الطائرات.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "bien! Remarquez que maintenant il est devenu un symbole de l'avion, ce qui signifie que vous pouvez maintenant attaquer un avion.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Good! Notice that now it has changed into an aircraft symbol which means you can now attack aircraft.", 10, true, Interrupted);
                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.AirAMSymbol:
                    {
                        StateTime += Time.unscaledDeltaTime;
                     
                        if (StateTime > StateStayTime * 4)
                        {
                            AMSymbolIntroTGO.SetActive(false);
                            Time.timeScale = 1;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "الآن يذهب واسقاط تلك الطائرة.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Maintenant, allez abattre cet avion.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Now, go and shoot down that bandit.", 4, true, Interrupted);
                                }
                            }
                            currState = TutorialState.Finished;
                        }
                        break;
                    }
                case TutorialState.Finished:
                    {
                    
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
            case TutorialState.ExplainAirAttack:
                {
                    currState = TutorialState.Nothing;
                    StateTime = 20;
                    break;
                }

            case TutorialState.ChangeAMode:
                {
                    currState = TutorialState.GroundAMSymbol;
                    StateTime = 20;
                    break;
                }
        }

        interruptTime = 3;

    }

    public void OnMissionComplete()
    {
     
    }


}
