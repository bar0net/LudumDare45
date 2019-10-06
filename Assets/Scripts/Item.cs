using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject itemPanel = null;

    SpriteRenderer _sr = null;

    // Start is called before the first frame update
    void Start()
    {
        if (itemPanel == null) itemPanel = GameObject.Find("ItemPanel");
        itemPanel.SetActive(false);
        _sr = GetComponentInChildren<SpriteRenderer>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            _sr.enabled = false;
            ItemEffect();
            itemPanel.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    protected virtual void ItemEffect() { }
}
