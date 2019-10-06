using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform turretSprite;
    public Transform laserContact;

    public Transform leftLimit;
    public Transform rightLimit;

    public SpriteRenderer laserSight;
    public ObjectPool shootPool;
    public ObjectPool explosionPool;

    public float angularSpeed = 30.0F;
    public float moveCooldown = 0.5F;
    public float shootCooldown = 0.2F;

    public bool debug = false;
    [SerializeField] float leftAngle = 0.0F;
    [SerializeField] float rightAngle = 0.0F;

    bool toRight = true;
    float moveCdTimer = 0.0F;
    float shootCdTimer = 0.0F;
    // Start is called before the first frame update
    void Start()
    {
        leftAngle = Vector2.Angle(-turretSprite.up, leftLimit.position - turretSprite.position);
        if (leftLimit.localPosition.x < 0) leftAngle = -leftAngle;

        rightAngle = Vector2.Angle(-turretSprite.up, rightLimit.position - turretSprite.position);
        if (rightLimit.localPosition.x < 0) rightAngle = -rightAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (shootCdTimer > 0.0F) shootCdTimer -= Time.deltaTime;

        Move();
        CheckLaser();
    }

    void Move()
    {
        if (moveCdTimer > 0)
        {
            moveCdTimer -= Time.deltaTime;
            return;
        }

        float rot = turretSprite.localEulerAngles.z;
        while (rot < -180.0F) rot += 360.0F;
        while (rot > 180.0F) rot -= 360.0F;
        float dst = angularSpeed * Time.deltaTime;

        if (toRight)
        {
            if (rot + dst > rightAngle) turretSprite.Rotate(0.0F, 0.0F, -dst);
            else
            {
                turretSprite.Rotate(0.0F, 0.0F, rightAngle - rot);
                toRight = false;
                moveCdTimer = moveCooldown;
            }
        }
        else
        {
            if (rot - dst < leftAngle) turretSprite.Rotate(0.0F, 0.0F, dst);
            else
            {
                turretSprite.Rotate(0.0F, 0.0F, leftAngle - rot);
                toRight = true;
                moveCdTimer = moveCooldown;
            }
        }
    }

    void CheckLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(laserSight.transform.position, -turretSprite.up);
        if (hit)
        {
            laserSight.size = new Vector2(laserSight.size.x, hit.distance);
            laserContact.gameObject.SetActive(true);
            laserContact.position = hit.point;
            if (hit.collider.tag == "Player") Shoot();
        }
        else
        {
            laserSight.size = new Vector2(laserSight.size.x, 100);
            laserContact.gameObject.SetActive(false);
        }
    }

    void Shoot()
    {
        GameObject go = shootPool.Create();
        go.transform.position = laserSight.transform.position;
        Projectile p = go.GetComponent<Projectile>();
        p.direction = -turretSprite.up;
        p.explosionPool = explosionPool;
    }
}
