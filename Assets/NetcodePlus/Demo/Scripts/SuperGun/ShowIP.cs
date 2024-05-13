using NetcodePlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowIP : MonoBehaviour
{
    TMP_Text TMP_Text;
    void Start()
    {
        TMP_Text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TheNetwork.Get().IsServer)
        {
            TMP_Text.text = "LAN IP \n" + NetworkTool.GetLocalIp();
        }
        else
        {
            this.gameObject.SetActive(false);
        }


    }
}
