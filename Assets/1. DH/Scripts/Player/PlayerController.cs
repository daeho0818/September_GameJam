using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMove playerMove;
    public PlayerAct playerAct;
    public PlayerAnimation playerAnimation;

    public Rigidbody2D rigid;

    public bool IsJump { get; set; }
    public float Horizontal { get; set; }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        IsJump = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        Horizontal = Input.GetAxisRaw("Horizontal");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flag"))
        {
            collision.GetComponent<Animator>().SetTrigger("Destroy");
            Invoke("Clear", 2);
        }
    }

    void Clear()
    {
        GameManager.Instance.stage_clear = true;
    }
}
