using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBHndlr : MonoBehaviour {

    public void Resume() {
        SPVGM.instance.UnPauseGame();
    }

    public void Restart() {
        SPVGM.instance.RestartMission();
    }

    public void Quit() {
        SPVGM.instance.GotoMainMenu();
    }

    public void Share() {
        SPVGM.instance.ShareAchievement();
    }

}
