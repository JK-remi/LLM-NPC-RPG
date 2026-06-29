using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel_Ingame : Panel_Base
{
    public UI_hpFrame hpBar;
    public TextMeshProUGUI uiInfo;

    private void Start()
    {
        hpBar.Init(GameManager.Instance.player);
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        uiInfo.text = MakeToDO();
        uiInfo.text += MakePlayerSkills();
    }

    private string MakeToDO()
    {
        string todo = "";
        if (!GameManager.Instance.IsEnemyAllDead())
        {
            int remain = GameManager.Instance.enemyCount;
            int entire = GameManager.Instance.enemies.Count;
            todo = "*TO DO LIST*\n" +
                "적 처치: " + (entire-remain) + "/" + entire + "\n\n";
        }
        else
        {
            todo = "*TO DO LIST*\n" +
                "광장의 우루슬라 찾기.\n\n";
        }

        return todo;
    }

    private string MakePlayerSkills()
    {
        string skillset = "*SKILL LIST*\n";
        List<SkillInfo> skills = GameManager.Instance.player.skills;
        for(int i=0; i<skills.Count;i++)
        {
            string skill = (skills[i].dmgVal > 0) ? string.Format("공격: {0}\n", skills[i].dmgVal) : string.Format("회복: {0}\n", Mathf.Abs(skills[i].dmgVal));
            skillset += skill;
        }
        return skillset;
    }
}
