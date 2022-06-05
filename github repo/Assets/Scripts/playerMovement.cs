using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;

    Vector2 moveInput;//karakterin hareketi hakkýnda alýk bilgi
    Rigidbody2D rb2d;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();//karakter hareket ederken bakýþ yönünü ayarlama
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
            


            //if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))//eðer yerde deðilsem bunu yapma 
            //{
            //    myAnimator.SetBool("isJumping", true);
            //}
            //else
            //{
            //    myAnimator.SetBool("isJumping", false);
            //}
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





}
