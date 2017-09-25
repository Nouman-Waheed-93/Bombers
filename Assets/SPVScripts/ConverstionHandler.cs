using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConverstionHandler : MonoBehaviour {
    
    public Sprite PlayerFoto, Instructor, TowerControl, GroundLeader;
    public static ConverstionHandler instance;
    public Image speakerPhoto;
    Text words;
    private float displayTime;
    public delegate void NotifyInterrrupt();
    private NotifyInterrrupt interruptMethod;
    private Animator myanim;
    private int importantId;
    private int commentId;
    public enum Character {
        PlayerC,
        InstructorC,
        TowerC,
        GroundLeaderC
    }
	// Use this for initialization
	void Awake () {
        importantId = Animator.StringToHash("ImportntCnv");
        commentId = Animator.StringToHash("Comment");
        myanim = GetComponent<Animator>();
        instance = this;
        words = GetComponentInChildren<Text>();
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        displayTime -= Time.deltaTime;
        if (displayTime < 0 && gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
	}

    IEnumerator SetAnimTrigger(bool important) {
        yield return new WaitForEndOfFrame();
        if (important)
            myanim.SetTrigger(importantId);
        else myanim.SetTrigger(commentId);

    }

    public void Speak(Character speaker, string words, float displayTime, bool important, NotifyInterrrupt interruptMethod) {
        if (gameObject.activeSelf && this.interruptMethod != null && this.interruptMethod != interruptMethod)
            this.interruptMethod();
        this.interruptMethod = interruptMethod;

        gameObject.SetActive(false);
        switch (speaker) {
            case Character.PlayerC: {
                    speakerPhoto.sprite = PlayerFoto;
                    break;
                }
            case Character.InstructorC: {
                    speakerPhoto.sprite = Instructor;
                    break;
                }
            case Character.TowerC: {
                    speakerPhoto.sprite = TowerControl;
                    break;
                }
            case Character.GroundLeaderC: {
                    speakerPhoto.sprite = GroundLeader;
                    break;
                }

        }

        gameObject.SetActive(true);

        if (GlobalVals.SelectedLanguage == GlobalVals.SupportedLanguages.Arabic)
        {
            this.words.text = ArabicSupport.ArabicFixer.Fix(words, true, true);
            UILanguageHandler.instance.SetArabicLines(this.words);
        }
        else
            this.words.text = words;
        this.displayTime = displayTime;
        
        StartCoroutine("SetAnimTrigger", important);

    }

}
