using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_hpFrame : MonoBehaviour
{
    public Image imgHpBar;
    public Image portrait;
    public TextMeshProUGUI uiText;

    private ControllerBase target;

    public void Init(ControllerBase cb)
    {
        target = cb;
        portrait.sprite = target.portrait;
    }

    private void SetHP()
    {
        imgHpBar.fillAmount = (float)target.CurHP / (float)target.MaxHP;
        uiText.text = target.CurHP + "/" + target.MaxHP;
    }

    private void Update()
    {
        if (target == null) return;

        SetHP();
    }
}
