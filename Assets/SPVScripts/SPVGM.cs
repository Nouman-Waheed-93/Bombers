
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
public class SPVGM : MonoBehaviour
{

    private AudioMixerSnapshot MusicOnShot, MusicOffShot;
    private int TargetsLeft;
    private int FriendlyTargetsLeft;
    private int FriendlyTargetsDestroyed;
    private int FriendlyPlanesDestroyed;
    private int lastScore;

    public int FriendlyPlaneLossThreshold = 0;

    public int FrndlyLossThreshold = 0; // FriendlyTargets - Acceptable damage to friendly targets

    public AudioClip SuccessMusic, FailureMusic;

    private int MedalAwardedInd = -1, RankPromotedInd = 0;

    private GameObject PauseMenu;
    private GameObject ResumeBtn;
    
    private Text GTargetsPointsTxt, ATargetsPointsTxt, HlthPntTxt, StatusPntsTxt, MStatusTxt, FriendlyTrgtsDestroyedTxt, TotalTxt, RemarksTxt;
    private Image StatusColor;
    private int GTargetsDestroyedNumber, ATargetsDestroyedNumber;
    private string RemarksString;
    private Image MedalImage;
    public Sprite[] AllMedals;
    private PlaneHealth PlayerHealth;
    public delegate void MissionCompleteCallback();
    public MissionCompleteCallback CallOnMissionComplete;

    public GameObject ShareBtn;

	public static SPVGM instance;
    public enum GameState {
        MissionInProgress,
        MissionComplete,
        MissionFailed
    }

    private GameState currState = GameState.MissionInProgress;
    
    public GameState GetGameState {
        get {
            return currState;
        }
    }


    public void PauseGame() {
        string MName = "StopTime";
        if (IsInvoking(MName) || PauseMenu.activeSelf)
            return;
        if (currState == GameState.MissionComplete)
        {
            CalculateNAwardPoints(10);
            AwardMedal();
        }
        PauseMenu.SetActive(true);
        AdvertisementHandler.instance.ShowInterstitial();

        Invoke(MName, 0.6f);
    }

    void StopTime() {
        //  MyMainMixer.SetFloat("FXVol", -80);
        MusicOnShot.TransitionTo(1);
        SPVCamera.Shaking = false;
        Time.timeScale = 0;
    }

    public void UnPauseGame() {
        string MName = "DisablePauseMenu";
        if (IsInvoking(MName))
            return;
        //  MyMainMixer.SetFloat("FXVol", 0);
        MusicOffShot.TransitionTo(1);
        Time.timeScale = 1;
        PauseMenu.GetComponent<Animator>().SetTrigger("DisAppear");
        Invoke(MName, 0.6f);

    }

    void DisablePauseMenu() {

        PauseMenu.SetActive(false);

    }

