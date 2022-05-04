using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour {
    public GameObject guideText0, guideText1, guideText2, btnGuideNext, btnGuideBefore, guide, btnRecord;
    public GameObject portal;
    public bool enablePortal = false;
    int squeakBtnClickCount = 0;
    public bool startCounting, scalePortalOn = false;

    public float scaleNum = 0.03f;

    private Vector3 scaleChange;

    void Start () {
        guideText0.SetActive (true);
        btnGuideNext.SetActive (true);
        btnGuideBefore.SetActive (false);
        guideText1.SetActive (false);
        guideText2.SetActive (false);
        btnRecord.SetActive (false);
        portal.SetActive (false);
    }

    void Awake () {
        scaleChange = new Vector3 (scaleNum, scaleNum, scaleNum);
    }

    void Update () {
        // Debug.Log (squeakBtnClickCount);
        if (squeakBtnClickCount == 1) {
            Debug.Log ("Run ShowGuideText2");
            ShowGuideText2 ();
        } else if (squeakBtnClickCount == 2) {
            HideAllGuide ();
            Debug.Log ("Run HideAllGuide");
        }

        if (enablePortal) PortalAppear ();
        if (scalePortalOn) PortalScale ();
    }

    public void ShowGuideText0 () {
        guideText0.SetActive (true);
        guideText1.SetActive (false);
        guideText2.SetActive (false);
        btnGuideNext.SetActive (true);
        btnGuideBefore.SetActive (false);

        startCounting = false;
    }

    public void ShowGuideText1 () {
        btnRecord.SetActive (true);
        guideText0.SetActive (false);
        guideText1.SetActive (true);
        guideText2.SetActive (false);
        btnGuideNext.SetActive (false);
        btnGuideBefore.SetActive (true);
        startCounting = true;
    }

    public void ShowGuideText2 () {
        guideText0.SetActive (false);
        guideText1.SetActive (false);
        guideText2.SetActive (true);
        btnGuideNext.SetActive (false);
        btnGuideBefore.SetActive (false);

    }

    void HideAllGuide () {
        guide.SetActive (false);
    }

    public void CountSqueakClick () {
        if (startCounting) squeakBtnClickCount++;
    }

    public void PortalAppear () {
        portal.SetActive (true);
    }

    void PortalScale () {
        if (portal.transform.localScale.x > 0.0f) portal.transform.localScale -= scaleChange;
        else if (portal.transform.localScale.x < 0.0f) portal.SetActive (false);
    }
}