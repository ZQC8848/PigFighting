using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NetcodePlus.Demo;
public class UIPanelAbovePlayer : MonoBehaviour
{
    public SuperGunPlayer superGunPlayer;
    public Image hpBar;
    public TMP_Text playerName;
    public Vector3 UIPosOffset;

    private void Start()
    {
        SetName();
        SetColor();
    }
    void SetName()
    {
        GameData gdata = GameData.Get();
        playerName.text = gdata.GetPlayer(superGunPlayer.PlayerID).username;
    }
    void SetColor()
    {
        if(superGunPlayer.IsOwner)
        {
            hpBar.color = Color.green;
        }
        else
        {
            hpBar.color = Color.red;
        }
    }
    private void Update()
    {
        UpDateHP();
    }
    void UpDateHP()
    {
        hpBar.fillAmount = superGunPlayer.hp / superGunPlayer.hp_max;
    }

    public void ReverseScale(int x)
    {
        if(x!= this.transform.localScale.x)
        {
            this.transform.localPosition += UIPosOffset * x;
        }
        this.transform.localScale = new Vector3(x,1,1);
    }
}
