using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportArea : MonoBehaviour
{
    public Transform teleportPoint;
    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.gameObject.transform.position = teleportPoint.position;
    }
}
