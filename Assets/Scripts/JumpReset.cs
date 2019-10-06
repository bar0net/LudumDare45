using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpReset : MonoBehaviour
{
    public Player player;

    CapsuleCollider2D _col;

    private void Start()
    {
        _col = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        List<Collider2D> lst = new List<Collider2D>();
        int x = _col.GetContacts(lst);
        
        foreach(Collider2D c in lst)
        {
            if (c.gameObject.tag == "Terrain")
            {
                player.grounded = true;
                return;
            }
        }
        player.grounded = false;
    }
}
