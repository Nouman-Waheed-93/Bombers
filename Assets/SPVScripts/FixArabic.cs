using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArabicSupport;
using UnityEngine.UI;

public class FixArabic : MonoBehaviour {

    public string ArabicText;
    public bool tashkeel = true;
    public bool hinduNumbers = true;
    
    // Use this for initialization
    void Start () {
        gameObject.GetComponent<Text>().text = ArabicFixer.Fix(ArabicText, tashkeel, hinduNumbers);
    }
    
}
