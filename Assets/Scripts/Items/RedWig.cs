using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedWig : Item
{
    protected override void ItemEffect()
    {
        FindObjectOfType<Player>().canDash = true;
    }
}
