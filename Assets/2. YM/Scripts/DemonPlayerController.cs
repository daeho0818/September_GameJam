using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonPlayerController : PlayerController
{
    StoreOrder storeOrder;
    int myStage;
    Vector2 startPosition;
    Quaternion startRotation;
    void Start()
    {
    }

    private void OnEnable()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        myStage = 1;
        StoreOrder playerStore = playerMove.storeOrder;

        storeOrder = new StoreOrder();
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
                IsJump = tempOrder.orderType == OrderType.jump;
                for (int j = 0; j <= tempOrder.duration; j++)
                {
                    if (tempOrder.orderType == OrderType.move || tempOrder.orderType == OrderType.idle)
                    {
                        Horizontal = tempOrder.h;
                    }
                    yield return null;
                }
            }
        } while (true);
        
    }
}
