using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public BetterPmove player;
    //player health
    public int health;
    public GameObject[] Stages;

    public Image[] UIhealth; //이미지는 3개이므로 배열 
    public Text UIPoint;
    public Text UIStage;

    public GameObject UIRestartBtn;
    public GameObject UIMenuBtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {

        if (stageIndex < Stages.Length - 1)
        {   // 마지막 스테이지 아닌 경우 -> 다음스테이지로 
            UIStage.text = "STAGE " + (stageIndex + 1);
            Stages[stageIndex].SetActive(false);
            stageIndex++; //스테이지 증가 
            Stages[stageIndex].SetActive(true); //다음 스테이지 활성화

            PlayerReposition(); //시작위치에서 플레이어를 태어나게?하는 함수 
        }
        else
        { //마지막 스테이지인 경우 ->게임끝 

            //플레이어 컨트롤 막기 

            Time.timeScale = 0;

            Text btnText = UIRestartBtn.GetComponentInChildren<Text>();
            btnText.text = "GameClear! Play Again?";
            UIRestartBtn.SetActive(true);
            UIMenuBtn.SetActive(true);
        }


        //Calculate point
        totalPoint += stagePoint; // 얻은 지역포인트 전체점수에 포함시키기 
        stagePoint = 0; //지역 포인트 초기화
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {

            //체력감소
          

            if(health > 1)
            {
                //떨어진 위치에서 플레이어 재생성
                other.attachedRigidbody.velocity = Vector2.zero;
                other.transform.position = new Vector3(-7, -2, 0); //플레이어의 시작위치로 되돌아오기
            }

            //health down
            HealthDown();
            
        }
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.3f);
        }
        else
        {
            player.OnDie();
            Time.timeScale = 0;
           

            //UI 다시시작버튼 
            UIRestartBtn.SetActive(true);
            UIMenuBtn.SetActive(true);
            //죽었을 경우 모든 UI가 사라지도록 해야함 -> All Health UI Off
            UIhealth[0].color = new Color(1, 0, 0, 0.3f);
        }
    }

    void PlayerReposition()
    {

        player.VelocityZero();
        player.transform.position = new Vector3(-7, -2, 0); //플레이어의 시작위치로 되돌아오기
    }

    public void Restart()
    { //재시작이므로 처음부터 다시시작이라 scene 0번 
        Time.timeScale = 1; //플레이어가 다시 움직일 수 있도록 함 
        SceneManager.LoadScene(1);
    }

    public void GotoMain()
    { //재시작이므로 처음부터 다시시작이라 scene 0번 
        Time.timeScale = 1; //플레이어가 다시 움직일 수 있도록 함 
        SceneManager.LoadScene(0);
    }
}
