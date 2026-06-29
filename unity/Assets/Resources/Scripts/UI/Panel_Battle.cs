using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Battle : Panel_Base
{
    public UI_hpFrame enemyHP;
    public UI_hpFrame playerHP;

    public Image enemyPortrait;
    public Image playerPortrait;

    public TextMeshProUGUI uiSpell;
    public TextMeshProUGUI uiReult;

    public TMP_Dropdown uiSkillset;
    public Button btnRecord;

    public AudioSource audioSource;
    public AudioClip audioBattleEnd;

    public UI_Hit hitEffect;
    public UI_Hit healEffect;
    public Transform enemyPos;
    public Transform playerPos;

    private List<SkillInfo> skills;

    private EnemyController enemy;
    private PlayerController player;

    public void Init(EnemyController target)
    {
        base.Init();

        enemy = target;
        player = GameManager.Instance.player;

        enemyHP.Init(enemy);
        playerHP.Init(player);

        enemyPortrait.sprite = enemy.highPortrait;
        playerPortrait.sprite = player.highPortrait;

        uiSkillset.ClearOptions();
        skills = player.skills;
        List<string> strSkills = new List<string>(skills.Count);
        for (int i = 0; i < skills.Count; i++)
        {
            string skill = (skills[i].dmgVal > 0) ? string.Format("공격: {0}\n", skills[i].dmgVal) : string.Format("회복: {0}\n", Mathf.Abs(skills[i].dmgVal));
            strSkills.Add(skill);
        }
        uiSkillset.AddOptions(strSkills);
        uiSkillset.value = 0;
        SetSkill();
        uiReult.text = "주문을 선택하고, RECORD를 눌러 주세요.";

        GameManager.Instance.stt.audioSource = audioSource;
    }

    private SkillInfo curSkill;
    public void SetSkill()
    {
        int idx = uiSkillset.value;

        curSkill = skills[idx];
        uiSpell.text = curSkill.spell;
        GameManager.Instance.ChangeSttSpell(uiSpell.text, uiReult);
    }

    private string GetDamageResult(ControllerBase a, ControllerBase b, int damage)
    {
        string result = "";
        if (damage > 0)
        {
            b.ChangeHP(damage);
            if (a == player) hitEffect.Play(enemyPos.transform, damage);
            else hitEffect.Play(playerPos.transform, damage);

            if (b.IsDead() == false)
            {
                result = string.Format("{0}은(는) {1}에게 {2}의 피해를 줬다.", a.charName, b.charName, damage);
            }
            else
            {
                audioSource.clip = audioBattleEnd;
                audioSource.Play();
                result = string.Format("{0}은(는) {1}을 쓰러트렸다!!", a.charName, b.charName, damage);
            }
        }
        else
        {
            a.ChangeHP(damage);
            if (a == player) healEffect.Play(playerPos.transform, -damage);
            else healEffect.Play(enemyPos.transform, -damage);
            result = string.Format("{0}은(는) {1}만큼 치유했다.", a.charName, Mathf.Abs(damage));
        }

        return result;
    }

    private STT.PronunciationData pronunData = null;
    public void HitTarget(STT.PronunciationData data)
    {
        pronunData = data;
        btnRecord.interactable = false;

        if (pronunData != null)
        {
            int damage = data.CalculateDamage(curSkill.dmgVal);
            uiReult.text += GetDamageResult(player, enemy, damage);
        }
        
        StartCoroutine(HitProcess());
    }

    private IEnumerator HitProcess()
    {
        if(pronunData == null || enemy.IsDead())
        {
            if (enemy.IsDead())
            {
                player.skills.Add(enemy.dropSkill);
                enemy.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(2f);
        }
        else
        {
            // 공격 이펙트
            yield return new WaitForSeconds(2f);

            // 적 공격
            int randIdx = Random.Range(0, enemy.skills.Count);
            SkillInfo skill = enemy.skills[randIdx];

            uiReult.text = string.Format("{0}은(는) [{1}]을(를) 사용했다.", enemy.charName, skill.spell);
            uiReult.text += GetDamageResult(enemy, player, skill.dmgVal);
            // 적 공격 이펙트
            yield return new WaitForSeconds(2f);
        }

        uiReult.text = "주문을 선택하고, RECORD를 눌러 주세요.";
        pronunData = null;
        flagClicked = 0;
        uiSkillset.interactable = true;
        btnRecord.interactable = true;

        if (player.IsDead() || enemy.IsDead())
            GameManager.Instance.CloseBattle();
    }

    int flagClicked = 0;
    public void OnRecord()
    {
        if (flagClicked > 0) return;

        flagClicked++;
        uiReult.text = "녹음 중 입니다.";
        uiSkillset.interactable = false;
    }
}
