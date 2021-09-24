using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAct : MonoBehaviour
{
    public PlayerController controller;

    [Header("바람 관련")]
    [SerializeField] internal float up_power;
    [SerializeField] internal float max_pos_y_value;
    public bool is_wind_zone = false;

    PlayerAction playerAction;
    void Start()
    {
        playerAction = new WindAct(this);
    }

    void Update()
    {
        playerAction.Act(is_wind_zone);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WindZone"))
        {
            is_wind_zone = true;
            controller.rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WindZone"))
        {
            is_wind_zone = false;
            controller.rigid.constraints = RigidbodyConstraints2D.None;
        }
    }
}

public abstract class PlayerAction
{
    public abstract void Act(bool isPlay);
}

public class WindAct : PlayerAction
{
    PlayerAct playerAct;
    Rigidbody2D rigid => playerAct.controller.rigid;
    float up_power => playerAct.up_power;
    float max_pos_y_value => playerAct.max_pos_y_value;

    Coroutine coroutine = null;
    Vector2 saveVelocity;
    public WindAct(PlayerAct playerAct)
    {
        this.playerAct = playerAct;
    }
    public override void Act(bool isPlay)
    {
        if (!isPlay) return;
        if (rigid.transform.position.y > max_pos_y_value)
        {
            if (coroutine == null)
            {
                saveVelocity = rigid.velocity;
                coroutine = playerAct.StartCoroutine(SetPosition());
            }
        }
        else
        {
            rigid.AddForce(Vector2.up * up_power);
        }
    }
    IEnumerator SetPosition()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        yield return new WaitForSeconds(0.5f);
        rigid.velocity = saveVelocity;
        coroutine = null;
    }
}
