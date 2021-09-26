using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreOrder
{
    public ArrayList orders;
    Order lastOrder;
    public StoreOrder()
    {
        orders = new ArrayList();
    }
    public void PutOrder(Order order)
    {/*
        if (orders.Count > 0)
        {
        lastOrder = orders[orders.Count-1] as Order;
        if(lastOrder.stage == order.stage)
        {
            if (lastOrder.orderType == order.orderType)
            {
                if (lastOrder.direction == order.direction)
                {
                        if (lastOrder.h == order.h)
                        {
                            lastOrder.duration++;
                            return;
                        }
                }
            }
            }
        }*/

        orders.Add(order);
    }
    public void PutOrder(Vector2 position, int animState, int direction,int stage,bool shotWind)
    {
        Order order = new Order();
        order.position = position;
        order.animState = animState;
        order.direction = direction;
        order.stage = stage;
        order.shotWind = shotWind;
        PutOrder(order);
    }
    public Order GetOrder(int stage, bool rv = true)
    {//Demon Awake에서 자기 stage 오더들을 싹다 추출해가게 하면 될듯
        Order returnOrder = null;
        for (int i = 0; i < orders.Count; i++)
        {
            if(((Order)orders[i]).stage == stage)
            {
                returnOrder = orders[i] as Order;
                if(rv)
                orders.Remove(returnOrder);
                break;
            }
        }
        return returnOrder;
    }
    public void GetOrderAll(int stage, ref ArrayList arrayList)
    {//Demon Awake에서 자기 stage 오더들을 싹다 추출해가게 하면 될듯
       
        for (int i = 0; i < orders.Count; i++)
        {
            if (((Order)orders[i]).stage == stage)
            {
                arrayList.Add(orders[i]);
                continue;
            }
        }
    }
    public void ResetOrder(int stage)
    {
        for (int i = orders.Count - 1; i >= 0; i--)
        {
            if (((Order)orders[i]).stage == stage)
            {
                orders.Remove(orders[i]);
            }
        }
    }
}

public class Order
{
    public int stage;

    public Vector2 position;
    public int animState;
    public int direction;
    public bool shotWind;
}