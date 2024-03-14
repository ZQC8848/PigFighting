using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapons : MonoBehaviour
{
    [Header("Weapon Parameter")]
    public string weaponsName;

    public float firstFuncCoolTime;
    public float secondFuncCoolTime;

    public GameObject weaponGameObject;

    public abstract void FisrtFunc();
    public abstract void SecondFunc();

    public abstract void SetGunFacing(int i);

    public void Discard()
    {

    }
    public void Distory()
    {
        Destroy(this.gameObject);
    }
}
