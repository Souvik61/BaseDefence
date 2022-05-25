using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTankScript : TankScript
{
    public float multiplier;

    Vector2 forceBuff;
    bool hasToMove;

    protected override void Start()
    {
        base.Start();
        hasToMove = false;
    }

    protected override void FixedUpdate()
    {
        //rBody.MovePosition(nextPosition);
        //rBody.MoveRotation(nextRotation);
        rBody.AddForce(forceBuff, ForceMode2D.Impulse);

        forceBuff.Set(0, 0);
    }

    /// <summary>
    /// forward 1 -> move forward, forward -1 -> movebackward,turn-> Rotate clockwise if -1 and counterclockwise if 1
    /// </summary>
    public override void Move(int forward)
    {
        if (!isDestroyed)
        {
            //rBody.MovePosition(transform.position+ (forward * driveSpeed * transform.up));
            forceBuff = (forward * 0.01f * tankProperty.driveSpeed * transform.up);
            //rBody.AddForce(force * multiplier, ForceMode2D.Impulse);
        }
    }

    public override void Rotate(int turn)
    {
       // if (!isDestroyed)
            
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

}

