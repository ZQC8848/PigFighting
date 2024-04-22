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
        GameObject weaponGhost =  Instantiate(this.gameObject, this.transform.parent.position,Quaternion.identity);
        weaponGhost.layer = 9;
        weaponGhost.AddComponent<Rigidbody2D>();
        DelayDistory distory = weaponGhost.AddComponent<DelayDistory>();
        distory.time = 6;
        distory.StartRotate();
        Destroy(weaponGhost.GetComponent<Weapons>());

        Distory();
    }
    public void Distory()
    {
        Destroy(this.gameObject);
    }
}
