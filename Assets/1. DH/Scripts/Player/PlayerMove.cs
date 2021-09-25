using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public PlayerController controller;

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

    StoreOrder storeOrder;
    void Awake()
    {
        storeOrder = new StoreOrder();
        Backstate = false;
    }

    bool Backstate;
    void Start()
    {
        switch (moveState)
        {
            case MoveState.Smooth:
                MoveFunc = (h) =>
                {
                    rigid.AddForce(Vector2.right * h * Time.deltaTime * move_speed, ForceMode2D.Impulse);
                    if (rigid.velocity.x > move_max_speed)
                        rigid.velocity = new Vector2(move_max_speed, rigid.velocity.y);
                    else if (rigid.velocity.x < move_max_speed * -1)
                        rigid.velocity = new Vector2(move_max_speed * -1, rigid.velocity.y);
                };
                break;
            case MoveState.Not:
                MoveFunc = (h) =>
                {
                    transform.Translate(Vector2.right * h * Time.deltaTime * move_speed);
                };
                break;
            default:
                break;
        }
    }
    IEnumerator backOrder()
    {
        Backstate = true;
        Order tempOrder = storeOrder.GetOrder(1);

        while (tempOrder != null)
        {
            for (int i = 0; i < tempOrder.duration; i++)
            {
                if (tempOrder.orderType == OrderType.move || tempOrder.orderType == OrderType.idle)
                {
                    MoveFunc(tempOrder.h);
                }
                yield return null;
                Debug.Log(tempOrder.duration);
            }
            tempOrder = storeOrder.GetOrder(1);
        }
        Backstate = false;

    }

    void Update()
    {
        if (!Backstate)
        {
            float h = controller.Horizontal;
            MoveFunc(h);

            if (controller.playerAct.is_wind_zone && h == 0) rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
            else rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

            Order order = new Order();
            order.h = h;
            order.orderType = OrderType.move;
            order.duration = 0;
            order.stage = 1;
            storeOrder.PutOrder(order);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(backOrder());
            }


            if (controller.IsJump)
            {
                hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.6f);

                foreach (var hit in hits)
                {
                    if (hit.transform.CompareTag("Platform"))
                    {
                        rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
                        break;
                    }
                }
            }
        }
    }
}
