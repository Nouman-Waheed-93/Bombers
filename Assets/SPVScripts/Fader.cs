using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Fader : MonoBehaviour {

    public static Fader instance;
    
    private Animator myAnim;
    private int FadeInId, FadeOutId;
    private float LastFadeTime;
	// Use this for initialization
	void Start () {
        if (instance)
        {
            Destroy(gameObject);
            instance.FadeIn();
            return;
        }
        instance = this;
        myAnim = GetComponent<Animator>();
        FadeInId = Animator.StringToHash("FadeIn");
        FadeOutId = Animator.StringToHash("FadeOut");
        DontDestroyOnLoad(transform.root.gameObject);
        FadeIn();
	}
    

    public void FadeIn() {
        if (Time.time - LastFadeTime < 1)
            return;
        LastFadeTime = Time.time;
        myAnim.SetTrigger(FadeInId);
    }

    public void FadeOut() {
        if (Time.time - LastFadeTime < 1)
            return;
        LastFadeTime = Time.time;
        myAnim.SetTrigger(FadeOutId);
    }
    
}
