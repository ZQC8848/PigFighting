using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NetcodePlus.Demo
{

    public class SuperGunPlayerUI : MonoBehaviour
    {
        public int player_id;
        public Text username;
        public SuperGunHPBar hp_bar;
        public Text lastLife;
        public Text kills;
        public PlayerIconChoose choose;
        void Start()
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            //username.gameObject.SetActive(false);
            //lastLife.gameObject.SetActive(false);
        }

        void Update()
        {
            if (!TheNetwork.Get().IsConnected())
                return;

            GameData gdata = GameData.Get();
            if (gdata == null)
                return;

            PlayerData player = gdata.GetPlayer(player_id);
            if (player == null)
                return;

            username.text = player.username.Substring(3);

            bool visible = player.connected;

            foreach(Transform child in transform)
            {
                if(child.gameObject.activeSelf != visible && child.name!= "Hpbar")
                    child.gameObject.SetActive(visible);
            }
            //if (username.gameObject.activeSelf != visible)
            //    username.gameObject.SetActive(visible);
            //if(lastLife.gameObject.activeSelf != visible)
            //    lastLife.gameObject.SetActive(visible);

            PlayerChoiceData choice = PlayerChoiceData.Get(GameMode.SuperGun, player.character);
            if (choice != null)
            {
                hp_bar.color = choice.color;
                choose.ChooseIcon(choice.id);
            }

            SuperGunPlayer gunPlayer = SuperGunPlayer.Get(player_id);
            if (gunPlayer != null)
            {
                //Debug.Log(gunPlayer.name);
                hp_bar.currentHealth = gunPlayer.hp;
                hp_bar.maxHealth = gunPlayer.hp_max;
                lastLife.text = gunPlayer.life.ToString();
                kills.text = gunPlayer.kills.ToString();
                //Debug.Log(hp_bar.currentHealth + " / " + hp_bar.maxHealth);
            }
            else
            {
                hp_bar.currentHealth = 0;
            }
        }
    }
}
