using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测周围环境
public class CheckAround : MonoBehaviour
{
    public float radius = 0.2f;
    public Vector2 floorCheckOffset;
    public Vector2 catchWallRight;
    public Vector2 catchWallLeft;
    public LayerMask floor;

    public bool onFloor;
    //public bool rightHaveWall;
    //public bool leftHaveWall;
    //public bool catchWall;

    private void Update()
    {
        onFloor = Physics2D.OverlapCircle((Vector2)transform.position + floorCheckOffset, radius, floor);
        //rightHaveWall = Physics2D.OverlapCircle((Vector2)transform.position + catchWallRight, radius, floor);
        //leftHaveWall = Physics2D.OverlapCircle((Vector2)transform.position + catchWallLeft, radius, floor);
        //if(catchRight || catchLeft)
        //{
        //    catchWall = true;
        //}
        //else
        //{
        //    catchWall = false;
        //}
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireSphere(圆心，半径)
        Gizmos.DrawWireSphere((Vector2)transform.position + floorCheckOffset, radius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + catchWallRight, radius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + catchWallLeft, radius);
    }
}
