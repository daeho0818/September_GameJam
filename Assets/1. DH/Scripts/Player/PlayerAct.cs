using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAct : MonoBehaviour
{
    public PlayerController controller;

    [Header("바람 관련")]
    [SerializeField] internal float up_power;
    [SerializeField] internal float max_pos_y_value;
    public int stage_number
    {
        get
        {
            DemonPlayerController DC = GetComponent<DemonPlayerController>();
            if (DC)
            {
                return DC.stage_number;
            }
            return controller.stage_number;
        }
        set
        {
            DemonPlayerController DC = GetComponent<DemonPlayerController>();
            if (DC)
            {
                DC.stage_number = value;
            }
            controller.stage_number = value;
        }
    }
    public bool is_wind_zone = false;
    [SerializeField] GameObject WindZonePrefab;
    public GameObject WindZone { get; set; } = null;
    Coroutine coroutine = null;

    PlayerAction playerAction;

    void Start()
    {
        playerAction = new WindAct(this);
    }

    void Update()
    {
        if (WindZone)
        {
            if (coroutine == null)
                coroutine = StartCoroutine(ObjectBlink(WindZone.GetComponent<SpriteRenderer>()));
        }
        if (controller.IsWindBlow)
        {
            controller.playerAnimation.SetAnimatorState(4);
            Invoke("BlowWind", 0.75f);
        }

        playerAction.Act(is_wind_zone);
    }

    void BlowWind()
    {
        if (WindZone)
        {
            DestroyWind();
        }
        WindZone = Instantiate(WindZonePrefab, transform.position + Vector3.up / 0.5f, Quaternion.identity, GameManager.Instance.stages[GameManager.Instance.current_stage_index].transform);
        WindZone.GetComponent<StageObject>().stage_number = GameManager.Instance.current_stage_index + 1;

        Invoke("DestroyWind", 1);
    }

    void DestroyWind()
    {
        Destroy(WindZone);
        StopCoroutine(coroutine);
        coroutine = null;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WindZone"))
        {
            if (stage_number == collision.GetComponent<StageObject>().stage_number + 1)
            {
                Debug.Log("응애 제발...");
                controller.playerAnimation.SetAnimatorState(2);
                is_wind_zone = true;
                controller.rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WindZone"))
        {
            if (stage_number == collision.GetComponent<StageObject>().stage_number + 1)
            {
                is_wind_zone = false;
                controller.rigid.constraints = RigidbodyConstraints2D.None;
            }
        }
    }
    IEnumerator ObjectBlink(SpriteRenderer renderer)
    {
        Color color;
        while (true)
        {
            for (int i = 0; i < 20; i++)
            {
                color = renderer.color;
                renderer.color = Color.Lerp(renderer.color, new Color(color.r, color.g, color.b, 0), 0.1f);
                yield return new WaitForSeconds(0.05f);
            }

            for (int i = 0; i < 20; i++)
            {
                color = renderer.color;
                renderer.color = Color.Lerp(renderer.color, new Color(color.r, color.g, color.b, 1), 0.1f);
                yield return new WaitForSeconds(0.05f);
            }
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
