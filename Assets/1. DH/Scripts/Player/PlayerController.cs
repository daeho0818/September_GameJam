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
    void dieAnimEnd()
    {
        gameObject.SetActive(false);
    }
    public GameObject dieParticle;
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBack)
        {
            if (collision.CompareTag("Flag"))
            {
                rigid.velocity = new Vector2(0,rigid.velocity.y);
                collision.GetComponent<Animator>().SetTrigger("Destroy");
                GameManager.Instance.stage_clear = true;
                SoundManager.Instance.PlaySound(SoundManager.Instance.winSound);
            }
            else if (collision.CompareTag("Thorn"))
            {
                playerAnimation.SetAnimatorTrigger("Die");
                GameObject g = Instantiate(dieParticle);
                g.transform.position = transform.position;
                IsDestroy = true;
            }
            else if (collision.CompareTag("Ghost"))
            {
                playerAnimation.SetAnimatorTrigger("Die");
                GameObject g = Instantiate(dieParticle);
                g.transform.position = transform.position;
                IsDestroy = true;
                collision.GetComponent<GhostScript>().Init();
            }
        }
    }
}
