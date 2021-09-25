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
    {
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
        }

        orders.Add(order);
    }
    public Order GetOrder(int stage)
    {
        Order returnOrder = null;
        for (int i = orders.Count-1; i >= 0; i--)
        {
            if(((Order)orders[i]).stage == stage)
            {
                returnOrder = orders[i] as Order;
                orders.Remove(returnOrder);
                break;
            }
        }
        return returnOrder;
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
public enum OrderType
{
    move = 0,
    jump = 1,
    shot = 2,
    idle = 3,
};

public class Order
{
    public OrderType orderType;
    public float h;
    public Vector2 direction;
    public int duration;
    public int stage;
}