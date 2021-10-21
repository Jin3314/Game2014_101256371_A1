using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterPmove : MonoBehaviour
{


    Animator animator;
    public bool LeftMove = false;
    public bool RightMove = false;
    public bool JumpMove = false;
    public bool JumpMoving = false;
    public float jumpPower = -10.0f;
    Vector3 moveVelocity = Vector3.zero;
    float moveSpeed = 4; //버튼을 누르는 동안에 오브젝트의 움직이는 속도
    SpriteRenderer spriteRenderer;

    public bool isJumping = false;
    public int jumpCount;
    public bool isGround;
   

    Rigidbody2D rigid;

    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();

        jumpCount = 1; //점프 가능횟수

        isGround = true; //땅에 있을때

    }

    // Update is called once per frame
    void Update()
    {
        
            if (isGround)
            {
                jumpCount = 1;
                if (JumpMove)
                {
                    if (jumpCount == 1)
                    {
                        rigid.velocity = Vector2.zero;

                        Vector2 jumpVelocity = new Vector2(0, jumpPower);
                        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

                        isGround = false;

                        jumpCount = 0;
                    }


                }

            }

        
       
        if (LeftMove)
        {
            
            moveVelocity = new Vector3(-0.30f, 0, 0);
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
            spriteRenderer.flipX = true;
        }
        if (RightMove)
        {
     
            moveVelocity = new Vector3(+0.30f, 0, 0);
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
            spriteRenderer.flipX = false;
        }
        if (moveSpeed < 0.2)
            animator.SetBool("isWalking", false);
        else
            animator.SetBool("isWalking", true);

    }

    private void OnCollisionEnter2D(Collision2D col)

    {

        if (col.gameObject.tag == "Ground")

        {
            Debug.Log("Grounded");
            isGround = true;    //Ground에 닿으면 isGround는 true

            jumpCount = 1;          //Ground에 닿으면 점프횟수가 1로 초기화됨

        }

    }
}