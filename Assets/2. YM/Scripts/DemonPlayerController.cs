using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonPlayerController : PlayerController
{
    StoreOrder storeOrder;
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
    {//////////////죽으면 0부터 다시하도록
        do {
            transform.position = startPosition;
            transform.rotation = startRotation;
            for (int i = 0; i < storeOrder.orders.Count; i++)
            {
                Order tempOrder = storeOrder.orders[i] as Order;
                if (tempOrder.orderType == OrderType.land)
                    continue;
                for (int j = 0; j <= tempOrder.duration; j++)
                {
                    yield return null;
                    if (tempOrder.orderType == OrderType.move || tempOrder.orderType == OrderType.idle)
                    {
                        Horizontal = tempOrder.h;
                    }
                    IsJump = tempOrder.orderType == OrderType.jump;
                }
            }
        } while (storeOrder.orders.Count>0);
        
    }
    IEnumerator resetBackOrder()
    {
        isBack = true;
            for (int i = storeOrder.orders.Count-1; i >= 0; i--)
            {
                Order tempOrder = storeOrder.orders[i] as Order;
                if (tempOrder.orderType == OrderType.jump)
                    continue;
                for (int j = 0; j <= tempOrder.duration/2; j++)
                {
                    yield return null;
                    if (tempOrder.orderType == OrderType.move || tempOrder.orderType == OrderType.idle)
                    {
                        Horizontal = tempOrder.h*-1;
                    }
                    IsJump = tempOrder.orderType == OrderType.land;
                }
            }
        Horizontal = 0;
        IsJump = false;
        GameManager.Instance.LoadPastStage();
    }
    public void resetStart()
    {
        StopAllCoroutines();
        StartCoroutine(resetBackOrder());
    }
}
