using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBase : MonoBehaviour
{
    protected const string MOVE_X = "MoveX";
    protected const string MOVE_Y = "MoveY";
    protected const string SPEED = "Speed";
    
    public string charName;
    public Sprite portrait;
    public Sprite highPortrait;
    public List<SkillInfo> skills = new List<SkillInfo>();

    [SerializeField]
    protected int maxHP = 10;
    public int MaxHP { get { return maxHP; } }
    protected int curHP = 10;
    public int CurHP { get { return curHP; } }

    [SerializeField]
    protected float speed = 5f;

    protected Rigidbody2D rigidbod2d;
    protected Animator animator;

    void Start()
    {
        rigidbod2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Init();
    }

    protected virtual void Init()
    {
        curHP = maxHP;
    }

    public virtual void ChangeHP(int val)
    {
        curHP = Mathf.Clamp(curHP - val, 0, maxHP);
    }

    public bool IsDead()
    {
        return curHP <= 0;
    }
}
