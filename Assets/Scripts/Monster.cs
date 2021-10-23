using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public float movePower;
    Animator animator;
    Vector3 movement;
    int movementFlag = 0;
    bool isTracing;
    GameObject traceTarget;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    CapsuleCollider2D capcollider;
    // Use this for initialization
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        capcollider = GetComponent<CapsuleCollider2D>();
        StartCoroutine("ChangeMovement");
    }

    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);

        if (movementFlag == 0)
            animator.SetBool("isWalking", false);
        else
            animator.SetBool("isWalking", true);

        yield return new WaitForSeconds(3f);

        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";

        if (isTracing)
        {
            Vector3 playerPos = traceTarget.transform.position;

            if (playerPos.x < transform.position.x)
                dist = "Left";
            else if (playerPos.x > transform.position.x)
                dist = "Right";
        }
        else
        {
            if (movementFlag == 1)
                dist = "Left";
            else if (movementFlag == 2)
                dist = "Right";
        }

        if (dist == "Left")
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        };

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            traceTarget = other.gameObject;

            StopCoroutine("ChangeMovement");
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            isTracing = true;
            animator.SetBool("isWalking", true);


        }


    }
    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            isTracing = false;
            StartCoroutine("ChangeMovement");
        }
    }

    public void OnDamaged()
    { //몬스터가 데미지를 입었을때 


        //Sprite Alpha : 색상 변경 
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Sprite Flip Y : 뒤집어지기 
        spriteRenderer.flipY = true;

        //Collider Disable : 콜라이더 끄기 
        capcollider.enabled = false;

        //Die Effect Jump : 아래로 추락(콜라이더 꺼서 바닥밑으로 추락함 )
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Destroy 
        Invoke("DeActive", 5);

    }

    void DeActive()
    { //오브젝트 끄기 
        gameObject.SetActive(false);
    }
}