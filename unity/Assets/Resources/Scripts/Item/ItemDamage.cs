using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDamage : Item
{
    protected override void GetItem(PlayerController player)
    {
        if (player == null) return;
        player.ChangeHP(Value);
        if (player.IsDead())
            GameManager.Instance.Respawn();
    }
}
