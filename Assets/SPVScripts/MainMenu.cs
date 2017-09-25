using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {

    public Sprite[] PilotAvatars;
    public Image UserDAvatarImage;
    public Text UserDNickText;
    
    public GameObject[] AcquiredMadelImages;
    
    public GameObject UserDetailScreen, LanguageScreen, MainScreen, AirplaneScreen, MissionScreen;

    public Image MainAvatarImage;
    public Text NickText;
    public Text Rank;

    public Text LanguageTxt;

    public Text MoneyText;

    public MainMenuCamera theCam;
    public GameObject[] PlaneGOs, PlaneDetails;
    public GameObject UnlockButton, SelectPlaneButton;
    public Text PriceTag;
    private int[] PUIndex;
    public GameObject ErrorBuyingMessage, WatchAdProposal;
    public GameObject WatchAdButton;

    private Coroutine LevelLoadingThread;

    public GameObject[] MissionBtns;

    // Use this for initialization
	void Start () {
        //Give Credits for testing

        //for (int i = 0; i < 5; i++)
        //{
        //    PlayerPrefs.SetInt(GlobalVals.UnlockedMissions + i, 10);
        //}
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        InitializeLanguageSystem();
        SetPrices();
        InitializePlaneSystem();
        DisplayUserDetails();
        InitializeCredit();
        InitializeMedals();
        Time.timeScale = 1;
    }

    void Update() {
        if (!WatchAdButton.activeSelf && UnityEngine.Advertisements.Advertisement.IsReady())
            WatchAdButton.SetActive(true);
    }

    public void FaderInitialized() {
        Fader.instance.FadeIn();
    }

    public void MusicHandlerInitialized() {

        MusicHandler.instance.PlayMainTheme();

    }

    void SetPrices() {
            for (int i = 0; i < PlaneGOs.Length; i++) {
                PlayerPrefs.SetInt(GlobalVals.AircraftPrice + i, i * 300);
            }
    }

    void InitializeMedals() {

        for (int i = 0; i < AcquiredMadelImages.Length; i++) {
            if (PlayerPrefs.GetInt(GlobalVals.Medal + i, 0) == 1) {
                AcquiredMadelImages[i].gameObject.SetActive(true);
               
            }
        }
    }
    
    void InitializePlaneSystem() {
        theCam.LATarget = PlaneGOs[0].transform;
        PUIndex = new int[PlaneGOs.Length];
        PUIndex[0] = 1;
        for (int i = 1; i < PlaneGOs.Length; i++) {
            PUIndex[i] = PlayerPrefs.GetInt(GlobalVals.PlaneUnlocked + i, 0);
        }
    }

    public void DisplayLanguageSettingScreen() {
        MainScreen.SetActive(false);
        AirplaneScreen.SetActive(false);
        MissionScreen.SetActive(false);
        UserDetailScreen.SetActive(false);
        LanguageScreen.SetActive(true);
    }

    public void DisplayUserDetailsScreen() {
        MainScreen.SetActive(false);
        AirplaneScreen.SetActive(false);
        MissionScreen.SetActive(false);
        LanguageScreen.SetActive(false);
        UserDetailScreen.SetActive(true);
    }

    public void SaveDetails() {
        GlobalVals.PlayerName = UserDNickText.text;
        PlayerPrefs.SetString("Nick", GlobalVals.PlayerName);
        PlayerPrefs.SetInt("Avatar", GlobalVals.AvatarInd);
        DisplayMainMenu();
        DisplayUserDetails();
    }

    void DisplayUserDetails() {
        MainAvatarImage.sprite = PilotAvatars[GlobalVals.AvatarInd];
        if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
        {
            NickText.text = ArabicSupport.ArabicFixer.Fix(GlobalVals.PlayerName, true, true);
            Rank.text = ArabicSupport.ArabicFixer.Fix(GlobalVals.ArabicRanks[PlayerPrefs.GetInt(GlobalVals.PlayerRank)], true, true);
        }
        else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
        {
            NickText.text = GlobalVals.PlayerName;
            Rank.text =
                GlobalVals.FrenchRanks
                [PlayerPrefs.GetInt(GlobalVals.PlayerRank)];
        }
        else
        {
            NickText.text = GlobalVals.PlayerName;
            Rank.text = GlobalVals.EnglishRanks[PlayerPrefs.GetInt(GlobalVals.PlayerRank)];
        }
   
    }

    public void Exit() {
        Application.Quit();
    }

    public void DisplayPlaneSelectionScreen() {
        MainScreen.SetActive(false);
        MissionScreen.SetActive(false);
        UserDetailScreen.SetActive(false);
        AirplaneScreen.SetActive(true);
        CyclePlane(0);
    }

    public void DisplayMainMenu() {
        MissionScreen.SetActive(false);
        UserDetailScreen.SetActive(false);
        AirplaneScreen.SetActive(false);
        MainScreen.SetActive(true);
    }

    public void DisplayMissionScreen() {
        int unlockedMissions = Mathf.Clamp(PlayerPrefs.GetInt(GlobalVals.UnlockedMissions + GlobalVals.CurrPlaneInd, 1), 1, 10);
        for (int i = 0; i < 10; i++) {
            if (i < unlockedMissions)
                MissionBtns[i].SetActive(true);
            else
                MissionBtns[i].SetActive(false);
        }
        MainScreen.SetActive(false);
        UserDetailScreen.SetActive(false);
        AirplaneScreen.SetActive(false);
        MissionScreen.SetActive(true);
    }

    void InitializeCredit() {
        MoneyText.text = PlayerPrefs.GetInt(GlobalVals.Credits, 0).ToString();
    }

    public void CyclePlane(int i) {
        PlaneDetails[GlobalVals.CurrPlaneInd].SetActive(false);
        GlobalVals.CurrPlaneInd = Mathf.Clamp(GlobalVals.CurrPlaneInd + i, 0, PlaneGOs.Length - 1);
        PlaneDetails[GlobalVals.CurrPlaneInd].SetActive(true);
        theCam.LATarget = PlaneGOs[GlobalVals.CurrPlaneInd].transform;
        if (PUIndex[GlobalVals.CurrPlaneInd] == 1)
        {
            SelectPlaneButton.SetActive(true);
            UnlockButton.SetActive(false);

        }
        else {
            SelectPlaneButton.SetActive(false);
            UnlockButton.SetActive(true);
            if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
            {
                PriceTag.text = ArabicSupport.ArabicFixer.Fix(PlayerPrefs.GetInt(GlobalVals.AircraftPrice + GlobalVals.CurrPlaneInd, 0).ToString(), true, true);
                PriceTag.fontSize = 20;
            }
            else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
            {
                PriceTag.fontSize = 20;
                PriceTag.text = PlayerPrefs.GetInt(GlobalVals.AircraftPrice + GlobalVals.CurrPlaneInd, 0).ToString();
            }
            else
            {
                PriceTag.text = PlayerPrefs.GetInt(GlobalVals.AircraftPrice + GlobalVals.CurrPlaneInd, 0).ToString();
                PriceTag.fontSize = 14;
            }
        }
    }

    public void WatchRewardedVideo() {
        AdvertisementHandler.instance.ShowRewardedAd(Reward);
    }

    public void Reward() {

        PlayerPrefs.SetInt(GlobalVals.Credits, PlayerPrefs.GetInt(GlobalVals.Credits, 0) + 50);
        InitializeCredit();
        WatchAdButton.SetActive(false);
        HideErrorMessage();

    }

    public void UnlockPlane() {
        if (PUIndex[GlobalVals.CurrPlaneInd] == 0) {
            if (PlayerPrefs.GetInt(GlobalVals.Credits, 0) >= PlayerPrefs.GetInt(GlobalVals.AircraftPrice + GlobalVals.CurrPlaneInd, 0))
            {
                PUIndex[GlobalVals.CurrPlaneInd] = 1;
                PlayerPrefs.SetInt(GlobalVals.PlaneUnlocked + GlobalVals.CurrPlaneInd, 1);
                PlayerPrefs.SetInt(GlobalVals.Credits,PlayerPrefs.GetInt(GlobalVals.Credits, 0) - PlayerPrefs.GetInt(GlobalVals.AircraftPrice + GlobalVals.CurrPlaneInd, 0));
                InitializeCredit();
                CyclePlane(0);
            }
            else {
                if (UnityEngine.Advertisements.Advertisement.IsReady())
                    WatchAdProposal.SetActive(true);
                else
                    ErrorBuyingMessage.SetActive(true);
                
            }
        }
    }

    public void HideErrorMessage() {
        ErrorBuyingMessage.SetActive(false);
        WatchAdProposal.SetActive(false);
    }

    void InitializeLanguageSystem() {
        if (PlayerPrefs.HasKey("Language"))
        {
            if (PlayerPrefs.GetString("Language") == "Arabic")
            {
                GlobalVals.SelectedLanguage = GlobalVals.SupportedLanguages.Arabic;
                LanguageTxt.text = ArabicSupport.ArabicFixer.Fix( "العربية", true, true);

            }
            else if (PlayerPrefs.GetString("Language") == "English")
            {
                GlobalVals.SelectedLanguage = GlobalVals.SupportedLanguages.English;
                LanguageTxt.text = "English";
            }
            else {
                GlobalVals.SelectedLanguage = GlobalVals.SupportedLanguages.French;
                LanguageTxt.text = "français";
            }
            UILanguageHandler.instance.HandleText();
            HideErrorMessage();
            AirplaneScreen.SetActive(false);
            InitializeAvatarSystem();
        }
        else
        {
            DisplayLanguageSettingScreen();
        }
    }

    void InitializeAvatarSystem() {
        if (PlayerPrefs.HasKey("Nick"))
        {
            GlobalVals.PlayerName = PlayerPrefs.GetString("Nick", "Nick");
            GlobalVals.AvatarInd = PlayerPrefs.GetInt("Avatar", 0);
            UserDNickText.text = GlobalVals.PlayerName;
            CycleAvatar(0);
        }
        else {
            DisplayUserDetailsScreen();
        }
    }


    public void SetEnglish()
    {
        PlayerPrefs.SetString("Language", "English");
        GlobalVals.SelectedLanguage = GlobalVals.SupportedLanguages.English;
        MainScreen.SetActive(true);
        LanguageScreen.SetActive(false);
        UILanguageHandler.instance.HandleText();
        AirplaneScreen.SetActive(false);
        LanguageTxt.text = "English";
        HideErrorMessage();
        InitializeAvatarSystem();
    }

    public void SetArabic()
    {
        PlayerPrefs.SetString("Language", "Arabic");
        GlobalVals.SelectedLanguage = GlobalVals.SupportedLanguages.Arabic;
        MainScreen.SetActive(true);
        AirplaneScreen.SetActive(true);
        LanguageScreen.SetActive(false);
        UILanguageHandler.instance.HandleText();
        AirplaneScreen.SetActive(false);
        LanguageTxt.text = ArabicSupport.ArabicFixer.Fix("العربية", true, true);
        HideErrorMessage();
        InitializeAvatarSystem();
    }

    public void SetFrench()
    {
        PlayerPrefs.SetString("Language", "French");
        GlobalVals.SelectedLanguage = GlobalVals.SupportedLanguages.French;
        MainScreen.SetActive(true);
        LanguageScreen.SetActive(false);
        UILanguageHandler.instance.HandleText();
        AirplaneScreen.SetActive(false);
        LanguageTxt.text = "français";
        HideErrorMessage();
        InitializeAvatarSystem();
    }

    public void CycleAvatar(int i) {

        GlobalVals.AvatarInd = Mathf.Clamp(GlobalVals.AvatarInd + i, 0, PilotAvatars.Length - 1);
        UserDAvatarImage.sprite = PilotAvatars[GlobalVals.AvatarInd];
        
    }
    
    public void StartMission(int levelInd) {

        if (LevelLoadingThread != null)
        {
            print("Bohta shokha na ho");
            return;
        }
        Fader.instance.FadeOut();
        LevelLoadingThread = StartCoroutine("WaitFaderNLoadScene", levelInd);
        
    }

    IEnumerator WaitFaderNLoadScene(int levelInd) {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync((GlobalVals.CurrPlaneInd * 10) + levelInd);
    }

}
