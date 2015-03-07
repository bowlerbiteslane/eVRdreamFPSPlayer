using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public float attackTime = 2.0f;
    public float attackDamage = 10f;
    public float enemyAwarenessResetTime = 10.0f;
    public AudioClip enemyShout;

    EnemySight enemySight;
    NavMeshAgent nav;
    Vector3 sightingDeltaPos;
    EnemyHealth enemyHealth;
    GameObject player;
    SphereCollider col;
    float attackTimer;
    bool canIncreasedSight = true;
    bool canDecreasedSight = false;
    float enemyAwarenessTimer;    

    void Awake()
    {
        attackTimer = 0.0f;
        enemyAwarenessTimer = 0.0f;

        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
        col = GetComponent<SphereCollider>();
    }

	void Update () {
        //FIX ME - replace it for a switch later
        if(enemyHealth.health <= 0){
            nav.Stop();
        }
        else if (enemySight.playerInSight)
        {
            EnemySightRadiusManager();

            sightingDeltaPos = enemySight.personalPlayerSighting.transform.position - transform.position;

            if (sightingDeltaPos.sqrMagnitude > 4f)
                MoveToPosition(enemySight.personalPlayerSighting.transform.position);
            else
                Attack();
        }
        else if (enemyHealth.enemyInjuredAlert)
        {
            enemyHealth.enemyInjuredAlert = false;

            EnemySightRadiusManager();
            MoveToPosition(player.transform.position);
        }
        else if (canDecreasedSight)
        {
            enemyAwarenessTimer += Time.deltaTime;
            if (enemyAwarenessResetTime < enemyAwarenessTimer)
            {
                enemyAwarenessTimer = 0;
                col.radius /= 2;

                canIncreasedSight = !canIncreasedSight;
                canDecreasedSight = !canDecreasedSight;                
            }
        }
	}

    void MoveToPosition(Vector3 position)
    {
        nav.SetDestination(position);
    }

    void Attack()
    {
        nav.Stop();
        attackTimer += Time.deltaTime;

        if (attackTimer > attackTime)
        {
            attackTimer = 0;
            enemySight.personalPlayerSighting.GetComponent<HealthManager>().TakeDamage(attackDamage);
        }
    }

    void EnemySightRadiusManager()
    {
        if (canIncreasedSight)
        {
            col.radius *= 2;
            canIncreasedSight = !canIncreasedSight;
            canDecreasedSight = !canDecreasedSight;
            AudioSource.PlayClipAtPoint(enemyShout, transform.position);
        }
    }

    public void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 200, 100), "Zombie Health: " + enemyHealth.health.ToString());
    }
}
