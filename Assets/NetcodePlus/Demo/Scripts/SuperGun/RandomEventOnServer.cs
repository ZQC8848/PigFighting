using NetcodePlus;
using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RandomEventOnServer : SNetworkObject
{
    public bool isServer;
    float gameTimer = 0;
    float createBoxTime = 10f;
    float createBoxProbability = 0.8f;

    bool doCreateBox;

    public GameObject ItemBox;
    public List<Transform> initBoxPoints;

    private SuperGunPlayerState sync_state = new SuperGunPlayerState();

    private SNetworkActions actions;
    protected override void Start()
    {
        base.Start();
        actions = new SNetworkActions();
        actions.Register("CreateBox", CreareItemBox);
        actions.RegisterSerializable("sync", ReceiveSync, NetworkDelivery.Unreliable);

        isServer = TheNetwork.Get().IsServer;
    }
    private void Update()
    {
        UpDateServerClint();
        UpdateOtherClint();
    }
    void UpDateServerClint()
    {
        if (!isServer)
            return;

        gameTimer += Time.deltaTime;

        sync_state.timing = sync_state.timing + 1;

        if (gameTimer > createBoxTime)
        {
            createBoxTime += gameTimer;
            if (Random.value < createBoxProbability)
            {
                doCreateBox = true;
                DoCreareItemBox(doCreateBox);
            }
        }
        sync_state.doCreateBox = doCreateBox;
        actions?.Trigger("sync", sync_state); // ReceiveSync(sync_state)
    }
    void UpdateOtherClint()
    {
        if (isServer)
            return;

        DoCreareItemBox(sync_state.doCreateBox);
    }
    private void ReceiveSync(SerializedData sdata)
    {
        if (IsOwner)
            return;

        SuperGunPlayerState state = sdata.Get<SuperGunPlayerState>();
        if (state.timing < sync_state.timing)
            return; //Old timing, ignore package, this means packages arrived in wrong order, prevent glitch

        sync_state = state;
    }
    void DoCreareItemBox(bool createBox)
    {
        if(createBox)
        {
            actions?.Trigger("CreateBox");
            doCreateBox = false;
        }
    }
    void CreareItemBox()
    {
        Transform initPoint = initBoxPoints[(int)Random.value * 10 % initBoxPoints.Count];

        Instantiate(ItemBox, initPoint);
    }

    [System.Serializable]
    public struct SuperGunPlayerState : INetworkSerializable
    {
        public ulong timing; //Increased by 1 each frame
        public bool doCreateBox;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref doCreateBox);
        }
    }
}
