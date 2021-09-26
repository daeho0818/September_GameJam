using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 3f;
    public float DemonDetectDistance = 1;
    Vector2 originalPosition;
    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        if(player == null)
        {
            player = GameObject.Find("Player");
        }
        animator = GetComponentInChildren<Animator>();
        originalPosition = transform.position;
    }
    private void OnEnable()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!checkDemon())
        {
            Vector2 dir = player.transform.position - transform.position;
            Vector2 moveStep = Vector2.zero;

            transform.Translate(dir.normalized * moveSpeed / 100);
            transform.localScale = new Vector2(dir.x > 0 ? -1 : 1, 1);
            animator.SetInteger("GhostState", 0);
        }
        else
        {
            animator.SetInteger("GhostState",1);
            
        }
    }
    public void Init()
    {
        transform.position = originalPosition;
    }
    bool checkDemon()
    {
        GameObject[] demons = GameObject.FindGameObjectsWithTag("Demon");
        foreach (var demon in demons)
        {
            if(Vector2.Distance(demon.transform.position, transform.position) <= DemonDetectDistance)
            {
                return true;
            }
        }


        return false;
    }
}
