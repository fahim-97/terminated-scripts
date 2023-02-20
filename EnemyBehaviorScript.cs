using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBehaviorScript : MonoBehaviour
{
    EnemyFieldOfViewScript eFOV;
    AIDestinationSetter ads;

    public bool patrolling, investigating, detecting, playerCaught;
    public float detectionDuration, investigationDuration;
    float waitTillDetect, waitTillInvestigate;
    public Transform target;
    public Transform[] roomPatrolPoints;
    // Start is called before the first frame update
    void Start()
    {
        eFOV = GetComponent<EnemyFieldOfViewScript>();
        ads = GetComponent<AIDestinationSetter>();
        pickNewPatrol();
    }

    // Update is called once per frame
    void Update()
    {
        if (eFOV.inView)
        {
            detectPlayer();
        }
        else
        {
            waitTillDetect = 0;
        }

        if (patrolling)
        {
            patrolAround();
        }

        if (investigating)
        {
            investigateAround();
        }
        
    }

    public void investigatePoint(Transform positionToInvestigate)
    {
        patrolling = false;
        investigating = true;
        target = positionToInvestigate;
    }
    
    void investigateAround()
    {
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            waitTillInvestigate += Time.deltaTime;
            if (waitTillInvestigate > investigationDuration)
            {
                patrolling = true;
                investigating = false;
                pickNewPatrol();
            }
        }
    }

    void patrolAround()
    {

        ads.target = target;

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            pickNewPatrol();
        }
        
        
    }

    void pickNewPatrol()
    {
        int r = Random.Range(0, roomPatrolPoints.Length);
        target = roomPatrolPoints[r];
    }

    void detectPlayer()
    {
        waitTillDetect += Time.deltaTime;
        if (waitTillDetect >= detectionDuration)
        {
            playerCaught = true;
        }
    }
}
