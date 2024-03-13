using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapons
{
    private void Awake()
    {
        currentWeaponCount = weaponCount;
        weaponGameobject = gameObject;
    }
    public override void FisrtFunc()
    {
        Debug.Log("use pistol 1st func");
    }

    public override void SecondFunc()
    {
        Debug.Log("use pistol 2nd func");
    }
}
