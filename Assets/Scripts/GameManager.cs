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
    //variable for UI
    public Image[] UIhealth; 
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
        {   // If it is not a last stage -> move to next stage 
            UIStage.text = "STAGE " + (stageIndex + 1);
            Stages[stageIndex].SetActive(false);
            stageIndex++; //move to next stage 
            Stages[stageIndex].SetActive(true); //activate next stage

            PlayerReposition(); //player respawn function
        }
        else
        { //If it is last stage ->ends game 

            //blocks player control
            Time.timeScale = 0;

            Text btnText = UIRestartBtn.GetComponentInChildren<Text>();
            btnText.text = "GameClear! Play Again?";
            UIRestartBtn.SetActive(true);
            UIMenuBtn.SetActive(true);
        }


        //Calculate point
        totalPoint += stagePoint; // add local's point to whole score
        stagePoint = 0; //reset local point
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {

 
          

            if(health > 1)
            {
                //respawn player
                other.attachedRigidbody.velocity = Vector2.zero;
                other.transform.position = new Vector3(-7, -2, 0); 
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
           

            //UI restart button
            UIRestartBtn.SetActive(true);
            UIMenuBtn.SetActive(true);
            //All Health UI Off if player is dead
            UIhealth[0].color = new Color(1, 0, 0, 0.3f);
        }
    }

    void PlayerReposition()
    {

        player.VelocityZero();
        player.transform.position = new Vector3(-7, -2, 0); //respawn
    }

    public void Restart()
    { //restart function
        Time.timeScale = 1; //player can move
        SceneManager.LoadScene(1);
    }

    public void GotoMain()
    { //go to main function
        Time.timeScale = 1; //player can move
        SceneManager.LoadScene(0);
    }
}
