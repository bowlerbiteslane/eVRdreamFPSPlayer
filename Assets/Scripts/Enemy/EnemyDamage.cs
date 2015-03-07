using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour {

    public float damageModifier = 1f;
    public float resistChance = 0f;

    EnemyHealth enemyHealth;

    void Awake() {
        enemyHealth = transform.root.GetComponent<EnemyHealth>();
        if (enemyHealth == null) {            
            this.enabled = false;
        }
    }

    public void TakeDamage(float damage) {
        enemyHealth.health -= damage * damageModifier;
    }
}
