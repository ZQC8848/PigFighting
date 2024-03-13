using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapons : MonoBehaviour
{
    public string weaponsName;
    public int weaponCount;
    public float firstFuncCoolTime;
    public float secondFuncCoolTime;
    public Transform firePoint;

    [HideInInspector]
    public int currentWeaponCount;
    [HideInInspector]
    public GameObject weaponGameobject;

    private void Awake()
    {
        currentWeaponCount = weaponCount;
        weaponGameobject = gameObject;
    }
    public abstract void FisrtFunc();
    public abstract void SecondFunc();

    public void Discard()
    {

    }
    public void Distory()
    {
        Destroy(this.gameObject);
    }
}
