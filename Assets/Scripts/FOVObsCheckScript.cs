using System;
using UnityEngine;

public class FOVObsCheckScript : MonoBehaviour
{
    public float radius;
    public float angle;
    [SerializeField]
    float collisionChecksEverySec;
    [SerializeField]
    LayerMask targetMask;

    public bool isObstaclesInRange;

    public GameObject[] obstaclesInRange;
    Collider2D[] collArray; 

    float timer;

    private void Awake()
    {
        obstaclesInRange = new GameObject[4];
        collArray = new Collider2D[8];
    }

    private void Start()
    {
        //InvokeRepeating(nameof(CheckObstacle), 0, 0.5f);
        CheckObstacle();
    }

    private void Update()
    {
        if (timer <= 0)
        {
            CheckObstacle();
            timer = 1 / collisionChecksEverySec;//Reset timer
            //Debug.Log("Collision Check");
        }
        timer -= Time.deltaTime;
    }

    /*
    private void FixedUpdate()
    {
        if (timer <= 0)
        {
            CheckObstacle();
            timer = 1 / collisionChecksEverySec;//Reset timer
            //Debug.Log("Collision Check");
        }
        timer -= Time.fixedDeltaTime;
    }
    */

    /*
    void CheckObstacle()
    {
        int instId = GetInstanceID();
        Array.Clear(obstaclesInRange, 0, obstaclesInRange.Length);

        isObstaclesInRange = false;

        //Collider2D[] collArray = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);
        Collider2D[] collArray = new Collider2D[3];
        int res = Physics2D.OverlapCircleNonAlloc(transform.position, radius, collArray, targetMask);

        /*
        for (int i = 0; i < obstaclesInRange.Length && i < collArray.Length; i++)
        {
            obstaclesInRange[i] = collArray[i].gameObject;
        }*/

    /*
    int j = 0;
    foreach (var item in collArray)
    {
        var directionToTarget = (item.transform.position - transform.position).normalized;
        if (Vector2.Angle(transform.up, directionToTarget) < angle)//If target in fov
        { 
            obstaclesInRange[j] = item.gameObject;//Add them to obstaclesinrange Array
            j++;
            isObstaclesInRange = true;
        }
    }
    */
    /*
    if (res > 0)//If resultant array is non zero.
    {
        int j = 0;
        for (int i = 0; i < collArray.Length && j < 3; i++)
        {
            if (collArray[i] != null)//Check if element is not null.
            {
                var directionToTarget = (collArray[i].transform.position - transform.position).normalized;
                if (Vector2.Angle(transform.up, directionToTarget) < angle)//If target in fov
                {
                    obstaclesInRange[j] = collArray[i].gameObject;//Add them to obstaclesinrange Array
                    j++;
                    isObstaclesInRange = true;
                }
            }

        }
    }
}
*/

    /*
    void CheckObstacle()
    {
        Array.Clear(obstaclesInRange, 0, obstaclesInRange.Length);
        Array.Clear(collArray, 0, collArray.Length);

        isObstaclesInRange = false;

        int res = Physics2D.OverlapCircleNonAlloc(transform.position, radius, collArray, targetMask);

        if (res > 0)//If resultant array is non zero.
        {
            int j = 0;
            for (int i = 0; i < collArray.Length && j < 4; i++)
            {
                if (collArray[i] != null)//Check if element is not null.
                {
                    var directionToTarget = (collArray[i].transform.position - transform.position).normalized;
                    if (Vector2.Angle(transform.up, directionToTarget) < angle)//If target in fov
                    {
                        float distToTarget = Vector2.Distance(transform.position, collArray[i].transform.position);

                        var hit = Physics2D.Raycast(transform.position, directionToTarget, distToTarget, targetMask);

                        if (hit.collider != null)
                        {
                            if (hit.collider == collArray[i])
                            {
                                obstaclesInRange[j] = collArray[i].gameObject;//Add them to obstaclesinrange Array
                                j++;
                                isObstaclesInRange = true;
                            }


                        }    
                    }
                }

            }
        }

    }
    */

    void CheckObstacle()
    {
        Array.Clear(obstaclesInRange, 0, obstaclesInRange.Length);
        Array.Clear(collArray, 0, collArray.Length);

        isObstaclesInRange = false;

        int res = Physics2D.OverlapCircleNonAlloc(transform.position, radius, collArray, targetMask);

        if (res > 0)//If resultant array is non zero.
        {
            int j = 0;
            for (int i = 0; i < collArray.Length && j < 4; i++)
            {
                if (collArray[i] != null)//Check if element is not null.
                {
                    var directionToTarget = (collArray[i].transform.position - transform.position).normalized;
                    if (Vector2.Angle(transform.up, directionToTarget) < angle)//If target in fov
                    {
                        if (!IsObstructed(collArray[i], directionToTarget))
                        {
                            obstaclesInRange[j] = collArray[i].gameObject;//Add them to obstaclesinrange Array
                            j++;
                            isObstaclesInRange = true;
                        }
                    }
                }

            }
        }

    }

    bool IsObstructed(Collider2D collider,Vector2 directionToTarget)
    {
        float distToTarget = Vector2.Distance(transform.position, collider.transform.position);

        var hit = Physics2D.Raycast(transform.position, directionToTarget, distToTarget, targetMask);

        if (hit.collider != null)
        {
            if (hit.collider == collider)
            {
                return false;
            }
            return true;
        }
        return true;
    }

}
