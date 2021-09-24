using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreOrder
{
    public ArrayList orders;
    public StoreOrder()
    {
        orders = new ArrayList();
    }
    public void PutOrder(Order order)
    {
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
};

public class Order
{
    public OrderType orderType;
    public Vector2 direction;
    public float duration;
    public int stage;
}