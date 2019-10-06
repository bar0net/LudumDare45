using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    const float RETURN_TIME = 0.05F;
    Vector3 position = Vector3.zero;
    float timer = 0.0F;
    float shakeRadius = 0.01F;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (position != transform.position) transform.position = Vector3.Lerp(position, transform.position, timer / RETURN_TIME);
        if (timer > 0.0F) timer -= Time.deltaTime;
        if (timer < 0.0F) timer = 0.0F;
    }

    public void Shake()
    {
        if (timer != 0.0F) return;

        float angle = Random.Range(-180.0F, 180.0F);
        this.transform.Translate(Mathf.Cos(angle) * shakeRadius, Mathf.Sin(angle) * shakeRadius, 0.0F);
        timer = RETURN_TIME;
    }
}
