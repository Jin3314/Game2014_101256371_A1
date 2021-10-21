using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;     //최대속도변수 선언
    Rigidbody2D rigid;  //Rigidbody2D -변수명 rigid 선언 
    SpriteRenderer spriteRenderer;
    Animator anim;

    //mobile key value
    int left_Value;
    int right_Value;
    bool left_Down;
    bool right_Down;
    bool left_Up;
    bool right_Up;
    float h;
    bool isHorizonMove;
    Vector3 dirVec;
    GameObject scanObject;


    public float movePower = 1f;
    public float jumpPower = 1f;

    Vector3 movement;

    void Awake()
    {
   
        rigid = GetComponent<Rigidbody2D>();    //rigid 변수 초기화
        maxSpeed = 3f;              //최대속도
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
          
    }

    void Update()
    {
       
       if (Input.GetButtonUp("Horizontal"))
       {
           rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
           //키를 떼면,x축 속도 기본 0.5배, y축 속도는 그대로
       }   
       if (Input.GetButton("Horizontal"))
       {
           spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
           //키를 누르고 있으면, 왼쪽누르면 -1되서 좌우바꾸기
       } 
        //animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

 
    }

    void FixedUpdate()
    {
        leftmove();
        rightmove();
        float h = Input.GetAxisRaw("Horizontal");       //h에 키를 누르면 입력 오른쪽=1,왼쪽=-1
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse); //h * 오른쪽곱해서 힘을 줌

        if (rigid.velocity.x > maxSpeed)         //x속도가 maxSpeed 보다 크면, 속도 maxSpeed로 고정
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))       //x속도가 -maxSpeed 보다 작으면(왼쪽으로 갈때) 속도는 -maxSpeed로 고정
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }

    
      public void ButtonDown(string type)
    {
      
        switch (type)
        {
            case "L":
                leftmove();
                break;
            case "R":
                rightmove();
                break;
        }
    }
     
    
     
     
     
    
   

    void leftmove()
    {
        Vector3 moveVelocity = Vector3.zero;

                moveVelocity = Vector3.left;
                transform.position += moveVelocity * movePower * Time.deltaTime;
              
               
    }

    void rightmove()
    {
        Vector3 moveVelocity2 = Vector3.zero;

        moveVelocity2 = Vector3.right;
        transform.position += moveVelocity2 * movePower * Time.deltaTime;
        
    }

   
}