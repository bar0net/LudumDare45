using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject template;

    Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject Create()
    {
        if (pool.Count == 0)
        {
            GameObject new_go = (GameObject)Instantiate(template);
            PoolableEntity pe = new_go.GetComponent<PoolableEntity>();
            if (!pe) new_go.AddComponent<PoolableEntity>();
            pe.pool = this;

            return new_go;
        }

        GameObject go = pool.Dequeue();
        go.SetActive(true);
        return go;
    }

    public void Release(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go);
    }
}
