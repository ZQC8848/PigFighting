using NetcodePlus.Demo;
using Suriyun.MCS;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TouchControlManager : MonoBehaviour
{
    [Header("Android Button")]
    public FixedJoystick joystick;
    public ButtonPressHandler attackButton;
    public ButtonPressHandler skillButton;
    public ButtonPressHandler jumpButton;
    public ButtonPressHandler rushButton;

    public bool usingAndroidPlantform = true;
    public GameObject PCUI;
    public PlayerControl2D playerControl;

    public static TouchControlManager instance;

    private void Start()
    {
        InitControlObject();
        playerControl = PlayerControl2D.Get();
        instance = this;
    }
    public void InitControlObject()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            usingAndroidPlantform = false;
            this.gameObject.SetActive(false);
            PCUI.gameObject.SetActive(true);
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            usingAndroidPlantform = true;
            this.gameObject.SetActive(true);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            usingAndroidPlantform = true;
            this.gameObject.SetActive(true);
            PCUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {

        if (joystick == null || attackButton == null || skillButton == null || jumpButton == null)
        {
            Debug.Log("analogStick" + joystick.gameObject.name);
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
            playerControl.adJump = false;
            playerControl.adAttack = false;
            playerControl.adSkill = false;
            playerControl.adRush = false;
            playerControl.adFall = false;

            if (joystick.Horizontal >= 0.1)
                playerControl.adRightMove = true;

            if (joystick.Horizontal <= -0.1)
                playerControl.adLeftMove = true;

            if (joystick.Vertical < -0.5 && jumpButton.isButtonPressed)
            {
                playerControl.adFall = true;
            }

            if (attackButton.isButtonPressed)
                playerControl.adAttack = true;

            if (skillButton.isButtonPressed)
                playerControl.adSkill = true;


            if (jumpButton.isButtonPressed && joystick.Vertical > -0.5)
                playerControl.adJump = true;

            if (rushButton.isButtonPressed)
            {
                playerControl.adRush = true;
            }
        }
    }
}
