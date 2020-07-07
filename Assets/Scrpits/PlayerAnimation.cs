using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制角色动画
public class PlayerAnimation : MonoBehaviour
{
    Animator ani;
    Rigidbody2D rb;
    PlayerMovement playerMovement;
    CheckAround checkAround;

    private void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        checkAround = GetComponent<CheckAround>();
    }

    private void Update()
    {
        ani.SetFloat("VelocityXAbs", Mathf.Abs(rb.velocity.x));
        ani.SetFloat("VelocityY", rb.velocity.y);
        ani.SetFloat("VelocityYAbs", Mathf.Abs(rb.velocity.y));
        ani.SetBool("Duck", playerMovement.isDuck);
        //ani.SetBool("Climb", playerMovement.isCatchWallSlide);
        ani.SetFloat("Vertical", playerMovement.ver);
    }
}
