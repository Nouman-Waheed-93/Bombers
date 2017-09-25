using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SPVPlayerControlloer : MonoBehaviour
{
	private float DTilt = 0;
	private SPVPlaneController m_PlayerPlane;
    public Transform PlayerPlane {
        get {
            return m_PlayerPlane.transform;
        }
    }
	private SPVWeaponController m_WeaponCntrlr;
    private bool fireToggle;

    private Slider ThrUI;

    public static SPVPlayerControlloer instance;

    private Vector2[] touchBeginPos;
    private Vector2 MouseDelta;
    private float TouchTime;

	void Start ()
	{
        ThrUI = GameObject.FindGameObjectWithTag("ThrottleBar").GetComponent<Slider>();
        touchBeginPos = new Vector2[2];
        instance = this;
        m_PlayerPlane = CameraPositioner.CameraInstance.GetComponentInParent<SPVPlaneController>();
		m_WeaponCntrlr = m_PlayerPlane.GetComponentInChildren<SPVWeaponController> ();

	}

	// Update is called once per frame
	void Update ()
	{
        
        if (Input.GetKeyDown(KeyCode.Escape))
            SPVGM.instance.PauseGame();

        if (Time.timeSinceLevelLoad < 2 || Time.timeScale < 0.5f || m_PlayerPlane == null || m_WeaponCntrlr == null)
            return;

#if UNITY_EDITOR || UNITY_STANDALONE

        #region
        //GestureControls
        if (Input.GetMouseButtonDown(0))
        {
            TouchTime = 0;
            touchBeginPos[0] = Input.mousePosition;
            MouseDelta = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) {
            if ((touchBeginPos[0].x > Screen.width * 0.5f))
            {
                if ((Input.mousePosition.y - touchBeginPos[0].y > 20))
                {

                    FireMissile();

                }

            }
            else {

                if (Vector2.Distance(touchBeginPos[0], Input.mousePosition) < 10)
                    DropBomb();

            }
        }

        if (Input.GetMouseButton(0)) {

            TouchTime += Time.deltaTime;
            if (Input.mousePosition.x < Screen.width * 0.5f)
            {

                if (Input.mousePosition.y - MouseDelta.y > 10)
                {
                    ChangeThrottle(0.01f * (Input.mousePosition.y - MouseDelta.y - 10));
                    MouseDelta = Input.mousePosition;
                }
                else if (MouseDelta.y - Input.mousePosition.y > 10)
                {
                    ChangeThrottle(0.01f * (Input.mousePosition.y - MouseDelta.y + 10));
                    MouseDelta = Input.mousePosition;
                }

            }
            else {

                if (Vector2.Distance(touchBeginPos[0], Input.mousePosition ) < 10 && TouchTime > 0.2f) {
                    m_WeaponCntrlr.FireCannon();
                }

            }

        }


        #endregion
        
        //MobileTouch
        

        if (Mathf.Abs(Input.GetAxis ("Horizontal")) > 0) {
            DTilt = Input.GetAxis("Horizontal");
		} else {
			DTilt = 0;
		}

        ChangeThrottle(Input.GetAxis("Vertical"));
      
		if (Input.GetButton ("Fire"))
			m_WeaponCntrlr.FireCannon ();
		if (Input.GetButtonDown ("Bomb"))
		    DropBomb ();
        if (Input.GetButtonDown("Missile"))
            FireMissile();
       if (Input.GetButtonDown("AttackMode"))
            m_WeaponCntrlr.SwitchAttackMode();
        //To Here
#elif UNITY_ANDROID

        #region
        //GestureControls
        for (int i = 0; i < Input.touchCount; i++)
        {

            if (i > 1)
                break;

            Touch currTouch = Input.GetTouch(i);

            if (currTouch.phase == TouchPhase.Began)
            {
                TouchTime = 0;
                touchBeginPos[currTouch.fingerId] = currTouch.position;
            }
            else if (currTouch.phase == TouchPhase.Ended)
            {
                if ((touchBeginPos[currTouch.fingerId].x > Screen.width * 0.5f))
                {
                    float yDiff = currTouch.position.y - touchBeginPos[currTouch.fingerId].y;
                 //   if ((yDiff > 50))
                   if(yDiff > Screen.dpi * 0.3f)
                    {

                        FireMissile();

                    }
                    //else if((yDiff < -50))
                    else if(yDiff < -Screen.dpi * 0.3f)
                    {
                        m_WeaponCntrlr.SwitchAttackMode();
                    }

                }
                else
                {

           //         if (Vector2.Distance(touchBeginPos[currTouch.fingerId], currTouch.position) < 10)
                      if(Vector2.Distance(touchBeginPos[currTouch.fingerId], currTouch.position) < Screen.dpi * 0.1f)
                        DropBomb();

                }
            }

            if (currTouch.phase != TouchPhase.Began || currTouch.phase != TouchPhase.Ended)
            {
                
                TouchTime += Time.deltaTime;
                if (currTouch.position.x < Screen.width * 0.5f)
                {

                    if (Mathf.Abs(currTouch.position.y - touchBeginPos[currTouch.fingerId].y) > Screen.dpi * 0.1f)
                    {
                        ChangeThrottle(10 * (currTouch.deltaPosition.y)/ Screen.dpi);
                    }
                    
                }
                else
                {

                    if (Vector2.Distance(touchBeginPos[currTouch.fingerId], currTouch.position) < Screen.dpi * 0.2f && TouchTime > 0.2f)
                    {
                        m_WeaponCntrlr.FireCannon();
                    }

                }

            }
        }


        #endregion

		DTilt = Mathf.Clamp(-Input.acceleration.x * 2,-1,1);

#endif

        if (fireToggle)
            m_WeaponCntrlr.FireCannon();
     //   balanceUI.value = DTilt;

    }

    void ChangeThrottle(float change) {
        ThrUI.value = Mathf.Clamp(ThrUI.value + change, ThrUI.minValue, ThrUI.maxValue);
        m_PlayerPlane.SetSpeed(Mathf.Clamp(m_PlayerPlane.GetThrottleAge() + change, 0, 1));
    }

    public void SetThrottle(float throttle) {
        m_PlayerPlane.SetSpeed(throttle);
    }

    public void FireMissile() {
        m_WeaponCntrlr.FireMissile();
    }

    public void DropBomb() {
        m_WeaponCntrlr.DropBomb();
    }

    public void ToggleFiring(bool toggle) {
        fireToggle = toggle;
    }

	void FixedUpdate ()
	{
        if (m_PlayerPlane == null)
        {
            return;
        }

		if (DTilt > 0.2f || DTilt < -0.2f)
			m_PlayerPlane.Turn (DTilt);
		else
			m_PlayerPlane.Turn (0);
	}

}
