using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Move : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 moveDirection;
    public float moveChangeTime;
    public float moveTime;
    private bool moveToRight;
    private bool moveToLeft;
    void Start()
    {
        moveTime = 0;
        moveToRight = true;
        moveToLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveToRight && !moveToLeft)
        {
            moveTime += Time.deltaTime;
            MoveRightOrUp();
        }
        if(moveToLeft && !moveToRight)
        {
            moveTime -= Time.deltaTime;
            MoveLeftOrDown();
        }

        if(moveTime >= moveChangeTime)
        {
            moveToRight = false;
            moveToLeft = true;
        }
        else if (moveTime <= 0)
        {
            moveToLeft = false;
            moveToRight = true;
        }
    }

    private void MoveRightOrUp()
    {
        this.transform.Translate(moveDirection*moveSpeed*moveTime);
    }
    private void MoveLeftOrDown()
    {
        this.transform.Translate(-moveDirection * moveSpeed * moveTime);
    }
}
