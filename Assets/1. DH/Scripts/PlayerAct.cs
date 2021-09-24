using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAct : MonoBehaviour
{
    public PlayerController controller;

    [Header("바람 관련")]
    [SerializeField] internal float up_power;
    [SerializeField] internal float max_pos_y_value;

    PlayerAction playerAction;
    void Start()
    {
        playerAction = new WindAct(this);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
            playerAction.Act();
    }
}

public abstract class PlayerAction
{
    public abstract void Act();
}

public class WindAct : PlayerAction
{
    PlayerAct playerAct;
    Rigidbody2D rigid => playerAct.controller.rigid;
    float up_power => playerAct.up_power;
    float max_pos_y_value => playerAct.max_pos_y_value;
    public WindAct(PlayerAct playerAct)
    {
        this.playerAct = playerAct;
    }
    public override void Act()
    {
        if (rigid.transform.position.y > max_pos_y_value)
            playerAct.Invoke("SetPosition", 0.1f);
        else
            rigid.AddForce(Vector2.up * up_power);
    }
    void SetPosition()
    {
        rigid.transform.position = new Vector2(rigid.transform.position.x, max_pos_y_value);
    }
}
