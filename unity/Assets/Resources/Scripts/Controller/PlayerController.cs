using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ControllerBase
{
    private float timeInvincible = 2f;
    private bool isInvincible = false;
    private float invincibleTimer = 0f;

    private Vector2 lookDirection = new Vector2(0, -1);

    private AudioSource audio;

    void Update()
    {
        if (GameManager.Instance.IsIngame() == false)
        {
            if(audio.isPlaying) audio.Stop();
            isInvincible = false;
            return;
        }
        if(IsDead())
        {
            if(audio.isPlaying) audio.Stop();
            isInvincible = false;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        // x, y 0인지 확인 (Approximately - 부동 소수점의 비정확성 염두)
        if (!Mathf.Approximately(move.x, 0f) || !Mathf.Approximately(move.y, 0f))    
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat(MOVE_X, lookDirection.x);
        animator.SetFloat(MOVE_Y, lookDirection.y);
        animator.SetFloat(SPEED, move.magnitude);

        Vector2 pos = rigidbod2d.position + move * speed * Time.deltaTime;
        rigidbod2d.MovePosition(pos);
        if(move.magnitude > 0f)
        {
            if(audio.isPlaying == false) audio.Play();
        }
        else
        {
            audio.Stop();
        }

        if(isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer < 0f)
            {
                isInvincible = false;
            }
        }
    }

    protected override void Init()
    {
        audio = GetComponent<AudioSource>();
        base.Init();
    }

    public bool IsMaxHP()
    {
        return curHP >= maxHP;
    }

    public override void ChangeHP(int val)
    {
        if (val > 0)
        {
            if (isInvincible) 
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        base.ChangeHP(val);
    }
}
