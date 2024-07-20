using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieBehavior : MonoBehaviour
{
    public Transform target;
    public float detectionRadius = 20f;
    public float attackRange = 2f;
    public float speed = 1f;
    public float rotationSpeed = 5f;
    public float wanderRadius = 20f;
    public float wanderTimer = 5f;

    private Animator animator;
    private float timer;
    private Vector3 wanderTarget;

    public GameObject healthBarPrefab;
    private GameObject healthBar;
    public float maxHealth = 100;
    private float currentHealth = 100;
    private bool isDead = false;
    private bool isAttacking = false;
    void Start()
    {
        if (target == null)
        {
            GameObject vrCharacter = GameObject.Find("PhysicsRig");
            if (vrCharacter != null)
            {
                XROrigin xrOrigin = vrCharacter.GetComponentInChildren<XROrigin>();
                if (xrOrigin != null)
                {
                    target = xrOrigin.transform;
                }
                else
                {
                    Debug.LogError("XR Origin not found in the PhysicsRig!");
                    return;
                }
            }
            else
            {
                Debug.LogError("PhysicsRig not found in the scene!");
                return;
            }
        }

        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        timer = wanderTimer;
        InitializeHealthBar();
        SetInitialRandomDestination();
    }

    void SetInitialRandomDestination()
    {
        wanderTarget = RandomNavSphere(transform.position, wanderRadius, -1);
    }

    void Update()
    {
        if (target == null || isDead) return;

        timer += Time.deltaTime;
        float distance = Vector3.Distance(target.position, transform.position);

        if (!isAttacking && !isDead)
        {
            if (distance <= attackRange)
            {
                AttemptAttack();
            }
            else
            {
                if (timer >= wanderTimer || distance > detectionRadius)
                {
                    SetInitialRandomDestination();
                    timer = 0;
                }
                MoveTowardsTarget(wanderTarget);
            }
        }

        if (distance <= detectionRadius && !isDead)
        {
            if (distance > attackRange)
            {
                MoveTowardsTarget(target.position);
                animator.Play("Z_Walk_InPlace");
            }
            else
            {
                if (!isAttacking)
                {
                    animator.Play("Z_Attack");
                    isAttacking = true;

                    ApplyDamageToPlayer();
                    StartCoroutine(ResetAttack());
                }
            }
        }
    }

    void AttemptAttack()
    {
        animator.Play("Z_Attack");
        isAttacking = true;
        ApplyDamageToPlayer();
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ResetAttack());
        }
    }


    private void ApplyDamageToPlayer()
    {
        if (target != null && Vector3.Distance(target.position, transform.position) <= attackRange)
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                AudioManager.instance.Play("bite");
                playerHealth.TakeDamage(20);
                PlayerDamageEffect damageEffect = target.GetComponent<PlayerDamageEffect>();
                if (damageEffect != null)
                {
                    damageEffect.TriggerDamageEffect();
                }
            }
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

    void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 moveDirection = targetPosition - transform.position;
        moveDirection.y = 0;
        if (Vector3.Distance(transform.position, targetPosition) > attackRange)
        {
            moveDirection.Normalize();
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed * Time.deltaTime);
        }
    }

    void UpdateHealthBarPosition()
    {
        if (healthBar != null)
        {
            healthBar.transform.position = transform.position + new Vector3(0, 2.5f, 0);
            healthBar.transform.LookAt(Camera.main.transform);
        }
    }

    public void InitializeHealthBar()
    {
        if (healthBarPrefab && healthBar == null)
        {
            healthBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity, transform);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        AudioManager.instance.Play("zombie-die");
        animator.Play("Z_FallingBack");
        DisableZombie();
    }

    void DisableZombie()
    {
        GetComponent<Collider>().enabled = false;
        if (GetComponent<NavMeshAgent>() != null)
            GetComponent<NavMeshAgent>().enabled = false;

        if (healthBar != null)
        {
            Destroy(healthBar);
        }
        Destroy(gameObject, 3);
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            Slider healthSlider = healthBar.GetComponentInChildren<Slider>();
            if (healthSlider != null)
            {
                healthSlider.value = CalculateHealthPercentage();
            }
        }
    }

    float CalculateHealthPercentage()
    {
        return 100.0f * currentHealth / maxHealth;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randDirection, out navHit, dist, layermask))
        {
            return navHit.position;
        }
        return origin;
    }
}
