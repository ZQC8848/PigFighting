using NetcodePlus;
using NetcodePlus.Demo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class StartGame : MonoBehaviour
{
    Transform thisTransform;
    public bool GameStart;
    public GameObject death;
    public DoMoveCurtain left;
    public DoMoveCurtain right;
    void Start()
    {
        thisTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStart)
            return;

        if (thisTransform.rotation.z < 0)
        {
            GameStart = true;
            StartGameFunc();
        }
        Debug.Log("thisTransform.rotation.z = " + thisTransform.rotation.z);
    }
    void StartGameFunc()
    {
        AudioSystem.Get().PlaySound("战斗背景音", true);
        AudioSystem.Get().StopSound("准备房间");

        GameData gdata = GameData.Get();
        if (gdata == null)
            return;

        if (!TheNetwork.Get().IsReady())
            return;

        PlayerData player = gdata.GetPlayer(TheNetwork.Get().PlayerID);
        if (player != null)
        {
            foreach (PlayerData aplayer in gdata.players)
            {
                SuperGunPlayer _superGunPlayer = SuperGunPlayer.Get(aplayer.player_id);
                if (_superGunPlayer != null && aplayer.IsAssigned())
                {
                    _superGunPlayer.ChangInvincibleState();
                    _superGunPlayer.life++;
                }
            }
        }
        death.SetActive(true);
        right.DoMoveObj();
        left.DoMoveObj();
    }
}