    public void FailMission() {
        string MName = "ShowFailMenu";
        if (IsInvoking(MName))
            return;
        if (ConverstionHandler.instance) {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "فشلت المهمة", 1, true, null);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "mission échouée", 1, true, null);
            }
            else
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "Mission Failed!", 1, true, null);
            }
        }
        currState = GameState.MissionFailed;
        MusicHandler.instance.PlayFailureMusic();
        Invoke(MName, 2);
    }

     void ShowFailMenu() {
        string MName = "StopTime";
        if (IsInvoking(MName))
            return;
        PauseMenu.SetActive(true);
        StatusColor.color = Color.red;

        if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
        {
            MStatusTxt.text = ArabicSupport.ArabicFixer.Fix("فشلت المهمة", true, true);
        }
        else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
        {
            MStatusTxt.text = "mission échouée";
        }
        else
        {
            MStatusTxt.text = "Mission Failed";
        }

        HideResumeBtn();
        CalculateNAwardPoints(-10);
        Invoke(MName, 0.6f);
        AdvertisementHandler.instance.ShowInterstitial();

    }

    public void HideResumeBtn() {
        ResumeBtn.SetActive(false);
    }

    public void CompleteMission() {
        string Mname = "ShowSucceedMenu";
        if (IsInvoking(Mname))
            return;

        if (ConverstionHandler.instance) {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "مهمة مكتملة! جيد! ارجع الى القاعدة", 3, true, null);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "mission accomplie! bon travail! retourner à la base", 3, true, null);
            }
            else {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "Mission Complete! Good Work! Return to base", 3, true, null);
            }
        }
        currState = GameState.MissionComplete;
        int nextLevel = Mathf.Clamp(SceneManager.GetActiveScene().buildIndex + 1 - GlobalVals.CurrPlaneInd * 10, 1, 10);
        if (PlayerPrefs.GetInt(GlobalVals.UnlockedMissions + GlobalVals.CurrPlaneInd, 1) < nextLevel)
        {
            PlayerPrefs.SetInt(GlobalVals.UnlockedMissions + GlobalVals.CurrPlaneInd, nextLevel);
        }
        MusicHandler.instance.PlaySuccessMusic();
        Invoke(Mname, 2);
    }


    void ShowSucceedMenu() {
        string Mname = "StopTime";
        if (IsInvoking(Mname))
            return;
        PauseMenu.SetActive(true);
        StatusColor.color = Color.green;

        if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
        {
            MStatusTxt.text = ArabicSupport.ArabicFixer.Fix( "تمت المهمة", true, true);
        }
        else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
        {
            MStatusTxt.text = "Mission accomplie";
        }
        else
        {
            MStatusTxt.text = "Mission accomplished";
        }
//if (CallOnMissionComplete != null)
        if (CallOnMissionComplete != null)
            CallOnMissionComplete();
        AwardMedal();
        CalculateNAwardPoints(10);
        Invoke(Mname, 0.6f);
        AdvertisementHandler.instance.ShowInterstitial();
    }
    
    void CalculateNAwardPoints(int Points) {
        StatusPntsTxt.text = Points.ToString();

        GTargetsPointsTxt.text = GTargetsDestroyedNumber.ToString();
        ATargetsPointsTxt.text = ATargetsDestroyedNumber.ToString();
        FriendlyTrgtsDestroyedTxt.text = FriendlyTargetsDestroyed.ToString();
        int healthScore = (int)(PlayerHealth.currHealth * 0.1f);
        HlthPntTxt.text = healthScore.ToString();
        int TotalCoinsAwarded = GTargetsDestroyedNumber
            + ATargetsDestroyedNumber + Points - FriendlyTargetsDestroyed + healthScore;
        TotalTxt.text = TotalCoinsAwarded.ToString();
        TotalCoinsAwarded = Mathf.Abs(TotalCoinsAwarded - lastScore);
        lastScore = TotalCoinsAwarded + lastScore ;
        PlayerPrefs.SetInt(GlobalVals.Credits, PlayerPrefs.GetInt(GlobalVals.Credits, 0) + TotalCoinsAwarded);
        StatusPntsTxt.gameObject.SetActive(true);
        if (RemarksString != null)
        {
            RemarksTxt.text = RemarksString;
            RemarksTxt.transform.parent.gameObject.SetActive(true);
            if(GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                UILanguageHandler.instance.SetArabicLines(RemarksTxt);
        }
        

    }

    bool AwardMedal() {

        if (PlayerPrefs.GetInt(GlobalVals.Medal + 4, 0) == 0 && GTargetsDestroyedNumber > 10 && PlayerHealth.currHealth < 20)
        {
            AwardMedal(4);
            return true;
        }

        else if (PlayerPrefs.GetInt(GlobalVals.Medal + 3, 0) == 0 && ATargetsDestroyedNumber > 2 && GTargetsDestroyedNumber > 9)
        {
            AwardMedal(3);
            return true;
        }
        else if (PlayerPrefs.GetInt(GlobalVals.Medal + 2, 0) == 0 && ATargetsDestroyedNumber > 4)
        {
            AwardMedal(2);
            return true;
        }
        else
        if (PlayerPrefs.GetInt(GlobalVals.Medal + 1, 0) == 0 && GTargetsDestroyedNumber > 9 && PlayerHealth.currHealth > 99)
        {

            AwardMedal(1);
            return true;
        }
        else if (PlayerPrefs.GetInt(GlobalVals.Medal + 0, 0) == 0 && GTargetsDestroyedNumber > 9)
        {

            AwardMedal(0);
            return true;
        }
        return false;
    }

    void AwardMedal(int i) {
        MedalAwardedInd = i;
        if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic) {
            RemarksString = "لقد حصلت على " + GlobalVals.ArabicMedalNames[i];
        }
        else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French) {
            RemarksString = "Vous avez reçu la " + GlobalVals.FrenchMedalNames[i];
        }
        else {
            RemarksString = "You have been awarded " + GlobalVals.EnglishMedalNames[i];
        }      
        MedalImage.gameObject.SetActive(true);
        MedalImage.sprite = AllMedals[i];
        PlayerPrefs.SetInt(GlobalVals.Medal + i, 1);
    }

    public void Promote(int rank) {
        if (PlayerPrefs.GetInt(GlobalVals.PlayerRank, 0) < rank)
        {
            PlayerPrefs.SetInt(GlobalVals.PlayerRank, rank);
            RankPromotedInd = rank;
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                SPVGM.instance.GiveRemarks(ArabicSupport.ArabicFixer.Fix(" تهانينا، لقد تم ترقيت إلى " + GlobalVals.ArabicRanks[rank], true, true));
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                SPVGM.instance.GiveRemarks("Félicitations, vous avez été promu" + GlobalVals.FrenchRanks[rank]);
            }
            else
            {
                SPVGM.instance.GiveRemarks("Congrats! You have been promoted to " + GlobalVals.EnglishRanks[rank]);
            }
        }
    }

    public void GiveRemarks(string RemarksString) {
        this.RemarksString = RemarksString;
    }

    public void ShareAchievement() {

        if (MedalAwardedInd > -1)
        {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                FBHandler.instance.ShareAchievement("أنا منحت " + GlobalVals.ArabicMedalNames[MedalAwardedInd],
                     GlobalVals.ArabicMedalDetails[MedalAwardedInd].Replace("nnnn", GlobalVals.ArabicRanks[PlayerPrefs.GetInt(GlobalVals.PlayerRank, 0)] + " " +
                    GlobalVals.PlayerName),
                   GlobalVals.OnlineMedalImageLinks[MedalAwardedInd]);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                FBHandler.instance.ShareAchievement("Je reçois " + GlobalVals.FrenchMedalNames[MedalAwardedInd],
                     GlobalVals.FrenchMedalDetails[MedalAwardedInd].Replace("nnnn", GlobalVals.FrenchRanks[PlayerPrefs.GetInt(GlobalVals.PlayerRank, 0)] + " " +
                    GlobalVals.PlayerName),
                   GlobalVals.OnlineMedalImageLinks[MedalAwardedInd]);
            }
            else
            {
                FBHandler.instance.ShareAchievement("I am Awarded " + GlobalVals.EnglishMedalNames[MedalAwardedInd],
                    GlobalVals.EnglishMedalDetails[MedalAwardedInd].Replace("nnnn", GlobalVals.EnglishRanks[PlayerPrefs.GetInt(GlobalVals.PlayerRank, 0)] + " " +
                    GlobalVals.PlayerName),
                    GlobalVals.OnlineMedalImageLinks[MedalAwardedInd]);
            }

        }
        else if (RankPromotedInd > 0)
        {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                FBHandler.instance.ShareAchievement("لقد تم ترقيته إلى " + GlobalVals.ArabicRanks[RankPromotedInd],
                    "تمت ترقية " + GlobalVals.PlayerName + " إلى " + GlobalVals.ArabicRanks[RankPromotedInd],
                    GlobalVals.OnlineAvatarImageLinks[GlobalVals.AvatarInd]);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                FBHandler.instance.ShareAchievement("J'ai été promu " + GlobalVals.FrenchRanks[RankPromotedInd],
                     GlobalVals.PlayerName + " a été promu " + GlobalVals.FrenchRanks[RankPromotedInd],
                     GlobalVals.OnlineAvatarImageLinks[GlobalVals.AvatarInd]);
            }
            else
            {
                FBHandler.instance.ShareAchievement("I have been promoted to " + GlobalVals.EnglishRanks[RankPromotedInd],
                GlobalVals.PlayerName + " has been promoted to " + GlobalVals.EnglishRanks[RankPromotedInd],
                GlobalVals.OnlineAvatarImageLinks[GlobalVals.AvatarInd]);
            }

        }
    }

    public void RestartMission() {
        string MName = "InvoRestart";
        if (IsInvoking(MName))
            return;
        Time.timeScale = 1;
        Fader.instance.FadeOut();
        Invoke(MName, 1);

    }

    void InvoRestart() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void GotoMainMenu() {
        string MName = "InvoMainMenu";
        if (IsInvoking(MName))
            return;

        Time.timeScale = 1;
        Fader.instance.FadeOut();
        Invoke(MName, 1);

    }

    void InvoMainMenu() {
        SceneManager.LoadSceneAsync(0);
    }

    public void NextMission() {

        if (currState == GameState.MissionComplete)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // Use this for initialization
    void Awake ()
	{
        instance = this;

        AudioMixer MyMainMixer = Resources.Load <AudioMixer> ("MainMixer");
        MusicOnShot = MyMainMixer.FindSnapshot("MusicOnly");
        MusicOffShot = MyMainMixer.FindSnapshot("MusicOff");
        PauseMenu = GameObject.FindGameObjectWithTag("PauseM");
        MedalImage = GameObject.FindGameObjectWithTag("MedalImg").GetComponent<Image>();
        StatusColor = GameObject.FindGameObjectWithTag("StatusColor").GetComponent<Image>();
        ResumeBtn = GameObject.FindGameObjectWithTag("ResumeBtn");
        ShareBtn = GameObject.FindGameObjectWithTag("ShareBtn");
        Fader.instance.FadeIn();
        MusicHandler.instance.StopMusic();
        InitializePoints();
        MedalImage.gameObject.SetActive(false);
        DisablePauseMenu();
    }

    void Start() {
        PlayerHealth = CameraPositioner.CameraInstance.GetComponentInParent<PlaneHealth>();
        AdvertisementHandler.instance.RequestInterstitial();
    }

    void InitializePoints() {
        GTargetsPointsTxt = GameObject.FindGameObjectWithTag("GTrgtsDestroyed").GetComponent<Text>();
        ATargetsPointsTxt = GameObject.FindGameObjectWithTag("AtrgtsDestroyed").GetComponent<Text>();
        HlthPntTxt = GameObject.FindGameObjectWithTag("HlthPnts").GetComponent<Text>();
        MStatusTxt = GameObject.FindGameObjectWithTag("MissionStatusTxt").GetComponent<Text>();
        FriendlyTrgtsDestroyedTxt = GameObject.FindGameObjectWithTag("FtrgtsDestroyed").GetComponent<Text>();
        TotalTxt = GameObject.FindGameObjectWithTag("TotalPnts").GetComponent<Text>();
        RemarksTxt = GameObject.FindGameObjectWithTag("RemarksTxt").GetComponent<Text>();
        StatusPntsTxt = GameObject.FindGameObjectWithTag("StatusPoints").GetComponent<Text>();
        RemarksTxt.transform.parent.gameObject.SetActive(false);
        StatusPntsTxt.gameObject.SetActive(false);
      }

    public void AddTargets(int amount) {
        TargetsLeft += amount;
    }

    public void RemoveTarget() {
        TargetsLeft--;
  }

    public void AirTargetDestroyed() {
        ATargetsDestroyedNumber++;
        PilotComments.instance.AirTargetHitComment();
    }

    public void GroundTargetDestroyed() {
        GTargetsDestroyedNumber++;
        PilotComments.instance.GroundTargetHitComment();
    }

    public void AddFriendlyTargets(int amount) {
        FriendlyTargetsLeft += amount;
    }

    public void FriendlyPlaneDestroyed() {
        FriendlyPlanesDestroyed++;
        if (PlayerHealth == null || PlayerHealth.currHealth < 1)
        {
            if (ConverstionHandler.instance) {
                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic) {
                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, "دمرت الطائرة بلدي!", 3, false, null);
                } 
                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                {
                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, "Mayday! Mayday!", 3, false, null);
                }
               else
                {
                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.PlayerC, "Mayday! Mayday!", 3, false, null);
                }
            }
                
            Invoke("FailMission", 2);
        }
        else
        {
            if (ConverstionHandler.instance)
            {
                if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                {
                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "دمرت الطائرة بلدي!", 3, false, null);
                }
                else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                {
                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Mayday! Mayday!", 3, false, null);
                }
                else
                {
                    ConverstionHandler.instance.Speak(ConverstionHandler.Character.InstructorC, "Mayday! Mayday!", 3, false, null);
                }
            }
            if (FriendlyPlanesDestroyed > FriendlyPlaneLossThreshold)
            {
                Invoke("FailMission", 2);
            }
        }
    }

    public void FriendlyTargetDestroyed() {
        FriendlyTargetsDestroyed++;
        FriendlyTargetsLeft--;
        if (ConverstionHandler.instance) {
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic) {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.GroundLeaderC, "نحن نتخذ الضرر!", 2, false, null);
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.GroundLeaderC, "Nous prenons des dégâts!", 2, false, null);
            }
            else 
            {
                ConverstionHandler.instance.Speak(ConverstionHandler.Character.GroundLeaderC, "We are taking damage!", 2, false, null);
            }


        }

    }

	// Update is called once per frame
	void Update ()
	{
        
        if (TargetsLeft < 1 && currState != GameState.MissionComplete) {
            CompleteMission();
        }

        if (FriendlyTargetsDestroyed > FrndlyLossThreshold && currState != GameState.MissionFailed)
        {
            FailMission();
        }

	}


}
