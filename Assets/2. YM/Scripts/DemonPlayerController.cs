using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonPlayerController : PlayerController
{
    public StoreOrder storeOrder;
    public int myStage;
    public Vector2 startPosition;
    public Quaternion startRotation;
    SpriteRenderer[] sr;
    void Start()
    {
        sr = GetComponentsInChildren < SpriteRenderer >();
    }
    public void setAlpha(float a)
    {
        if (sr == null)
        {
            sr = GetComponentsInChildren<SpriteRenderer>();
        }
        for (int i = 0; i < sr.Length; i++)
        {
            Color c = sr[i].color;
            c.a = a;
            sr[i].color = c;
        }
    }

    private void OnEnable()
    {
        StoreOrder playerStore = GameObject.Find("Player").GetComponent<PlayerMove>().storeOrder;

        if (storeOrder != null)
        {
        }
        else
        {
            storeOrder = new StoreOrder();
            if (playerStore == null)
            {
                playerMove.storeOrder = new StoreOrder();
                playerStore = playerMove.storeOrder;
            }
            Order tempOrder = playerStore.GetOrder(myStage);
            while (tempOrder != null)
            {
                storeOrder.PutOrder(tempOrder);
                tempOrder = playerStore.GetOrder(myStage);
            }
        }
        StartCoroutine(backOrder());
    }

    // Update is called once per frame
    void Update()
    {

    }
    bool isReady = true;
    IEnumerator backOrder()
    {//////////////Á×À¸¸é 0ºÎÅÍ ´Ù½ÃÇÏµµ·Ï
        if (!isBack&&!playerMove.isMain)
        {
            do
            {
                if (isReady)
                {
                    transform.position = startPosition;
                    transform.rotation = startRotation;
                    playerAnimation.SetAnimatorState(0);
                    for (int i = 0; i < storeOrder.orders.Count; i++)
                    {
                        Order tempOrder = storeOrder.orders[i] as Order;

                        transform.position = tempOrder.position;
                        playerAnimation.SetAnimatorState(tempOrder.animState);
                        Horizontal = tempOrder.direction;
                        if (tempOrder.shotWind)
                        {
                            //¼¦
                        }
                        yield return new WaitForFixedUpdate();
                    }
                    Horizontal = 0;
                    isReady = false;
                    GameManager.Instance.demonReady();
                }
                else
                {
                    yield return null;
                    if (GameManager.Instance.demonList.Count <= 1)
                        ready();
                }
            } while (storeOrder.orders.Count > 0 && !isBack);
        }
    }
    IEnumerator resetBackOrder()
    {
        isBack = true;
        float myDuration = 3f;
        if (playerMove.isMain)
            playerMove.mycontroller.isBack = true;
        for (int i = storeOrder.orders.Count-1; i >= 0; i--)
        {
            Order tempOrder = storeOrder.orders[i] as Order;

            transform.position = tempOrder.position;
            playerAnimation.SetAnimatorState(tempOrder.animState);
            Horizontal = -tempOrder.direction;
            if (tempOrder.shotWind)
            {
                //¼¦
            }
            yield return new WaitForSeconds(myDuration/storeOrder.orders.Count);
        }

        Horizontal = 0;
        if (playerMove.isMain)
        {
            GameManager.Instance.LoadPastStage();
        }

        for (int i = storeOrder.orders.Count - 1; i >= 0; i--)
        {
            Order tempOrder = storeOrder.orders[i] as Order;

            transform.position = tempOrder.position;
            playerAnimation.SetAnimatorState(tempOrder.animState);
            Horizontal = -tempOrder.direction;
            if (tempOrder.shotWind)
            {
                //¼¦
            }
            yield return new WaitForSeconds(myDuration / storeOrder.orders.Count);
        }

        if (playerMove.isMain)
            GameManager.Instance.setControl();

        isBack = false;
        if (playerMove.isMain)
            playerMove.mycontroller.isBack = false;
        else
        StartCoroutine(backOrder());
    }
    private void FixedUpdate()
    {
        
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    public void ready()
    {
        isReady = true;
    }
    public void resetStart()
    {
        StopAllCoroutines();
        StartCoroutine(resetBackOrder());
    }
}
