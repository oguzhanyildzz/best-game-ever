using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehaviour : MonoBehaviour
{
    
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;

    public int maxHealth = 1000;
    int currentHealth;

    BoxCollider2D myEnemyBoxCollider;


    
    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool cooling;
    private float intTimer;

    public Transform HitBox;
    public int attackDamage = 10;
    public LayerMask playerLayer;



    private void Awake()
    {
        SelectTarget();
        intTimer = timer;
        anim = GetComponent<Animator>();
        myEnemyBoxCollider = GetComponent<BoxCollider2D>();
        
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if (!InsideofLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("isAttack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            EnemyLogic();
        }

        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
            moveSpeed = 0;
        }
    }


    void Die()
    {
        anim.SetBool("isDead", true);
        this.enabled = false;
        StartCoroutine(DestroyEnemy());
    }

    public IEnumerator DestroyEnemy()//ard arda saldýrý yapmayý engelleme
    {
        yield return new WaitForSeconds(2f);//þu kadar saniye sonra bunlarý yap
        Destroy(gameObject);
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if(distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("isAttack", false);
        }
    }

    void Move()
    {
        anim.SetBool("canWalk", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("isAttack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = intTimer;
        attackMode = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(HitBox.position, attackDistance, playerLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<playerMovement>().TakeDamageCharacter(attackDamage);
        }


        anim.SetBool("canWalk", false);
        anim.SetBool("isAttack", true);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    public void StopAttack()
    {
        cooling = false;
        attackMode=false;
        anim.SetBool("isAttack", false); 
    }

    public void TriggerCooling()
    {
        cooling=true;
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if (distanceToLeft < distanceToRight)
        {
            target = rightLimit;
        }
        else
        {
            target = leftLimit;
        }

        Flip();

    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }

        transform.eulerAngles = rotation;
    }
}
