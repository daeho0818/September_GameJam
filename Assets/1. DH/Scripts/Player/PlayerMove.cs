using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    PlayerController controller;
    public PlayerController mycontroller;
    public DemonPlayerController demonController;

    [Header("이동 관련")]
    [SerializeField] float move_speed;
    [SerializeField] float move_max_speed;
    public enum MoveState
    {
        Smooth,
        Not
    }
    public MoveState moveState;

    [Header("점프 관련")]
    [SerializeField] float jump_power;

    Rigidbody2D rigid => controller.rigid;

    RaycastHit2D[] hits;

    delegate void Func(float h);
    Func MoveFunc;

    public StoreOrder storeOrder;

    [Header("현재 스테이지 메인캐릭터인가")]
    public bool isMain = false;
    void Awake()
    {
    }

    void Start()
    {
        storeOrder = new StoreOrder();
        switch (moveState)
        {
            case MoveState.Smooth:
                MoveFunc = (h) =>
                {
                    rigid.AddForce(Vector2.right * h * Time.fixedDeltaTime * move_speed, ForceMode2D.Impulse);
                    if (rigid.velocity.x > move_max_speed)
                        rigid.velocity = new Vector2(move_max_speed, rigid.velocity.y);
                    else if (rigid.velocity.x < move_max_speed * -1)
                        rigid.velocity = new Vector2(move_max_speed * -1, rigid.velocity.y);
                };
                break;
            case MoveState.Not:
                MoveFunc = (h) =>
                {
                    transform.Translate(Vector2.right * h * Time.fixedDeltaTime * move_speed);
                };
                break;
            default:
                break;
        }
    }
    bool jumpStart;
    bool firstFrame = true;
    private void FixedUpdate()
    {
        if (firstFrame)
        {
            firstFrame = false;
            return;
        }
        if (isMain && !controller.isBack)
        {
            storeOrder.PutOrder(transform.position, controller.playerAnimation.GetAnimatorState(), (int)transform.localScale.x, GameManager.Instance.GetStageIndex(), false);
        }
    }
    void Update()
    {
        controller = mycontroller;
        if (controller.enabled == false)
        {
            controller = demonController;
        }

        float h = controller.Horizontal;
        if (isMain)
            MoveFunc(h);

        if (h != 0)
        {

            if (!controller.isBack)
                transform.localScale = new Vector2(1 * h, 1);
            if (controller.isBack)
                transform.localScale = new Vector2(1 * -h, 1);

            if (rigid.velocity.y == 0 && isMain&&!controller.isBack && !controller.playerAct.is_wind_blow)
                controller.playerAnimation.SetAnimatorState(1);
        }


        if (controller.playerAct.is_wind_zone && h == 0) rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
        else rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.6f);
        foreach (var hit in hits)
        {
            if (hit.transform.CompareTag("Platform"))
            {
                if (jumpStart)
                {
                    jumpStart = false;
                }
                if (controller.IsJump && rigid.velocity.y == 0f)
                {
                    if (isMain&&!controller.isBack && !controller.playerAct.is_wind_blow)
                    {
                        controller.playerAnimation.SetAnimatorState(2);
                        rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
                        jumpStart = true;
                        controller.IsJump = false;
                    }
                }
                else if (!controller.IsJump && rigid.velocity.y == 0)
                {
                    if (h == 0 && isMain&&!controller.isBack && !controller.playerAct.is_wind_blow)
                    {
                        controller.playerAnimation.SetAnimatorState(0);
                    }
                }
            }
        }
    }
}
