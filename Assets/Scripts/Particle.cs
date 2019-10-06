using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : PoolableEntity
{
    enum ParticleStates { UP, DOWN, END }

    public float upTime = 0.1F;
    public float downTime = 0.3F;

    SpriteRenderer _sr;
    float timer = 0.0F;
    ParticleStates state = ParticleStates.UP;

    AudioSource _as;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        state = ParticleStates.UP;
        timer = upTime;
        if (!_sr) _sr = GetComponent<SpriteRenderer>();
        _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, 1 - timer / upTime);

        if (_as)
        {
            _as.time = 0;
            _as.pitch = 0.75F + 0.5F * Random.value;
            //_as.volume = 0.85F + 0.3F * Random.value;
            _as.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        switch(state)
        {
            case ParticleStates.UP:
                if (timer > 0) _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, 1 - timer / upTime);
                else { state = ParticleStates.DOWN; timer = downTime; }
                break;
            case ParticleStates.DOWN:
                if (timer > 0) _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, timer / downTime);
                else { state = ParticleStates.END; timer = downTime; }
                break;
            case ParticleStates.END:
                Destroy();
                break;
        }

    }
}
