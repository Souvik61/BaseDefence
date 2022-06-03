using UnityEngine;

public class NewTankScript : TankScript
{
    public float moveMultiplier;
    public float rotateMultiplier;

    Vector2 forceBuff;
    float torqueBuff;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        rBody.AddForce(forceBuff, ForceMode2D.Impulse);
        rBody.AddTorque(torqueBuff);

        forceBuff.Set(0, 0);
        torqueBuff = 0;
    }

    /// <summary>
    /// forward 1 -> move forward, forward -1 -> movebackward
    /// </summary>
    public override void Move(int forward)
    {
        if (!isDestroyed)
        {
            //rBody.MovePosition(transform.position+ (forward * driveSpeed * transform.up));
            forceBuff = (Vector2)(forward * moveMultiplier * tankProperty.driveSpeed * transform.up);
            //rBody.AddForce(force * multiplier, ForceMode2D.Impulse);
        }
    }

    public override void Rotate(int turn)
    {
        if (!isDestroyed)
            torqueBuff = turn * tankProperty.rotateSpeed * rotateMultiplier;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Trigger Enter");
        if (collision.tag.Contains("tag_projectile"))
        {
            Destroy(collision.gameObject);//Destroy the projectile

            if (!collision.CompareTag("tag_projectile" + unitC.teamID))
                OnTakeDamage(collision.transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision Enter");
    }

}

