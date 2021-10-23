using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMove : MonoBehaviour
{

    GameObject Player;
    BetterPmove betterPmove2;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        betterPmove2 = Player.GetComponent<BetterPmove>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    // for left moving button
    public void LeftBtnDown()
    {
        betterPmove2.LeftMove = true;
    }
    public void LeftBtnUp()
    {
        betterPmove2.LeftMove = false;
        betterPmove2.Idle = true;
    }
    // for right moving button
    public void RightBtnDown()
    {
        betterPmove2.RightMove = true;
    }

    public void RightBtnUp()
    {
        betterPmove2.RightMove = false;
        betterPmove2.Idle = true;
    }
    // for jumping button
    public void Jump()
    {
       
        betterPmove2.JumpMove = true;
        
    }


    public void JumpBtnUp()
    {
        betterPmove2.JumpMove = false;
    }
}