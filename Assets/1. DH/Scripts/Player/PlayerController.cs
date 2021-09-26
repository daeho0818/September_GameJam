using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMove playerMove;
    public PlayerAct playerAct;
    public PlayerAnimation playerAnimation;

    public Rigidbody2D rigid;
    public bool isBack;

    public int stage_number;

    public bool IsJump { get; set; }
    public bool IsWindBlow { get; set; }
    public bool IsDestroy { get; set; }
    public float Horizontal { get; set; }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        isBack = false;
    }

    private void Update()
    {
        IsWindBlow = Input.GetKeyDown(KeyCode.C);
        if (playerAct.is_wind_blow || GameManager.Instance.stage_clear || GameManager.Instance.window_open)
        {
            IsJump = false;
            Horizontal = 0;
            return;
        }
        IsJump = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
       
        Horizontal = Input.GetAxisRaw("Horizontal");
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBack)
        {
            if (collision.CompareTag("Flag"))
            {
                collision.GetComponent<Animator>().SetTrigger("Destroy");
                GameManager.Instance.stage_clear = true;
            }
            else if (collision.CompareTag("Thorn"))
            {
                IsDestroy = true;
            }
        }
    }
}
