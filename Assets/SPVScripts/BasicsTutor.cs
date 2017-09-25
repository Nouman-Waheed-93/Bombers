using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicsTutor : MonoBehaviour {

    public GameObject PlaneControlsTGO;
    public GameObject ThrottleControlsTGO;
    public GameObject ASpeedIntroTGO, BLoadedIntroTGO, BImpactPointerTGO, PTargetPointerTGO, PTargetCrossTGO;
    public GameObject BombTGO;

    private GameObject PTPointer;

    public float StateStayTime = 2;
    float StateTime;
    float interruptTime;
    private enum TutorialState {
        Nothing,
        PlaneControlling,
        AirSpeedIntro,
        ThrottleControl,
        BLoadedTxtIntro,
        BImpactPIntro,
        PTrgtPointerIntro,
        BombingSpeedTip,        
        ThrowingBomb
    };

    private TutorialState currState;

	// Use this for initialization
	void Start () {

        currState = TutorialState.Nothing;
        if (ConverstionHandler.instance)
        {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic) {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, GlobalVals.PlayerName + "! أنا نقيب فيصل. وسوف توجه لكم من خلال أساسيات الطيران.", 6, true, Interrupted);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French) {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, GlobalVals.PlayerName + "! Je suis capitaine Francis. Je vais vous guider dans les bases du vol.", 6, true, Interrupted);
            }
            else
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, GlobalVals.PlayerName + "! I am Capt. Fredrick. I will guide you through the basics of flying.", 6, true, Interrupted);
            }
        }
        Invoke("AssignPTPointer", 1);
        SPVGM.instance.CallOnMissionComplete = OnMissionComplete;

    }

    void AssignPTPointer() {

        PTPointer = FindObjectOfType<Pointer>().gameObject;
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
                case TutorialState.Nothing:
                    {
                        StateTime += Time.deltaTime;
                        if (StateTime > StateStayTime * 3)
                        {
                            currState = TutorialState.PlaneControlling;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "أولا تحويل الطائرة نحو اليمين.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Tourner d'abord l'avion vers la droite.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "First turn the plane towards right.", 10, true, Interrupted);
                                }
                            }
                            PlaneControlsTGO.SetActive(true);
                            Text currInstructionTxt = PlaneControlsTGO.transform.GetComponentInChildren<Text>();
                            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                            {
                                currInstructionTxt.text
                                    = ArabicSupport.ArabicFixer.Fix("إمالة الجهاز لتحويل الطائرة", true, true);
                                UILanguageHandler.instance.SetArabicLines(currInstructionTxt);
                            }
                            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                            {
                                currInstructionTxt.text
                                  = "Incline l'appareil pour tourner l'avion";
                            }
                            else
                            {
                                currInstructionTxt.text
                                  = "tilt the device to turn the plane";
                            }

                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.PlaneControlling:
                    {
                        TeachPlaneControls();
                        if (StateTime > StateStayTime)
                        {
                            currState = TutorialState.AirSpeedIntro;
                            Time.timeScale = 0;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "جيد! الآن، هنا يتم عرض سرعة الهواء الخاص بك.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "bien! Maintenant, voici votre vitesse d'air.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Good!..... \n Now, Here is displayed, your air speed in kts.", 10, true, Interrupted);
                                }
                            }
                            PlaneControlsTGO.SetActive(false);
                            ASpeedIntroTGO.SetActive(true);
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.AirSpeedIntro:
                    {
                        StateTime += Time.unscaledDeltaTime;
                        if (StateTime > StateStayTime * 3)
                        {
                            Time.timeScale = 1;
                            currState = TutorialState.ThrottleControl;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "الآن زيادة سرعة الطيران الخاص بك إلى أكثر من .360", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Augmentez maintenant votre vitesse à plus de 360.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Now, Increase your air speed to more than 360 kts", 10, true, Interrupted);
                                }
                            }
                            ThrottleControlsTGO.SetActive(true);

                            Text currInstructionTxt = ThrottleControlsTGO.transform.GetComponentInChildren<Text>();
                            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                            {
                                currInstructionTxt.text
                                    = ArabicSupport.ArabicFixer.Fix("حرك إصبعك على الجانب الأيسر من الشاشة لضبط دواسة الوقود", true, true);
                                UILanguageHandler.instance.SetArabicLines(currInstructionTxt);
                            }
                            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                            {
                                currInstructionTxt.text
                                  = "Déplacez votre doigt sur le côté gauche de l'écran pour ajuster l'accélérateur";
                            }
                            else
                            {
                                currInstructionTxt.text
                                  = "move your finger at the left side of the screen to adjust the throttle";
                            }

                            ASpeedIntroTGO.SetActive(false);
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.ThrottleControl:
                    {
                        TeachThrottleControls();
                        if (StateTime > StateStayTime)
                        {
                            Time.timeScale = 0;
                            currState = TutorialState.BLoadedTxtIntro;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "جيد! الآن هنا يتم عرض عدد من القنابل تحميلها.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "bien! Maintenant, voici le nombre de bombes chargées.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Good!...\n Now, here is displayed the number of bombs loaded.", 10, true, Interrupted);
                                }
                            }
                            ThrottleControlsTGO.SetActive(false);
                            BLoadedIntroTGO.SetActive(true);
                            StateTime = 0;
                        }
                        break;
                    }

                case TutorialState.BLoadedTxtIntro:
                    {
                        StateTime += Time.unscaledDeltaTime;
                        if (StateTime > StateStayTime * 3)
                        {
                            currState = TutorialState.BImpactPIntro;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "وهذا هو نقطة تأثير محسوبة من القنبلة.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "C'est le point d'impact calculé de la bombe.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "This is the computed impact point of the bomb.", 10, true, Interrupted);
                                }
                            }
                            BLoadedIntroTGO.SetActive(false);
                            BImpactPointerTGO.SetActive(true);
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.BImpactPIntro:
                    {
                        StateTime += Time.unscaledDeltaTime;
                        if (Time.timeScale == 0 && StateTime > StateStayTime * 2)
                            Time.timeScale = 1;
                        if (StateTime > StateStayTime * 3)
                        {
                            currState = TutorialState.PTrgtPointerIntro;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "وهذا السهم يشير نحو الهدف الأساسي.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Et cette flèche pointe vers la cible principale", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "And this arrow points towards the primary target.", 10, true, Interrupted);
                                }
                            }
                            BImpactPointerTGO.SetActive(false);
                            PTargetPointerTGO.SetActive(true);
                        //    PTPointer = PTargetPointerTGO.GetComponent<TutorialDetailPointer>().DetailAbout.gameObject;
                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.PTrgtPointerIntro:
                    {
                        StateTime += Time.unscaledDeltaTime;
                            if (PTPointer && !PTPointer.activeInHierarchy && PTargetPointerTGO.activeSelf)
                            {
                            Time.timeScale = 0;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "الصليب يحدد الهدف الأساسي.", 6, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "La croix identifie la cible principale.", 6, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "The Cross Identifies the primary target.", 6, true, Interrupted);
                                }
                            }
                                PTargetCrossTGO.SetActive(true);
                                PTargetPointerTGO.SetActive(false);
                                StateTime = StateStayTime;
                            }
                            else if (PTPointer.activeInHierarchy && !PTargetPointerTGO.activeSelf)
                            {
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "وهذا السهم يشير نحو الهدف الأساسي.", 10, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Et cette flèche pointe vers la cible principale.", 10, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "And this arrow points towards the primary target.", 10, true, Interrupted);
                                }
                            }
                                PTargetCrossTGO.SetActive(false);
                                PTargetPointerTGO.SetActive(true);
                                StateTime = StateStayTime;
                            }
                            if (StateTime > StateStayTime * 7)
                            {
                                currState = TutorialState.BombingSpeedTip;
                                Time.timeScale = 1;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "القصف هو أسهل في سرعة منخفضة. لذلك، تبطئ الآن.", 20, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Le bombardement est plus facile à basse vitesse. Alors, ralentis maintenant.", 20, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Bombing Is easier at low speed. So, Slow down now.", 20, true, Interrupted);
                                }
                            }
                                PTargetCrossTGO.SetActive(false);
                                PTargetPointerTGO.SetActive(false);
                                StateTime = 0;
                            }
                        
                        break;
                    }
                case TutorialState.BombingSpeedTip: {
                        SlowDownForBombing();
                        if (StateTime > StateStayTime) {
                            currState = TutorialState.ThrowingBomb;
                            if (ConverstionHandler.instance) {
                                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "الآن، اذهب وقصف الهدف المحدد.", 6, true, Interrupted);
                                }
                                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Maintenant, allez et bombardez la cible assignée.", 6, true, Interrupted);
                                }
                                else
                                {
                                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Now, Go and bomb the assigned target.", 6, true, Interrupted);
                                }
                            }
                            BombTGO.SetActive(true);

                            Text currInstructionTxt = BombTGO.transform.GetComponentInChildren<Text>();
                            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                            {
                                currInstructionTxt.text
                                    = ArabicSupport.ArabicFixer.Fix("اضغط على إصبعك على الجانب الأيسر من الشاشة لإسقاط قنبلة", true, true);
                                UILanguageHandler.instance.SetArabicLines(currInstructionTxt);
                            }
                            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                            {
                                currInstructionTxt.text
                                  = "Tapez votre doigt sur le côté gauche de l'écran pour déposer de la bombe";
                            }
                            else
                            {
                                currInstructionTxt.text
                                  = "tap at the left side of the screen to drop bomb";
                            }


                            StateTime = 0;
                        }
                        break;
                    }
                case TutorialState.ThrowingBomb:
                    {
                        if (SPVGM.instance.GetGameState == SPVGM.GameState.MissionComplete)
                        {
                            BombTGO.SetActive(false);
                        }
                        break;
                    }
            }

	}

    void TeachPlaneControls() {
        
        if (Mathf.Abs(Vector3.Dot(SPVPlayerControlloer.instance.PlayerPlane.up, Vector3.right)) > 0.8f)
        {
        //    print("Aho ni aho");
            StateTime += Time.deltaTime;
        }
        else {
            StateTime = 0;
        }

    }

    void TeachThrottleControls() {

        if (SPVPlayerControlloer.instance.PlayerPlane.GetComponent<Rigidbody>().velocity.magnitude > 60) {
            StateTime += Time.deltaTime;
        }

    }

    void SlowDownForBombing() {
        if (SPVPlayerControlloer.instance.PlayerPlane.GetComponent<Rigidbody>().velocity.magnitude < 45) {
            StateTime += Time.deltaTime;
        }
    }

    void Interrupted() {

        switch (currState)
        {
            case TutorialState.Nothing:
                {
                    StateTime = 0;
                    break;
                }
            case TutorialState.PlaneControlling:
                {
                    currState = TutorialState.Nothing;
                    PlaneControlsTGO.SetActive(false);
                    StateTime = 20;
                        break;
                }
            case TutorialState.ThrottleControl:
                {
                    currState = TutorialState.AirSpeedIntro;
                        ThrottleControlsTGO.SetActive(false);
                        StateTime = 20;
                    break;
                }
            case TutorialState.PTrgtPointerIntro:
                {
                    currState = TutorialState.BImpactPIntro;
                        PTargetPointerTGO.SetActive(false);
                        StateTime = 20;
                    break;
                }
  
        }

        interruptTime = 3;

    }

    public void OnMissionComplete() {
        BombTGO.SetActive(false);
        SPVGM.instance.Promote(1);
    }

}
