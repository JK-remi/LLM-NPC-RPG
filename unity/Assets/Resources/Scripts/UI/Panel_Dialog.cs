using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogInfo
{
    public string lName;
    public string rName;
    public Sprite lPortrait;
    public Sprite rPortrait;

    public Sprite backgrund;
    public AudioClip clip;
    public string hint;
    public string sys_msg;
    public string rag;
}

public class Panel_Dialog : Panel_Base
{
    public List<DialogInfo> listDialog;
    private DialogInfo curDialog;

    public Image imgBG;
    public Button btnRecord;
    public Button btnSkip;
    public Image imgPortraitL;
    public Image imgPortraitR;
    public TextMeshProUGUI txtDialog;
    public TextMeshProUGUI txtNmae;
    public TextMeshProUGUI txtHint;

    public AudioSource audioSource;

    private int enterCount = 0;

    public override void Init()
    {
        GameManager.Instance.stt.uiText = txtDialog;
        GameManager.Instance.stt.audioSource = audioSource;
        GameManager.Instance.gpt.uiText = txtDialog;
        GameManager.Instance.tts.audioSource = audioSource;

        isDialogEnd = false;

        if (enterCount < listDialog.Count)
        {
            curDialog = listDialog[enterCount++];
            GameManager.Instance.gpt.SetSystem(curDialog.sys_msg, curDialog.rag);
            if (enterCount == 2)
            {
                btnRecord.interactable = false;
                btnSkip.interactable = false;

                txtNmae.text = curDialog.lName;

                string prompt = 
@"아리엘이 목소리를 찾기위해 널 찾아왔어.
아리엘은 인간세계에서 외로움을 느끼고, 침울한 상태야.
네가 먼저 시작해.
";
                GameManager.Instance.gpt.OnGPT(prompt);
            }
            else
            {
                txtNmae.text = curDialog.rName;
                txtDialog.text = "RECORD를 눌러서 대화를 시작하세요.";
            }
        }

        imgBG.sprite = curDialog.backgrund;
        imgPortraitL.sprite = curDialog.lPortrait;
        imgPortraitR.sprite = curDialog.rPortrait;
        txtHint.text = curDialog.hint;
        clip = curDialog.clip;
        //GameManager.Instance.gpt.SYSTEM_MSG = curDialog.sys_msg;

        base.Init();
    }

    public void SttFinish(bool isSuccess)
    {
        btnRecord.interactable = false;
        if(isSuccess)
        {
            StartCoroutine(RequestGPT());
        }
        else 
        {
            StartCoroutine(WaitDialog(2f));
        }
    }

    private IEnumerator RequestGPT()
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        GameManager.Instance.gpt.OnGPT(txtDialog.text);
    }

    private bool isDialogEnd = false;
    public void GptFinish(bool isSuccess, bool isEnd) 
    {
        if (isSuccess)
        {
            isDialogEnd = isEnd;
            txtNmae.text = curDialog.lName;
            GameManager.Instance.tts.StartTTS(txtDialog.text);
        }
        else
        {
            StartCoroutine(WaitDialog(2f));
        }
    }

    public void TtsFinish(float wait) 
    {
        StartCoroutine(WaitDialog(wait));
    }

    private IEnumerator WaitDialog(float time)
    {
        yield return new WaitForSeconds(time);

        btnRecord.interactable = true;
        btnSkip.interactable = true;

        if (isDialogEnd) GameManager.Instance.CloseDialog(enterCount == 2);
    }

    public void OnRecord()
    {
        btnSkip.interactable = false;
        txtNmae.text = curDialog.rName;
    }

    public void OnSkip()
    {
        GameManager.Instance.CloseDialog(enterCount == 2);
    }
}
