using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIconChoose : MonoBehaviour
{
    public GameObject _pink;
    public GameObject _brown;
    public GameObject _yellow;
    public GameObject _grey;
    public void ChooseIcon(string color)
    {
        switch (color)
        {
            case "pink":
                {
                    _pink.SetActive(true);
                }break;
            case "brown":
                {
                    _brown.SetActive(true);
                }break;
            case "yellow":
                {
                    _yellow.SetActive(true);
                }break;
            case "grey":
                {
                    _grey.SetActive(true);
                }break;
        }
    }
}
