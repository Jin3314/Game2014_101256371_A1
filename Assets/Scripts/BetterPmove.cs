using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterPmove : MonoBehaviour
{
    //variables
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;
    public bool LeftMove = false;
    public bool RightMove = false;
    public bool JumpMove = false;
    public bool JumpMoving = false;
    public bool Idle = false;
    public float jumpPower = -10.0f;
    float moveSpeed = 4; 
    public bool isJumping = false;
    public int jumpCount;
    public bool isGround;

    public GameManager gameManager;

    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capcollider;
    Vector3 moveVelocity = Vector3.zero;
    AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        capcollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        jumpCount = 1; //jump count

        isGround = true; //if player is on the ground

    }

    // Update is called once per frame
    void Update()
    {
        //jump move
        if (isGround)
        {
            jumpCount = 1;
            if (JumpMove)
            {
                if (jumpCount == 1)
                {
                    audioSource.clip = audioJump;
                    audioSource.Play();

                    rigid.velocity = Vector2.zero;

                    Vector2 jumpVelocity = new Vector2(0, jumpPower);
                    rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

                    isGround = false;

                    jumpCount = 0;
                }


            }

        }
        //for idle state & animation
        if (Idle)
        {
            moveVelocity = new Vector3(0, 0, 0);
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
            animator.SetBool("isWalking", false);
        }
        //for moving left
        if (LeftMove)
        {

            moveVelocity = new Vector3(-0.90f, 0, 0);
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
            spriteRenderer.flipX = true;
            animator.SetBool("isWalking", true);
        }
        //for moving right
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
        //collision for jump
        if (col.gameObject.tag == "Ground")

        {

            isGround = true;    

            jumpCount = 1;          

        }
        //collision for enemy
        if (col.gameObject.tag == "Enemy")
        {
            if (rigid.velocity.y < 0 && transform.position.y > col.transform.position.y)
            {
       
                OnAttack(col.transform); //function for attacking
            }
            else
            { //for player getting damage
                OnDamaged(col.transform.position);   
            }
        }


    }
    //player damaged funtion, activates invincibility for 2 seconds
    void OnDamaged(Vector2 tartgetPos)
    {
        audioSource.clip = audioDamaged;
        audioSource.Play();

        gameManager.HealthDown();

        gameObject.layer = 11; 

        spriteRenderer.color = new Color(1, 1, 1, 0.4f); 

        //motion for rebound
        int dirc = transform.position.x - tartgetPos.x > 0 ? 1 : -1;
        
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse); 


        Invoke("OffDamaged", 2); //2 sceconds of invincibility


    }
    //disables invincibility
    void OffDamaged()
    { 
        gameObject.layer = 10;

        spriteRenderer.color = new Color(1, 1, 1, 1); 

    }
    //player attack funtion
    void OnAttack(Transform enemy)
    {
        audioSource.clip = audioAttack;
        audioSource.Play();
        //Point 
        gameManager.stagePoint += 100;

        //Reaction Force 
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Enemy Die
        
        Monster enemyMove = enemy.GetComponent<Monster>();
        enemyMove.OnDamaged(); 


    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "item")
        {
            audioSource.clip = audioItem;
            audioSource.Play();
            //Point
          
            bool isBronze = other.gameObject.name.Contains("Bronze");
            bool isSilver = other.gameObject.name.Contains("Silver");
            bool isGold = other.gameObject.name.Contains("Gold");

            if (isBronze)
                gameManager.stagePoint += 50;
            else if (isSilver)
                gameManager.stagePoint += 100;
            else if (isGold)
                gameManager.stagePoint += 300;

            
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "finish")
        {
            audioSource.clip = audioFinish;
            audioSource.Play();
            //Next Stage
            gameManager.NextStage();

        }
    }

    public void OnDie()
    { //dying effect
        audioSource.clip = audioDie;
        audioSource.Play();
        //Sprite Alpha 
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Sprite Flip Y 
        spriteRenderer.flipY = true;

        //Collider Disable 
        capcollider.enabled = false;

        //Die Effect Jump 
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);


    }
    public void VelocityZero()
    { //makes velocity to zero
        rigid.velocity = Vector2.zero;
    }
}

