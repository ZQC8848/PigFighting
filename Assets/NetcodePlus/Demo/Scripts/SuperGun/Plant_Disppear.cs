using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Disppear : MonoBehaviour
{
    public float disappearTime;
    public float timer;

    PlatForm2D plat;
    private void Start()
    {
        timer = 0;
    }

    // Update is called once per frame

    void Update()
    {
        plat = GetComponent<PlatForm2D>();
        if(plat.onPlatPlayerList.Count>0 )
        {
            timer += Time.deltaTime;
            if( timer > disappearTime )
            {
                PlatDestory();
            }
        }
    }
    void PlatDestory()
    {
        Destroy( this.gameObject );
    }
}
