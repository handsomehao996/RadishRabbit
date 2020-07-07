using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//控制玩家移动
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float jumpHoldForce = 3f;
    public float jumpHoldTime = 0.5f;
    public float sprintForce = 5f;
    public float sprintTime = 0.5f;
    public float sprintDrag = 8;
    public float sprintFrequency = 1;
    public float catchWallGravity = 3;
    public float wallSlideSpeed = 1;
    public float wallJumpForce = 5f;
    public float wallJumpWaitTime = 0.5f;

    public PlayerBackView PlayerBackView;

    Rigidbody2D rb;
    CheckAround checkAround;
    SpriteRenderer spriteRenderer;

    float hor;
    float horRaw;
    public float ver;
    float verRaw;
    float jumpHoldTimeP;
    float originalGravity;
    float originalDrag;
    float sprintFrequencyP;

    bool jumpHold;
    public bool isDuck;
    bool isSprint;
    bool onFloorParticle;

    PlayerParticleEffect particleEffect;

    //bool isCatchWall;
    //public bool isCatchWallSlide;
    //bool wallJump;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkAround = GetComponent<CheckAround>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleEffect = GetComponent<PlayerParticleEffect>();

        originalGravity = rb.gravityScale;
        originalDrag = rb.drag;
        sprintFrequencyP = sprintFrequency;
    }

    private void Update()
    {
        hor = Input.GetAxis("Horizontal");
        horRaw = Input.GetAxisRaw("Horizontal");
        ver = Input.GetAxis("Vertical");
        verRaw = Input.GetAxisRaw("Vertical");

        //按键跳跃
        if (Input.GetKeyDown(KeyCode.K) && checkAround.onFloor)
        {
            rb.velocity += Vector2.up * jumpForce;
            particleEffect.jumpParticle.Play();
            jumpHold = true;
        }
        //掉在地面上的粒子效果与启动
        if(onFloorParticle && checkAround.onFloor)
        {
            particleEffect.onFloorParticle.Play();
            onFloorParticle = false;
        }
        if(!onFloorParticle && !checkAround.onFloor)
        {
            onFloorParticle = true;
        }

        //转身
        if (horRaw > 0.1)
        {
            spriteRenderer.flipX = false;
        }
        if (horRaw < -0.1)
        {
            spriteRenderer.flipX = true;
        }

        //下蹲
        if (Input.GetKey(KeyCode.S) && checkAround.onFloor)
        {
            isDuck = true;
        }
        else
        {
            isDuck = false;
        }

        //冲刺
        if (Input.GetKeyDown(KeyCode.L) && !isSprint && sprintFrequencyP > 0 && !isDuck)
        {
            isSprint = true;
            Vector2 dir = new Vector2(horRaw, verRaw);

            //冲刺开始改变参数
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            rb.drag = sprintDrag;
            particleEffect.dashParticle.Play();
            //冲刺次数减少
            sprintFrequencyP--;

            //背影、摄像机震动、水波
            PlayerBackView.BackViewAppear();
            Camera.main.transform.DOComplete();
            Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
            Camera.main.GetComponent<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

            if (dir != Vector2.zero)
            {
                //如果带方向的冲刺
                rb.velocity += dir.normalized * sprintForce;
            }
            else
            {
                //如果直接按下冲刺
                rb.velocity += Vector2.right * (spriteRenderer.flipX ? -1 : 1) * sprintForce;
            }
            StartCoroutine(Sprint());
        }
        //到地面恢复冲刺次数
        if (checkAround.onFloor && !isSprint)
        {
            sprintFrequencyP = sprintFrequency;
        }

        #region 墙跳部分（暂时）
        //抓右墙
        //if (checkAround.rightHaveWall)
        //{
        //    //滑墙
        //    if (!isCatchWall)
        //    {
        //        if (horRaw > 0.1)
        //        {
        //            isCatchWallSlide = true;
        //            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        //            rb.gravityScale = catchWallGravity;
        //        }
        //        else
        //        {
        //            isCatchWallSlide = false;
        //            rb.gravityScale = originalGravity;
        //        }
        //    }

        //    //抓住墙
        //    if (Input.GetKey(KeyCode.J))
        //    {
        //        isCatchWall = true;
        //        rb.velocity = new Vector2(rb.velocity.x, 0);
        //    }
        //    else
        //    {
        //        isCatchWall = false;
        //    }
        //}

        //右墙跳
        //if (checkAround.rightHaveWall)
        //{
        //    if (Input.GetKeyDown(KeyCode.K))
        //    {
        //        //wallJump = true;
        //        if(horRaw != 0)
        //        {
        //            Vector2 dir = Vector2.up;
        //            rb.velocity = new Vector2(rb.velocity.x, 0);
        //            rb.velocity += dir.normalized * wallJumpForce;
        //        }
        //        //else
        //        //{
        //        //    wallJump = true;
        //        //    Vector2 dir = Vector2.up + Vector2.left;
        //        //    rb.velocity = Vector2.zero;
        //        //    rb.velocity += dir.normalized * wallJumpForce;
        //        //    //rb.AddForce(dir.normalized * wallJumpForce, ForceMode2D.Impulse);
        //        //}

        //        //StartCoroutine(WallJump());
        //    }
        //}
        //左墙跳
        //if (checkAround.leftHaveWall)
        //{
        //    if (Input.GetKeyDown(KeyCode.K))
        //    {
        //        //wallJump = true;
        //        if (horRaw != 0)
        //        {
        //            Vector2 dir = Vector2.up;
        //            rb.velocity = new Vector2(rb.velocity.x, 0);
        //            rb.velocity += dir.normalized * wallJumpForce;
        //        }
        //        //else
        //        //{
        //        //    wallJump = true;
        //        //    Vector2 dir = Vector2.up + Vector2.right;
        //        //    rb.velocity = Vector2.zero;
        //        //    rb.velocity += dir.normalized * wallJumpForce;
        //        //    //rb.AddForce(dir.normalized * wallJumpForce, ForceMode2D.Impulse);
        //        //}

        //        //StartCoroutine(WallJump());
        //    }
        //}
        #endregion
    }

    private void FixedUpdate()
    {
        //左右移动
        if(!isSprint && !isDuck)
        {
            rb.velocity = new Vector2(hor * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if (isDuck)
        {
            rb.velocity = Vector2.zero;
        }
        //久按跳跃
        if (Input.GetKey(KeyCode.K) && jumpHold)
        {
            if (jumpHoldTimeP < jumpHoldTime)
            {
                rb.velocity += Vector2.up * jumpHoldForce * Time.fixedDeltaTime;
                jumpHoldTimeP += Time.fixedDeltaTime;
            }
        }
        else
        {
            jumpHoldTimeP = 0;
            jumpHold = false;
        }
    }

    IEnumerator Sprint()
    {
        yield return new WaitForSeconds(sprintTime);

        //冲刺完毕后恢复参数
        rb.gravityScale = originalGravity;
        rb.drag = originalDrag;
        particleEffect.dashParticle.Stop();

        isSprint = false;
    }
}
