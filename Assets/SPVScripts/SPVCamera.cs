using UnityEngine;
using System.Collections;

public class SPVCamera : MonoBehaviour
{
    
	public float XBoundMin = -50;
	public float XBoundMax = 50;
	public float YBoundMin = -50;
	public float YBoundMax = 50;
    public float TimeAllowedOutside = 5;
    public static bool Shaking = false;

	private Transform playerPlane;
    private float TimeSinceOut;
    private float WarningTime;
    private bool NotifiedToGoMain;
	// Use this for initialization
	void Start ()
	{
        playerPlane = CameraPositioner.CameraInstance.transform;
	}

    public static void Shake() {

#if UNITY_ANDROID
        Handheld.Vibrate();
#endif
        Shaking = true;

    }

	// Update is called once per frame
	void Update ()
	{
        if (!playerPlane)
            return;
        Vector3 TempPos = transform.position;
		TempPos.x = Mathf.Clamp (Mathf.Lerp (TempPos.x, playerPlane.position.x + (Shaking?Random.Range(-20, 20):0), 0.1f), XBoundMin, XBoundMax);
        TempPos.y = Mathf.Clamp(Mathf.Lerp(TempPos.y, playerPlane.position.y + (Shaking ? Random.Range(-20, 20) : 0), 0.1f), YBoundMin, YBoundMax);
		transform.position = TempPos;

        if ((playerPlane.position.x > XBoundMax || playerPlane.position.x < XBoundMin ||
            playerPlane.position.y > YBoundMax || playerPlane.position.y < YBoundMin ) && Time.timeSinceLevelLoad > 10)
        {
            WarningTime -= Time.deltaTime;
            TimeSinceOut += Time.deltaTime;
            if (TimeSinceOut > TimeAllowedOutside || playerPlane.position.x > XBoundMax + 200 ||
                playerPlane.position.x < XBoundMin - 200 || playerPlane.position.y > YBoundMax + 200 ||
                playerPlane.position.y < YBoundMin - 200)
            {
                if (SPVGM.instance.GetGameState == SPVGM.GameState.MissionComplete && !NotifiedToGoMain)
                {
                    NotifiedToGoMain = true;
                    SPVGM.instance.HideResumeBtn();
                    SPVGM.instance.PauseGame();
                }
                else
                    SPVGM.instance.FailMission();

            }
            else if (WarningTime < 0)
            {
                if (ConverstionHandler.instance) {
                    if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
                    {
                        ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "العودة إلى منطقة البعثة.", 2f, true, null);
                    }
                    else if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.French)
                    {
                        ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "Retourner dans la zone de la mission.", 2f, true, null);
                    }
                    else
                    {
                        ConverstionHandler.instance.Speak(ConverstionHandler.Character.TowerC, "Return Back To the mission area.", 2f, true, null);
                    }
                }
                WarningTime = 3;
            }
        }
        else {
            TimeSinceOut = 0;
        }

        if (Shaking)
            Invoke("StopShaking", 1);

	}

    void StopShaking() {
        Shaking = false;
    }
}
