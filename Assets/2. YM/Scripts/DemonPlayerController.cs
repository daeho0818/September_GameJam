using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonPlayerController : PlayerController
{
    public StoreOrder storeOrder;
    public int myStage;
    public Vector2 startPosition;
    public Quaternion startRotation;
    void Start()
    {
    }

    private void OnEnable()
    {
        StoreOrder playerStore = playerMove.storeOrder;

        storeOrder = new StoreOrder();
        if(playerStore == null)
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
        StartCoroutine(backOrder());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator backOrder()
    {//////////////Á×À¸¸é 0ºÎÅÍ ´Ù½ÃÇÏµµ·Ï
        do
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
        } while (storeOrder.orders.Count>0&&!isBack);
        
    }
    IEnumerator resetBackOrder()
    {
        isBack = true;
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
            yield return new WaitForFixedUpdate();
        }

        Horizontal = 0;
        if (playerMove.isMain)
        {
            GameManager.Instance.LoadPastStage();

            Debug.Log("?");
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
            yield return new WaitForFixedUpdate();
        }

        if (playerMove.isMain)
            GameManager.Instance.setControl();

        isBack = false;
        if (playerMove.isMain)
            playerMove.mycontroller.isBack = false;
        
    }
    private void FixedUpdate()
    {
        
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    public void resetStart()
    {
        StopAllCoroutines();
        StartCoroutine(resetBackOrder());
    }
}
