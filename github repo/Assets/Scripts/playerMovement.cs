using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    float runSpeedCopy;

    Vector2 moveInput;//karakterin hareketi hakkýnda alýk bilgi
    Rigidbody2D rb2d;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;

    public Transform groundPos;
    private bool isGrounded;
    public float checkRadius;
    public LayerMask whatIsGround;

    private bool attacked;

    public Transform attackPoint;
    public int attackDamage = 20;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        runSpeedCopy = runSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {    
        Run();
        FlipSprite();//karakter hareket ederken bakýþ yönünü ayarlama

        isGrounded = Physics2D.OverlapCircle(groundPos.position, checkRadius, whatIsGround);

        if (isGrounded == true)
        {
            myAnimator.SetBool("isJumping", false);

        }
        else
        {
            myAnimator.SetBool("isJumping", true);       
        }
    }

    void OnFire(InputValue value)//saldýr
    {
        if (attacked == false)
        {
            Attack();
            
        }   
    }

    void Attack()
    {  
        myAnimator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<enemyBehaviour>().TakeDamage(attackDamage);
        }

        attacked = true;
        runSpeed = 0;
        StartCoroutine(changeAttackedState());
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public IEnumerator changeAttackedState()//ard arda saldýrý yapmayý engelleme
    {
        yield return new WaitForSeconds(0.5f);//þu kadar saniye sonra bunlarý yap
        attacked = false;
        runSpeed = runSpeedCopy;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))//eðer yerde deðilsem bunu yapma 
        {       
            return;
        }

        

        if (value.isPressed)
        {
            rb2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnSpecialAttack1(InputValue value)
    {
        if (value.isPressed)
        {
            myAnimator.SetTrigger("Special");
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb2d.velocity.y);// aþaðý yukarý gitmeyi engelliyo
        rb2d.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
        }
    }

    public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.gameObject.transform.position = animator.transform.position;
    }






}
