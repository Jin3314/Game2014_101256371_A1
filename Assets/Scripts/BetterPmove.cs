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
    public bool Idle = false;
    public float jumpPower = -10.0f;
    Vector3 moveVelocity = Vector3.zero;
    float moveSpeed = 4; //버튼을 누르는 동안에 오브젝트의 움직이는 속도
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capcollider;
    public bool isJumping = false;
    public int jumpCount;
    public bool isGround;

    public GameManager gameManager;

    Rigidbody2D rigid;

    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        capcollider = GetComponent<CapsuleCollider2D>();
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

        if (Idle)
        {
            moveVelocity = new Vector3(0, 0, 0);
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
            animator.SetBool("isWalking", false);
        }

        if (LeftMove)
        {

            moveVelocity = new Vector3(-0.90f, 0, 0);
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
            spriteRenderer.flipX = true;
            animator.SetBool("isWalking", true);
        }
        if (RightMove)
        {

            moveVelocity = new Vector3(+0.90f, 0, 0);
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
            spriteRenderer.flipX = false;
            animator.SetBool("isWalking", true);
        }


    }

    private void OnCollisionEnter2D(Collision2D col)

    {

        if (col.gameObject.tag == "Ground")

        {

            isGround = true;    //Ground에 닿으면 isGround는 true

            jumpCount = 1;          //Ground에 닿으면 점프횟수가 1로 초기화됨

        }

        if (col.gameObject.tag == "Enemy")
        {
            if (rigid.velocity.y < 0 && transform.position.y > col.transform.position.y)
            {
                OnAttack(col.transform); //적을 공격하는 함수
            }
            else
            { // 밟는 모션이 아닌 적과의 충돌-> 데미지를 플레이어가 받음(데미지 받는함수) 
                OnDamaged(col.transform.position); //현재 충돌한 오브젝트의 위치값을 넘겨줌  
            }
        }


    }

    void OnDamaged(Vector2 tartgetPos)
    {
        gameManager.HealthDown();

        gameObject.layer = 11; //playerDamaged Layer number가 11로 지정되어있음 

        spriteRenderer.color = new Color(1, 1, 1, 0.4f); //투명도를 0.4로 부여하여 지금이 무적시간으로 변경되었음을 보여줌

        //맞으면 튕겨나가는 모션
        int dirc = transform.position.x - tartgetPos.x > 0 ? 1 : -1;
        //튕겨나가는 방향지정 -> 플레이어 위치(x) - 충돌한 오브젝트위치(x) > 0: 플레이어가 오브젝트를 기준으로 어디에 있었는지 판별
        //> 0이면 1(오른쪽으로 튕김) , <=0 이면 -1 (왼쪽으로 튕김)
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse); // *7은 튕겨나가는 강도를 의미 


        Invoke("OffDamaged", 2); //2초의 딜레이 (무적시간 2초)

       
    }

    void OffDamaged()
    { //무적해제함수 
        gameObject.layer = 10; //플레이어 레이어로 복귀함

        spriteRenderer.color = new Color(1, 1, 1, 1); //투명도를 1로 다시 되돌림 

    }

    void OnAttack(Transform enemy)
    {

        //Point 점수 올리기
        gameManager.stagePoint += 100;

        //Reaction Force : 반동(플레이어가 튕겨져나감)
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Enemy Die
        //몬스터에 적용한 스크립트의 함수를 사용하기위해 해당 클래스의 변수를 선언해서 초기화
        Monster enemyMove = enemy.GetComponent<Monster>();
        enemyMove.OnDamaged(); // 몬스터가 데미지를 입었을때 실행할 함수를 불러옴 


    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "item")
        {
            //Point
            //포인트를 얻음
            bool isBronze = other.gameObject.name.Contains("Bronze");
            bool isSilver = other.gameObject.name.Contains("Silver");
            bool isGold = other.gameObject.name.Contains("Gold");

            if (isBronze)
                gameManager.stagePoint += 50;
            else if (isSilver)
                gameManager.stagePoint += 100;
            else if (isGold)
                gameManager.stagePoint += 300;

            //동전 사라지기(비활성화)
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "finish")
        {
            //Next Stage
            gameManager.NextStage();

        }
    }

    public void OnDie()
    { //죽음 effect(외적인거)

        //Sprite Alpha : 색상 변경 
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Sprite Flip Y : 뒤집어지기 
        spriteRenderer.flipY = true;

        //Collider Disable : 콜라이더 끄기 
        capcollider.enabled = false;

        //Die Effect Jump : 아래로 추락(콜라이더 꺼서 바닥밑으로 추락함 )
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);


    }
    public void VelocityZero()
    { //속력을 0으로 만드는 함수 
        rigid.velocity = Vector2.zero;
    }
}

