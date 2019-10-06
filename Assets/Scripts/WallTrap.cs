using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrap : MonoBehaviour
{

    public ObjectPool projectilePool;
    public ObjectPool explosionPool;
    public Transform spawnPoint;

    public float cooldownTime = 1.0F;
    public float timeOffset = 0.0F;
    public bool fire = false;
    
    private Animator _anim;
    public bool triggered = false;

    [Header("Debug")]
    [SerializeField] float timer = 2.0F;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        timer = cooldownTime + timeOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        if (timer <= 0 && !triggered) { _anim.SetTrigger("Fire"); triggered = true; }

        if (fire && timer <= 0) Fire();

    }

    void Fire()
    {
        GameObject go = projectilePool.Create();
        go.transform.position = spawnPoint.position;

        Projectile p = go.GetComponent<Projectile>();
        p.direction = transform.right * transform.localScale.x;
        p.explosionPool = explosionPool;

        fire = false;
        triggered = false;
        timer = cooldownTime;
    }
}
