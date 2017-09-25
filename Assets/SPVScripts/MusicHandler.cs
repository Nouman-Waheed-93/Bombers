using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MusicHandler : MonoBehaviour {

    public static MusicHandler instance;
    public AudioClip MainThemeMusic, SuccessMusic, FailureMusic;
    private AudioSource musicSound;
    private float TargetVolume;
    private AudioClip currClip;
    private bool curClipPlayed;
    private bool stopMusicb;
    public AudioMixerSnapshot MusicOn, MusicOff;
	// Use this for initialization
	void Awake () {
        if (!instance)
            instance = this;
        else
            Destroy(this);
        musicSound = GetComponent<AudioSource>();
        instance.PlayMainTheme();
	}

    void Start() {
        MusicOn.TransitionTo(1);
    }

    public void PlayMainTheme() {
        if(musicSound.isPlaying)
            TargetVolume = 0;
        currClip = MainThemeMusic;
        curClipPlayed = false;
        stopMusicb = false;
    }

    public void PlaySuccessMusic() {
        if (musicSound.isPlaying)
            TargetVolume = 0;
        currClip = SuccessMusic;
        curClipPlayed = false;
        stopMusicb = false;
    }

    public void PlayFailureMusic() {
        if (musicSound.isPlaying)
            TargetVolume = 0;
        currClip = FailureMusic;
        curClipPlayed = false;
        stopMusicb = false;
    }

    public void StopMusic() {
        TargetVolume = 0;
        stopMusicb = true;
        curClipPlayed = false;
        MusicOff.TransitionTo(1);
    }

	// Update is called once per frame
	void Update () {
        if (!curClipPlayed)
        {
            musicSound.volume = Mathf.Lerp(musicSound.volume, TargetVolume, Time.deltaTime);
            if (musicSound.volume < 0.2f)
            {
                if (!stopMusicb)
                {
                    musicSound.clip = currClip;
                    TargetVolume = 1;
                }
                else
                {
                    musicSound.Stop();
                    curClipPlayed = true;
                }
            }
            else if (musicSound.volume > 0.8f && TargetVolume == 1) {
                musicSound.volume = 1;
                musicSound.Play();
                curClipPlayed = true;
            }
           
        }
    }
}
