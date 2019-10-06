using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolableEntity
{
    public Vector3 direction;
    public float lifeTime = 5.0f;
    public float speed = 10.0F;

    public float timer = 0.0F;

    public ObjectPool explosionPool = null;

    AudioSource _as;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        timer = lifeTime;

        if (_as)
        {
            _as.time = 0;
            _as.pitch = 0.75F + 0.5F * Random.value;
            _as.volume = 0.035F + 0.03F * Random.value;
            _as.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy();
            return;
        }

        this.transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponentInParent<Player>().Die();

        Destroy();
    }

    public override void Destroy()
    {
        CameraControl cc = FindObjectOfType<CameraControl>();
        if (cc) cc.Shake();

        if (explosionPool)
        {
            GameObject go = explosionPool.Create();
            go.transform.position = transform.position;
        }

        if (pool == null) Destroy(this.gameObject);
        else pool.Release(this.gameObject);
    }
}
