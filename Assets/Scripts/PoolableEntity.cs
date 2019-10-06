using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableEntity : MonoBehaviour
{
    public ObjectPool pool = null;

    public virtual void Destroy()
    {
        if (pool == null) Destroy(this.gameObject);
        else pool.Release(this.gameObject);
    }
}
