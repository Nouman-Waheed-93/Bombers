using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileNGunTutor : MonoBehaviour {


    public GameObject MissileControlsTGO;
    public GameObject LTargetIntro, MissileLoadedIntro;

    private GameObject LockedTargetBox;

    public float StateStayTime = 2;
    float StateTime;
    float interruptTime;
    private enum TutorialState
    {
        Nothing,
        ShootAfterLock,
        IntroduceMissilesLoaded,
        CongratLock,
        RadarIntro,
        LockedTargetIntro,
        ShootMissile
    };

    private TutorialState currState;

    // Use this for initialization
    void Awake()
    {

        currState = TutorialState.Nothing;
        if (ConverstionHandler.instance) {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, GlobalVals.PlayerName + "! اليوم سوف تتعلم لاطلاق الصواريخ.", 6, true, Interrupted);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, GlobalVals.PlayerName + "! Aujourd'hui, vous apprendrez à tirer des missiles.", 6, true, Interrupted);
            }
            else {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, GlobalVals.PlayerName + "! Today you will learn to fire missiles.", 6, true, Interrupted);
            }
        }
        LockedTargetBox = GameObject.FindGameObjectWithTag("OVTBox");
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
                        if (StateTime > StateStayTime * 2)
                        {
                            currState = TutorialState.ShootAfterLock;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "عندما تدخل سيارة العدو الخاص بك مجموعة الرادار. وسوف تصبح هدف الرادار الخاص بك.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Quand un véhicule ennemi entre dans votre gamme de radar. Il deviendra votre objectif radar.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "When an enemy vehicle enters your radar range, Your radar will automatically lock it.", 10, true, Interrupted);
                                }
                            }
                                  StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.ShootAfterLock: {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 3) {
                            currState = TutorialState.IntroduceMissilesLoaded;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "ثم يمكنك اطلاق صاروخ على ذلك.", 3, false, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Alors vous pouvez tirer un missile à ce sujet.", 3, false, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Then you can fire a missile at it.", 3, false, Interrupted);
                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.IntroduceMissilesLoaded:
                    {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 2)
                        {
                            currState = TutorialState.CongratLock;
                            MissileLoadedIntro.SetActive(true);
                            Time.timeScale = 0;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "هنا يتم عرض عدد من الصواريخ تحميلها.", 6, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Voici le nombre de missiles chargés.", 6, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Here is displayed the number of missiles loaded.", 6, true, Interrupted);
                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.CongratLock:
                    {
                    //    IntroduceRadarUI();
                        StateTime += Time.unscaledDeltaTime;
                        if (StateTime > StateStayTime * 2) {
                            Time.timeScale = 1;
                        }
                        if (LockedTargetBox.activeSelf)
                        {
                            MissileLoadedIntro.SetActive(false);
                            Time.timeScale = 0;
                            currState = TutorialState.RadarIntro;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "جيد! اكتسب الرادار الخاص بك هدفا.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "bien! Votre radar a acquis une cible.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Good! Your radar has locked an enemy.", 10, true, Interrupted);
                                }
                            }
                           StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.RadarIntro:
                    {
                        StateTime += Time.unscaledDeltaTime;
                        if (StateTime > StateStayTime * 2)
                        {
                            currState = TutorialState.LockedTargetIntro;
                            LTargetIntro.SetActive(true);
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "هذا المربع يشير إلى الاتجاه إلى الهدف وتحته هو المسافة إليها.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Cette boîte indique la direction de la cible et en dessous il y a distance.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "This box indicates the direction to the locked target, and beneath it is the distance to it.", 10, true, Interrupted);
                                }
                            }
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.LockedTargetIntro:
                    {
                        StateTime += Time.unscaledDeltaTime;
                        if (StateTime > StateStayTime * 3)
                        {
                            LTargetIntro.SetActive(false);
                            Time.timeScale = 1;
                            currState = TutorialState.ShootMissile;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "الآن، يمكنك اطلاق صاروخ", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Maintenant, vous pouvez déclencher un missile", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Now, you can shoot.", 10, true, Interrupted);
                                }
                            }
                            MissileControlsTGO.SetActive(true);
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.ShootMissile:
                    {
                        break;
                    }
            }

    }

    void IntroduceRadarUI()
    {
        print(LockedTargetBox.name);
        if (LockedTargetBox.activeSelf)
        {
            
            StateTime += Time.deltaTime;
        }
        else
        {
            StateTime = 0;
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
            case TutorialState.ShootAfterLock:
                {
                    currState = TutorialState.Nothing;
                    StateTime = 20;
                    break;
                }
            case TutorialState.IntroduceMissilesLoaded:
                {
                    currState = TutorialState.ShootAfterLock;
                    StateTime = 20;
                    break;
                }
            case TutorialState.ShootMissile:
                {
                    currState = TutorialState.LockedTargetIntro;
                    MissileControlsTGO.SetActive(false);
                    StateTime = 20;
                    break;
                }
                
        }

        interruptTime = 3;

    }

    public void OnMissionComplete()
    {
        MissileControlsTGO.SetActive(false);
        SPVGM.instance.Promote(3);
    }
}
