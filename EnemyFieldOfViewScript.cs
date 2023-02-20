using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyFieldOfViewScript : MonoBehaviour
{
    public bool inView;
    public float radius = 5;
    [Range(1, 360)] public float angle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;

    public GameObject playerRef;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVCheck());
    }

    private IEnumerator FOVCheck() {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true) {
            yield return wait;
            FOV();
        }
    }

    private void FOV() {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeCheck.Length > 0) {
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < angle / 2) {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer)) {
                    inView = true;
                }
                else {
                    inView = false;
                }
            }
            else {
                inView = false;
            }
        } else if (inView) {
            inView = false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        //UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

        Vector3 angle01 = DirectionFromAngle(-transform.eulerAngles.z, -angle / 2);
        Vector3 angle02 = DirectionFromAngle(-transform.eulerAngles.z, angle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angle01 * radius);
        Gizmos.DrawLine(transform.position, transform.position + angle02 * radius);

        if (inView) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }
    }

    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees) {
        angleInDegrees += eulerY;
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
