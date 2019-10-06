using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningShoes : Item
{
    protected override void ItemEffect()
    {
        FindObjectOfType<Player>().jumpSkill = true;
    }
}
