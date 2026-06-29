using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : ControllerBase
{
    public SkillInfo dropSkill;

    [SerializeField]
    private bool isVertical = false;

    [SerializeField]
    private float changeTime = 3f;
    private float timer;
    private int direction = 1;

    [SerializeField]
    private float idleTime = 1f;
    private bool isIdle = false;

    protected void Init()
    {
        timer = changeTime;
        isIdle = false;
        base.Init();
    }

    void Update()
    {
        if (GameManager.Instance.IsIngame() == false) return;

        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            if(isIdle)
            {
                timer = changeTime;
                isIdle = false;
                direction *= -1;
            }
            else
            {
                timer = idleTime;
                isIdle = true;
            }
        }

        if(isIdle)
        {
            if (isVertical)
            {
                SetAnimator(0f, direction, 0f);
            }
            else
            {
                SetAnimator(direction, 0f, 0f);
            }
        }
        else
        {
            Move();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player == null) return;

        // UIManager 전투 시작
        GameManager.Instance.OpenBattle(this);
    }

    private void SetAnimator(float x, float y, float s)
    {
        if (animator == null) return;

        animator.SetFloat(MOVE_X, x);
        animator.SetFloat(MOVE_Y, y);
        animator.SetFloat(SPEED, s);
    }

    private void Move()
    {
        Vector2 pos = rigidbod2d.position;
        if (isVertical)
        {
            pos.y += (float)direction * speed * Time.deltaTime;
            SetAnimator(0f, direction, Mathf.Abs(direction));
        }
        else
        {
            pos.x += (float)direction * speed * Time.deltaTime;
            SetAnimator(direction, 0f, Mathf.Abs(direction));
        }

        rigidbod2d.MovePosition(pos);
    }
}
