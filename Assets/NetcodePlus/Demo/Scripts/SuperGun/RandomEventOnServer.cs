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

        //superGunPlayer.eventData.RandomEventSeed = (int)DateTime.Now.Ticks;//set random seed

        gameTimer += Time.deltaTime;
        BoxTimer += Time.deltaTime;

        if (BoxTimer >= createBoxTime)
        {
            BoxTimer = 0;

            if (UnityEngine.Random.value < createBoxProbability)
            {
                UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
                int index = UnityEngine.Random.Range(0, 9)*100 + UnityEngine.Random.Range(1, initBoxPoints.Count);//INDEX START WITH 1 BUT NOT 0 !!! 
                Debug.Log(index);
                //9900-100 is eventNumber string and 99-1 is postion string
                superGunPlayer.eventData.indexOfItemBox = index;
            }
        }
    }

    public void CreateItemBox(int index)
    {
        GameObject itemBox =  Instantiate(ItemBox, initBoxPoints[index%100]);
        itemBox.transform.GetChild(0).GetComponent<ItemBox>().SetEventSerial(index / 100);
        superGunPlayer.eventData.indexOfItemBox = -1;
    }

    public void TriggerEvent(int index,SuperGunPlayer player)
    {
        if (index <= 4)
        {
            index = 1;
        }
        else
        {
            index = 2;
        }

        switch(index)
        {
            case 1:
                {
                    player.EquipWeapon("magicstick");
                }break;
            case 2:
                {
                    player.EquipWeapon("axe");
                }
                break;
        }
    }
}
