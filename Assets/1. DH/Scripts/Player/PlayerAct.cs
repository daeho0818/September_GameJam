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
    public bool is_wind_blow = false;
    [SerializeField] GameObject WindZonePrefab;
    public GameObject WindZone { get; set; } = null;

    PlayerAction playerAction;

    void Start()
    {
        playerAction = new WindAct(this);
    }

    void Update()
    {
        if (controller.IsWindBlow && !is_wind_blow)
        {
            is_wind_blow = true;
            if (controller.rigid.velocity.y == 0)
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
        WindZone = Instantiate(WindZonePrefab, GameObject.Find("WindParent").transform);
        WindZone.transform.localPosition = Vector2.up;
        WindZone.GetComponent<StageObject>().stage_number = GameManager.Instance.current_stage_index + 1;

        Invoke("DestroyWind", 1);
    }

    void DestroyWind()
    {
        Destroy(WindZone);
        is_wind_blow = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WindZone"))
        {
            if (stage_number == collision.GetComponent<StageObject>().stage_number + 1)
            {
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
