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
    {
        do {
            transform.position = startPosition;
            transform.rotation = startRotation;
            for (int i = 0; i < storeOrder.orders.Count; i++)
            {
                Order tempOrder = storeOrder.orders[i] as Order;
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
}
