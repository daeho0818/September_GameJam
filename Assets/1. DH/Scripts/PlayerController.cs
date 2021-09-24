using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMove playerMove;
    public PlayerAct playerAct;

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
}
