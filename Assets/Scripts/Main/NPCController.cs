using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Transform> waypoints;
    public int currentWaypoint = 0;
    public float minimumDistanceFromWaypoint = 1f;
    [Space(10)]
    public float distanceToStartFollowing = 4f;
    private float distanceToGetCaught = 1f;
    public bool chasing = false;
    [Space(10)]
    public Transform raycastPoint;

    private Transform target; // player to find
    private CharacterController playerCharacterController;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = Random.Range(0, waypoints.Count);
        target = PlayerController.Instance.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[currentWaypoint].position);

        playerCharacterController = target.gameObject.GetComponent<CharacterController>();
        distanceToGetCaught = (playerCharacterController.radius + agent.radius);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + playerCharacterController.height / 2, target.position.z);
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        // If the target is within my distance to chase. I will follow target
        if (distanceToTarget < distanceToStartFollowing && !chasing)
        {
            RaycastHit hit;
            //Debug.DrawRay(raycastPoint.position, targetPosition - raycastPoint.position);
            if (Physics.Raycast(raycastPoint.position, targetPosition - raycastPoint.position, out hit))
            {
                if (hit.transform.tag == "Player")
                {
                    //Debug.Log(" i hit a tag = " + hit.transform.name);
                    agent.SetDestination(targetPosition);
                    agent.speed *= 1.5f;
                    chasing = true;
                }
            }
        } else if (chasing)
        {
            agent.SetDestination(targetPosition);
            //Debug.Log("distance caught: " + distanceToGetCaught + " distance target: " + distanceToTarget);
            if (distanceToTarget < distanceToGetCaught)
            {
                target.gameObject.GetComponent<PlayerController>().OnBecameInvisible();
                chasing = false;
                agent.speed /= 1.5f;
                agent.SetDestination(waypoints[currentWaypoint].position);
            }
        }

        float distanceCheck = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);
        if (distanceCheck < minimumDistanceFromWaypoint && !chasing)
        {
            currentWaypoint++;
            if (currentWaypoint > waypoints.Count - 1)
            {
                currentWaypoint = 0;
            }
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }
}
