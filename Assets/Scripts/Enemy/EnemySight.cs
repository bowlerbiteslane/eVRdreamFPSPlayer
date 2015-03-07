using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {

    public float fieldOfViewAngle = 110f;
    public GameObject personalPlayerSighting;
    public bool playerInSight;

    GameObject player;
    Animator playerAnim;
    SphereCollider col;
    NavMeshAgent nav;
    HashIDs hash;
    HealthManager playerHealth;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        playerAnim = player.GetComponent<Animator>();
        playerHealth = player.GetComponent<HealthManager>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        col = GetComponent<SphereCollider>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Update() {
        
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInSight = false;

            if (playerHealth.Health > 0)
            {
                Vector3 direction = other.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                if (angle < fieldOfViewAngle * 0.5f)
                {
                    RaycastHit hit;
                
                    //FIX ME - offset fot the player height
                    direction -= other.transform.up;
                    //Debug.DrawRay(transform.position + transform.up, direction, Color.red, col.radius);
                    if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                    {
                        if (hit.collider.gameObject == player)
                        {
                            playerInSight = true;

                            personalPlayerSighting = hit.collider.gameObject;
                        }
                    }
                }

                /*//FIX ME DETECT PLAYER STATE
                int playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo(0).nameHash;
                int playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo(1).nameHash;
            
                if (playerLayerZeroStateHash == hash.locomotionState || playerLayerOneStateHash == hash.shoutState)
                {                
                    if (CalculatePathLength(player.transform.position) <= col.radius)                    
                        personalPlayerSighting = player.gameObject;
                }*/
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;
        }
    }

    float CalculatePathLength(Vector3 targetPosition)
    {        
        NavMeshPath path = new NavMeshPath();
        if (nav.enabled)
            nav.CalculatePath(targetPosition, path);
        
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
        
        allWayPoints[0] = transform.position;
        
        allWayPoints[allWayPoints.Length - 1] = targetPosition;
        
        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }
        
        float pathLength = 0;
        
        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
}
