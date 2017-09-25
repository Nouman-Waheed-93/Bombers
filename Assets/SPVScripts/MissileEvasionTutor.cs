using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileEvasionTutor : MonoBehaviour {

    public GameObject ConceptTGO;
    GameObject ThreatLineTGO;

    public float StateStayTime = 2;
    float StateTime;
    float interruptTime;
    
    private PlaneHealth playerHealth;

    private enum TutorialState
    {
        Greetings,
        TargetBriefing,
        LearningObjective,
        TechniqueExplanation,
        Practical
    };

    private TutorialState currState;

    // Use this for initialization
    void Start () {
        currState = TutorialState.Greetings;
        if (ConverstionHandler.instance) {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
               GlobalVals.PlayerName +
               "!, اليوم سوف تتعلم للتهرب من الصواريخ.", 6, true, Interrupted);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
               GlobalVals.PlayerName +
               "!, Aujourd'hui, vous apprendrez à échapper à des missiles.", 6, true, Interrupted);
            }
            else
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
               GlobalVals.PlayerName +
               "!, Today You will learn to Evade Missiles.", 6, true, Interrupted);
            }
        }

        RWR playerRwr = FindObjectOfType<RWR>();
        playerRwr.CallOnThreat = MissileFired;
        playerRwr.CallOnDethreat = MissileEvaded;
        playerHealth = CameraPositioner.CameraInstance.GetComponentInParent<PlaneHealth>();
        SPVGM.instance.CallOnMissionComplete = OnMissionComplete;

    }

    // Update is called once per frame
    void Update () {
        if (SPVGM.instance.GetGameState == SPVGM.GameState.MissionComplete)
            return;

        if (interruptTime > 0)
            interruptTime -= Time.deltaTime;
        else
            switch (currState)
            {
                case TutorialState.Greetings:
                    {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 3)
                        {
                            currState = TutorialState.TargetBriefing;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
                                 "أهدافك الأساسية هما صاروخ أرض-جو وحدات.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
                                 "Vos objectifs principaux sont deux  missile anti-aérien Unités", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Your Primary Targets are two SAM units.", 10, true, Interrupted);
                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.TargetBriefing:
                    {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 3)
                        {
                            currState = TutorialState.LearningObjective;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
                                  "يجب عليك التهرب من الصواريخ التي يطلقونها لاستكمال هذه المهمة التدريب.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
                                   "Vous devez échapper aux missiles qu'ils tirent pour compléter cette mission de formation", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "You must evade the missiles they fire to complete this training mission.", 10, true, Interrupted);
                                }
                            }
                                
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.LearningObjective:
                    {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 3)
                        {
                            currState = TutorialState.TechniqueExplanation;
                            ConceptTGO.SetActive(true);
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
                                   "يجب أن تبقي طائرة تتحرك في 90 درجة من الاتجاه إلى صاروخ بسرعة عالية.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
                                   "Vous devriez garder votre jet en mouvement à 90 degrés de la direction au missile à haute vitesse", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "You should keep your jet moving at 90 degrees from the direction to missile at high speed.", 10, true, Interrupted);
                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.TechniqueExplanation:
                    {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 4)
                        {
                            ConceptTGO.SetActive(false);
                            currState = TutorialState.Practical;
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.Practical:
                    {
                        break;
                    }
            }

   }

    public void MissileFired() {

        currState = TutorialState.Practical;
        if (ConverstionHandler.instance) {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
               " تحويل 90 درجة إلى هذا الخط.", 10, true, Interrupted);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
               "Tournez 90 degrés sur cette ligne", 10, true, Interrupted);
            }
            else
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Turn 90 degrees to this line.", 10, true, Interrupted);
            }
        }
    
    }

    public void MissileEvaded() {
        StartCoroutine("CheckHealthAfterMissileDissappears");
    }

    IEnumerator CheckHealthAfterMissileDissappears() {

        yield return new WaitForSeconds(0.2f);

        if (playerHealth.currHealth < playerHealth.maxHealth)
        {
            SPVGM.instance.FailMission();
            if (ConverstionHandler.instance)
            {
                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                {
                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
                   "كنت من المفترض أن تهرب من الصاروخ.", 10, true, Interrupted);
                }
                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                {
                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
                   "Vous étiez censé échapper au missile", 10, true, Interrupted);
                }
                else
                {
                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "You were supposed to evade the missile.", 10, true, Interrupted);
                }
            }
        }
        else if (ConverstionHandler.instance) {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
               "التهرب الجميل!", 10, true, Interrupted);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC,
               "Bonne évasion!", 10, true, Interrupted);
            }
            else
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Nice Save!", 10, false, Interrupted);
            }
        }
            


    }

    void Interrupted()
    {

        switch (currState)
        {
            case TutorialState.Greetings:
                {
                    StateTime = 0;
                    break;
                }
            case TutorialState.TargetBriefing:
                {
                    currState = TutorialState.Greetings;
                    StateTime = 20;
                    break;
                }
            case TutorialState.LearningObjective:
                {
                    currState = TutorialState.TargetBriefing;
                    StateTime = 20;
                    break;
                }
            case TutorialState.TechniqueExplanation:
                {
                    currState = TutorialState.LearningObjective;
                    ConceptTGO.SetActive(false);
                    StateTime = 20;
                    break;
                }

            case TutorialState.Practical:
                {
                    currState = TutorialState.TechniqueExplanation;
                 //   ThreatLineTGO.SetActive(false);
                    StateTime = 20;
                    break;
                }
        }

        interruptTime = 3;

    }

    public void OnMissionComplete()
    {

        SPVGM.instance.Promote(2);
    }

}

