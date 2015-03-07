using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public float health = 100f;
    public bool enemyInjuredAlert;
    public AudioClip deathClip;

    float previousHealth;
    Animator anim;
    bool enemyDead;
    float disappearTimer = 0f;
    float disappearTime = 5.0f;
    HashIDs hash;
    

	// Use this for initialization
	void Awake() {
        previousHealth = health;
        enemyInjuredAlert = false;
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
	}
	
	// Update is called once per frame
	void Update () {

        if (previousHealth > health)
        {
            previousHealth = health;
            enemyInjuredAlert = true;  //Set Flag injured
        }

        if (health <= 0)
        {
            EnemyDying();

            if (!enemyDead)
            {
                EnemyDying();
            }
            else
            {
                EnemyDead();
            }
        }        
	}

    void EnemyDying()
    {
        if(!enemyDead)
            AudioSource.PlayClipAtPoint(deathClip, transform.position);

        //Holdplacer for anim.SetBool(hash.deadBool", enemyDead);
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 90));

        enemyDead = true;        
    }

    void EnemyDead()
    {
        //Holdplacer for if(anim.GetCurrentAnimatorStateInfo(0).nameHash == 1) anim.SetBool(hash.deadBool, false);
        disappearTimer += Time.deltaTime;
        if (disappearTimer > disappearTime)
            Destroy(this.gameObject);
    }
}
