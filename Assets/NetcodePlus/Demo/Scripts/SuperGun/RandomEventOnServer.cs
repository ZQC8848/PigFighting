using NetcodePlus;
using NetcodePlus.Demo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class RandomEventOnServer : SNetworkObject
{
    public bool isServer;
    float gameTimer = 0;
    float createBoxTime = 5f;
    float BoxTimer = 0;
    float createBoxProbability = 1f;

    public GameObject ItemBox;
    public List<Transform> initBoxPoints;

    public SuperGunPlayer superGunPlayer;

    static public RandomEventOnServer instance;
    protected override void Start()
    {
        Invoke("LateInitData", 1);
        
        isServer = TheNetwork.Get().IsServer;
        instance = this;
    }
    void LateInitData()
    {
        GameData gdata = GameData.Get();
        PlayerData player = gdata.GetPlayer(TheNetwork.Get().PlayerID);
        Debug.Log("playerId=" + player.player_id);
        superGunPlayer = SuperGunPlayer.Get(player.player_id);
        if (superGunPlayer == null)
        {
            Debug.Log("player is null");
        }
        else
        {
            Debug.Log("playerId=" + player.player_id);
        }
    }
    private void Update()
    {
        UpDateServerClint();
    }
    void UpDateServerClint()
    {
        if (!isServer)
            return;

        gameTimer += Time.deltaTime;
        BoxTimer += Time.deltaTime;

        if (BoxTimer >= createBoxTime)
        {
            BoxTimer = 0;

            if (UnityEngine.Random.value < createBoxProbability)
            {
                int index = UnityEngine.Random.Range(1, initBoxPoints.Count);//INDEX START WITH 1 BUT NOT 0 !!! 
                superGunPlayer.eventData.indexOfItemBox = index;
            }
        }
    }

    public void CreateItemBox(int index)
    {
        Instantiate(ItemBox, initBoxPoints[index]);
        superGunPlayer.eventData.indexOfItemBox = -1;
    }
}
