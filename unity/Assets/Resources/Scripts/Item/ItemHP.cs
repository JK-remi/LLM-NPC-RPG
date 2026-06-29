using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHP : Item
{
    protected override void GetItem(PlayerController player)
    {
        if (player == null) return;
        if (player.IsMaxHP())
        {
            return;
        }

        player.ChangeHP(-Value);

        StartCoroutine(Disappear());
    }
}
