using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RWR : MonoBehaviour {

    public List<Transform> threats;
    public List<GameObject> threatHUDLines;
    private Transform HUDGO;
    private GameObject ThreatLine;
   
    public delegate void ThreatCallback();
    public ThreatCallback CallOnThreat;
    public ThreatCallback CallOnDethreat;

    void Start() {
        HUDGO = GameObject.FindGameObjectWithTag("HUDCanvas").transform;
        ThreatLine = Resources.Load<GameObject>("ThreatLine");
    }

    public void SetThreat(Transform threat)
    {
        if (!threats.Contains(threat))
        {
            threats.Add(threat);
            threatHUDLines.Add(Instantiate<GameObject>(ThreatLine,HUDGO));
            GameObject thisLine = threatHUDLines[threats.IndexOf(threat)];
            thisLine.transform.localScale = Vector3.one;
            if(CallOnThreat != null)
                CallOnThreat();

        }
    }

    public void RemoveThreat(Transform threat) {
        if (threats.Contains(threat))
        {
            int threatInd = threats.IndexOf(threat);
            GameObject tempThrtLine = threatHUDLines[threatInd];
            threatHUDLines.Remove(tempThrtLine);
            Destroy(tempThrtLine);
            threats.RemoveAt(threatInd);
            if (CallOnDethreat != null)
                CallOnDethreat();
        }
    }

}
