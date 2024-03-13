using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    private static WeaponsManager instance;

    public List<Weapons> weapons;

    private void Awake()
    {
        instance = this;
    }
    public Weapons FindWeapon(string weaponName)
    {
        foreach (var weapon in weapons)
        {
            if(weapon.weaponsName == weaponName) 
                return weapon;
        }
        return null;
    }

    public void InstantiateWeapon(string weaponName,Transform transform)
    {
        InstantiateWeapon(FindWeapon(weaponName),transform);
    }
    public GameObject InstantiateWeapon(Weapons weapon,Transform transform)
    {
        GameObject obj = Instantiate(weapon.weaponGameObject,transform.position,transform.rotation,transform);
        return obj;
    }
    public static WeaponsManager Get()
    {
        return instance;
    }
}
