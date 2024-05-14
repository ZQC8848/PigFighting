using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetcodePlus.Demo
{
    public class SuperGunGame : SMonoBehaviour
    {
        private SNetworkActions actions;

        private bool ended = false;
        public List<SuperGunPlayer> playerDataList;
        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;

            if (!TheNetwork.Get().IsActive())
            {
                //Start in test mode, when running directly from Unity Scene
                Authenticator.Get().LoginTest("Player"); //May not work with more advanced auth system, works in Test mode
                DemoConnectData cdata = new DemoConnectData(GameMode.SuperGun);
                TheNetwork.Get().SetConnectionExtraData(cdata);
                TheNetwork.Get().StartHost(NetworkData.Get().game_port);
            }
        }
        void Start()
        {
            //Shuffle PlayerSpawn ids
            if (TheNetwork.Get().IsServer)
            {
                List<PlayerSpawn> spawns = PlayerSpawn.GetAll();
                ListTool.Shuffle(spawns);
                for (int i = 0; i < spawns.Count; i++)
                    spawns[i].player_id = i;
            }

            BlackPanel.Get().Show(true);
        }
        protected override void OnReady()
        {
            actions = new SNetworkActions(1); //Use NetworkID 1 for the manager
            actions.RegisterRefresh("data", DoRefresh);
            BlackPanel.Get().Hide();
        }
        protected override void OnClientReady(ulong client_id)
        {
            //Refresh data to other players
            RefreshData();
        }

        void Update()
        {
            GameData gdata = GameData.Get();
            if (gdata == null)
                return;

            if (!TheNetwork.Get().IsReady())
                return;

            //Check for endgame
            if (!ended)
            {
                PlayerData player = gdata.GetPlayer(TheNetwork.Get().PlayerID);
                if (player != null)
                {
                    SuperGunPlayer superGunPlayer = SuperGunPlayer.Get(player.player_id);
                    if (superGunPlayer != null && superGunPlayer.life <= 0)
                    {
                        ended = true;
                        GameOverPanel.Get().Show("你输了", "准备上桌");
                    }

                    int count_all = 0;
                    int count_alive = 0;

                    foreach (PlayerData aplayer in gdata.players)
                    {
                        SuperGunPlayer _superGunPlayer = SuperGunPlayer.Get(aplayer.player_id);
                        if (_superGunPlayer != null && aplayer.IsAssigned())
                        {
                            playerDataList.Add(_superGunPlayer);
                            count_all += 1;
                            if (!_superGunPlayer.Ondie)
                                count_alive += 1;
                        }
                    }

                    if (superGunPlayer != null && superGunPlayer.life > 0 && count_all >= 2 && count_alive == 1)
                    {
                        ended = true;
                        GameOverPanel.Get().Show("你赢了", "准备吃席");
                    }
                }
            }
        }
        public void RefreshData()
        {
            byte[] bytes = NetworkTool.Serialize(GameData.Get());
            actions?.Refresh("data", bytes); //DoRefresh();
        }

        private void DoRefresh(byte[] sdata)
        {
            GameData gdata = NetworkTool.Deserialize<GameData>(sdata);
            GameData.Override(gdata);
        }
    }
}

