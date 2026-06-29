using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NarrationInfo
{
    public Sprite bg;
    public AudioClip clip;
    public string desc;
    public bool isEnd;
    public bool hasDialog;
}

public class Panel_Narration : Panel_Base
{
    public List<NarrationInfo> narrations;
    private NarrationInfo curNarration;
    private int enterCount = 0;

    public Button btnNarr;
    public Image imgBG;
    public TextMeshProUGUI textNarr;
    public GameObject outro;
    public Sprite outroBG;

    protected void Start()
    {
        btnNarr.gameObject.SetActive(true);
        outro.SetActive(false);
    }

    public override void Init()
    {
        if (enterCount < narrations.Count)
        {
            curNarration = narrations[enterCount++];

            imgBG.sprite = curNarration.bg;
            clip = curNarration.clip;
            textNarr.text = curNarration.desc;
        }
        else
        {
            btnNarr.gameObject.SetActive(false);
            outro.SetActive(true);
            imgBG.sprite = outroBG;
        }

        base.Init();
    }

    public void OnNext()
    {
        if (curNarration.isEnd == false)
            GameManager.Instance.OpenNarration();
        else
            GameManager.Instance.CloseNarration(curNarration.hasDialog);
    }
}
