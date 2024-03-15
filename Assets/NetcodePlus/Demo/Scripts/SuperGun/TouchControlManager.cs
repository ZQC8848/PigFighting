using NetcodePlus.Demo;
using Suriyun.MCS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlManager : MonoBehaviour
{
    [Header("Android Button")]
    public AnalogStick analogStick;
    public UniversalButton attackButton;
    public UniversalButton skillButton;
    public UniversalButton jumpButton;
    public UniversalButton rushButton;

    public bool usingAndroidPlantform = true;

    public PlayerControl2D playerControl;

    private void Start()
    {
        InitControlObject();
        playerControl = PlayerControl2D.Get();
    }
    public void InitControlObject()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer )
        {
            usingAndroidPlantform = false;
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (analogStick == null || attackButton == null || skillButton == null || jumpButton == null)
        {
            Debug.Log("analogStick" + analogStick.gameObject.name);
            Debug.Log("attackButton" + attackButton.gameObject.name);
            Debug.Log("skillButton" + skillButton.gameObject.name);
            Debug.Log("jumpButton" + jumpButton.gameObject.name);
            //Debug.Log("button need to be set!");
            return;
        }

        if (usingAndroidPlantform)
        {
            //Debug.Log("using contorl!!");
            //Reset Control
            //playerControl.adRightMove = false;
            //playerControl.adLeftMove = false;
            //playerControl.adFall = false;
            //playerControl.adJump = false;
            playerControl.adAttack = false;
            playerControl.adSkill = false;

            if (analogStick.horizontal >= 0.1)
                playerControl.adRightMove = true;

            if (analogStick.horizontal <= -0.1)
                playerControl.adLeftMove = true;

            if (analogStick.vertical <= -0.5 && jumpButton.isFingerDown)
                playerControl.adFall = true;

            if (attackButton.isFingerDown)
                playerControl.adAttack = true;

            if (skillButton.isFingerDown)
                playerControl.adSkill = true;

            if (jumpButton.isFingerDown && analogStick.vertical > -0.5)
                playerControl.adJump = true;

        }
    }
}
