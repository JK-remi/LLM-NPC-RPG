using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            _instance = GameObject.FindObjectOfType<GameManager>();
            if (_instance == null)
            {
                GameObject container = new GameObject("GameManager");
                _instance = container.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    public Panel_Opening panelOpening;
    public Panel_Narration panelNarration;
    public Panel_Dialog panelDialog;
    public Panel_Battle panelBattle;
    public Panel_Ingame panelIngame;
    private AudioSource audioSource;

    private Panel_Base curPanel;

    public PlayerController player;
    public List<EnemyController> enemies;
    public EnemyController boss;

    [HideInInspector] public STT stt;
    [HideInInspector] public TTS tts;
    [HideInInspector] public GPT gpt;

    public Transform transRespawn;
    public bool isTest = false;
    private void Awake()
    {
        Init();
    }

    bool isKilSwitch = false;
    private void Update()
    {
        if(!isKilSwitch && curPanel == panelIngame && Input.GetAxis("Jump") > 0f)
        {
            isKilSwitch = true;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsDead()) continue;
                enemies[i].ChangeHP(1000);
                enemies[i].gameObject.SetActive(false);
                player.skills.Add(enemies[i].dropSkill);
            }
            panelIngame.UpdateInfo();
            boss.gameObject.SetActive(true);
            OpenNarration();
        }
    }

    private void Init()
    {
        Application.targetFrameRate = 30;
        audioSource = this.gameObject.GetComponent<AudioSource>();

        stt = this.GetComponent<STT>();
        tts = this.GetComponent<TTS>();
        gpt = this.GetComponent<GPT>();

        player.gameObject.SetActive(true);
        boss.gameObject.SetActive(false);

        int i = 0;
        for (i = 0; i < enemies.Count; i++)
            enemies[i].gameObject.SetActive(true);

        if(!isTest)
        {
            panelOpening.Init();
        }
        else
        {
            OpenIngame();
            //OpenDialog();
        }

        enemyCount = enemies.Count;
    }
    [HideInInspector]
    public int enemyCount;
    
    public bool IsEnemyAllDead()
    {
        int deadCount = 0;
        for(int i=0; i<enemies.Count; i++)
        {
            if (enemies[i].IsDead()) deadCount++;
        }

        enemyCount = enemies.Count - deadCount;
        return deadCount == enemies.Count;
    }

    public void PlayAudio(AudioClip clip)
    {
        if (clip == null) return;
        if (audioSource.isPlaying && audioSource.clip == clip) return; // 현재 재생 중인 clip과 같으면 다시 재생 필요 X

        audioSource.clip = clip;
        audioSource.Play();
    }

    public bool IsIngame()
    {
        return curPanel == panelIngame;
    }

    public void CloseOpening()
    {
        panelOpening.Close();
        OpenNarration();
    }

    private int bossTry = 0;
    public void OpenBattle(EnemyController target)
    {
        if(bossTry == 0 && target == boss)
        {
            bossTry++;
            OpenNarration();
        }
        else
        {
            panelBattle.Init(target);
            curPanel = panelBattle;
        }
    }

    public void Respawn() 
    {
        player.ChangeHP(-player.MaxHP);
        player.transform.position = transRespawn.position;
    }

    public void ChangeSttSpell(string spell, TextMeshProUGUI uiResult)
    {
        stt.uiText = uiResult;
        stt.spellText = spell;
    }

    public void HitTarget(STT.PronunciationData data)
    {
        panelBattle.HitTarget(data);
    }

    public void CloseBattle()
    {
        panelBattle.Close();
        if (player.IsDead())
        {
            Respawn();
            OpenIngame();
        }
        else if(IsEnemyAllDead() && !boss.IsDead())
        {
            boss.gameObject.SetActive(true);
            OpenNarration();
        }
        else if(boss.IsDead())
        {
            OpenNarration();
        }
        else
        {
            OpenIngame();
        }

        panelIngame.UpdateInfo();
    }

    public void OpenNarration()
    {
        panelNarration.Init();
        curPanel = panelNarration;
    }

    public void CloseNarration(bool hasDialog)
    {
        if(hasDialog)
        {
            OpenDialog();
        }
        else
        {
            OpenIngame();
        }

        panelNarration.Close();
    }

    public void OpenDialog()
    {
        panelDialog.Init();
        curPanel = panelDialog;
    }

    public void CloseDialog(bool isBattle=false)
    {
        panelDialog.Close();

        if (isBattle)
        {
            panelBattle.Init(boss);
            curPanel = panelBattle;
        }
        else
        {
            OpenIngame();
        }
    }

    public void SttFinish(bool isSuccess)
    {
        panelDialog.SttFinish(isSuccess);
    }

    public void GptFinish(bool isSuccess, bool isEnd =false)
    {
        panelDialog.GptFinish(isSuccess, isEnd);
    }

    public void TtsFinish(float wait)
    {
        panelDialog.TtsFinish(wait);
    }

    public void OpenIngame()
    {
        curPanel = panelIngame;
        panelIngame.Init();
    }
}
