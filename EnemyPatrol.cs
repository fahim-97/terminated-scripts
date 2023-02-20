using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] PlayerMovement player;
    [SerializeField] SpriteRenderer spr;
    public Transform FOV;
    public float turnSpeed = .01f;
    Quaternion rotGoal;
    Vector3 direction;

    public bool playerInView;
    public EnemyFieldOfViewScript enemyFov;
    public float detectionDuration;
    float waitTillDetection;
    public GameObject exclaimationMark;
    public AudioManager audiomgr;


    [SerializeField] float speed = 30f;
    //[SerializeField] float maxRange;

    [SerializeField] Transform[] waypointList;
    [SerializeField] private int waypointIndex;
    [SerializeField] bool reverseWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        //enemyFov = gameObject.GetComponent<EnemyFieldOfViewScript>();
        //Physics2D.queriesStartInColliders = false;
        audiomgr = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        //HandleRotation();
        RotateFOV();
        HandleSpriteFlip();

        CheckForEnemyFOV();
    } 

    void CheckForEnemyFOV()
    {
        playerInView = enemyFov.inView;

        if (playerInView)
        {
            audiomgr.Play("Nearby");
            waitTillDetection += Time.deltaTime;

            if (!exclaimationMark.activeSelf) exclaimationMark.SetActive(true);
            //Debug.Log("true");


            if (waitTillDetection > detectionDuration)
            {
                ///////ADD GAME ENDING LOGIN
                audiomgr.Play("Shriek");
                SceneManager.LoadScene("Dead Dead");
            }
        }
        else
        {
            if (waitTillDetection >= 0) waitTillDetection -= Time.deltaTime;
            else {
                waitTillDetection = 0;
            }
            // Debug.Log(waitTillDetection);
       
        }

        if (waitTillDetection == 0) {
            exclaimationMark.SetActive(false);
            // Debug.Log("false");
        }

    }

    private void HandleSpriteFlip()
    {
        if (waypointList[waypointIndex].transform.position.x > transform.position.x)
        {
            spr.flipX = false;
        }
        else
        {
            spr.flipX = true;
        }
    }

    private void HandleMovement() {
        Transform waypoint = waypointList[waypointIndex];

        Vector3 waypointDir = (waypoint.position - transform.position).normalized;
        direction = waypointDir;

        float distanceBefore = Vector3.Distance(transform.position, waypoint.position);
        transform.position = transform.position + waypointDir * speed * Time.deltaTime;
        float distanceAfter = Vector3.Distance(transform.position, waypoint.position);

        if (distanceBefore <= distanceAfter) {
            // Go to next waypoint


            ///// PATROL REVERSES WHEN ENEMY REACHES LAST POINT
            if (!reverseWaypoint)
            {
                waypointIndex += 1;

                if (waypointIndex > waypointList.Length - 1)
                {
                    reverseWaypoint = true;
                    waypointIndex = waypointList.Length-1;
                }
            }
            else
            {
                waypointIndex -= 1;

                if (waypointIndex < 0)
                {
                    reverseWaypoint = false;
                    waypointIndex = 0;
                }

            }


            //waypointIndex = (waypointIndex + 1) % waypointList.Length;

            //if (waypointIndex >)
            //{

            //}
        }
    }

    private void RotateFOV()
    {
        Transform waypoint = waypointList[waypointIndex];

        var offset = 90f;
        Vector2 direction = waypoint.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        FOV.rotation = Quaternion.Euler(Vector3.forward * (angle + offset + 180));
    }

    private void HandleRotation() {
        rotGoal = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, turnSpeed * Time.deltaTime);
    }
}
