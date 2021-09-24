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

    void Update()
    {
        MoveFunc(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.6f);

            foreach (var hit in hits)
            {
                if (hit.transform.CompareTag("Platform"))
                {
                    Debug.Log("응애");
                    rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
                    break;
                }
            }
        }
    }
}
