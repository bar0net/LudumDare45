using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glasses : Item
{
    protected override void ItemEffect()
    {
        Camera.main.farClipPlane = 1000.0F;
        foreach (ParticleSystem ps in FindObjectsOfType<ParticleSystem>())
        {
            ps.gameObject.SetActive(false);
        }
        Door door = FindObjectOfType<Door>();
        if (door) door.active = true;
    }
}
