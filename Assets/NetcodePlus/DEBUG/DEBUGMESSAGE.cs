using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DEBUGMESSAGE : MonoBehaviour
{
    static public DEBUGMESSAGE instance;
    public GameObject message;
    int max = 80;
    int list = 0;
    void Start()
    {
        instance = this;
    }
    public void AddDebugMessage(string str)
    {
        GameObject msg =  Instantiate(message, this.transform);
        TMP_Text tMP_Text = msg.GetComponent<TMP_Text>();
        tMP_Text.text = str;
        tMP_Text.color = Color.red;
        list++;
    }
    private void Update()
    {
        if(list >= max)
        {
            list = 0;
            foreach(Transform t in transform)
            {
                Destroy(t.gameObject);
            }
        }
    }
    static public DEBUGMESSAGE Get()
    {
        return instance;
    }
}
