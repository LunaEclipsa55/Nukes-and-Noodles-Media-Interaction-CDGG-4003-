using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] public float attackRange = 0.8f;
    [SerializeField] public int damage = 10;
    [SerializeField] public float attackCooldown = 0.35f;
    [SerializeField] public LayerMask enemyLayer;

    [Header("Refs")]
    [SerializeField] public Transform attackPoint;      // assign your AttackPoint child here
    [SerializeField] public GameObject slashFXPrefab;   // assign your SlashFX prefab here

    float lastAttackTime;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown) // Left click
        {
            MeleeAttack();
        }
    }

    void MeleeAttack()
    {
        lastAttackTime = Time.time;

        // Instantiate slash effect at the attack point
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // Ensure the z-coordinate is zero for 2D
        Vector2 dir = (mouseWorldPos - transform.position).normalized;

        // place AttackPoint a bit in front of player toward the mouse
        attackPoint.position = transform.position + (Vector3)(dir * (attackRange * 0.9f));

        // Hit detection (small circle at AttackPoint)
        var hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Temporary visual slash
        if (slashFXPrefab != null)
        {
            var fx = Instantiate(slashFXPrefab, attackPoint.position, Quaternion.identity);
            // rotate visual to face mouse for clarity
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            fx.transform.rotation = Quaternion.Euler(0, 0, angle);
            Destroy(fx, 0.15f); // quick flash
        }
    }
}

