using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPBoss : Boss, IEnemy
{
    public override void Die(Player player)
    {
        base.Die(player);
    }
}
