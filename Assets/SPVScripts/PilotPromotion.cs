using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotPromotion : MonoBehaviour {

    public int PromoteRank = 0;

	// Use this for initialization
	void Start () {
        SPVGM.instance.CallOnMissionComplete = OnMissionComplete;
    }

    public void OnMissionComplete()
    {

        SPVGM.instance.Promote(PromoteRank);
    }

}
