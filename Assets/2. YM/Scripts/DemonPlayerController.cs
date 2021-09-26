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
            ArrayList orders = new ArrayList();
            playerStore.GetOrderAll(myStage-1,ref orders);

            if (orders.Count > 0)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    storeOrder.PutOrder(orders[i] as Order);
                }
            }
            orders = new ArrayList();
            playerStore.GetOrderAll(myStage, ref orders);
            if (orders.Count > 0)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    storeOrder.PutOrder(orders[i] as Order);
                }
            }

            
        }
        if (playerMove.isMain)
            storeOrder = playerStore;
        StartCoroutine(backOrder());
    }

    // Update is called once per frame
    void Update()
    {

    }
    bool isReady = true;
    int frameIndex = -1;
    IEnumerator backOrder()
    {
        isBack = false;
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
                        if (tempOrder.stage == myStage)
                        {
                            transform.position = tempOrder.position;
                            playerAnimation.SetAnimatorState(tempOrder.animState);
                            Horizontal = tempOrder.direction;
                            if (tempOrder.shotWind)
                            {
                                //¼¦
                            }
                        }
                        else
                            continue;
                        frameIndex = i;
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
        bool isChaged = false;
        for (int i = frameIndex!=-1 ? frameIndex:storeOrder.orders.Count-1; i >= 0; i-=(int)((storeOrder.orders.Count)/(myDuration/Time.fixedDeltaTime)))
        {

            Order tempOrder = storeOrder.orders[i] as Order;
            if(!isChaged&& playerMove.isMain&&tempOrder.stage == GameManager.Instance.GetStageIndex()-1)
            {
                    isChaged = true;
                    GameManager.Instance.LoadPastStage();
            }
            if (isChaged&&playerMove.isMain && tempOrder.stage == GameManager.Instance.GetStageIndex() - 1)
            {
                break;
            }
            transform.position = tempOrder.position;
            playerAnimation.SetAnimatorState(tempOrder.animState);
            Horizontal = -tempOrder.direction;
            yield return new WaitForFixedUpdate();

        }

        Horizontal = 0;
        isBack = false;
        if (playerMove.isMain)
            playerMove.mycontroller.isBack = false;

        if (playerMove.isMain)
        {
            GameManager.Instance.setControl();
            GameManager.Instance.demonGO();
            GameManager.Instance.demonReady(2);

            playerMove.storeOrder.ResetOrder(GameManager.Instance.GetStageIndex()+1);
            playerMove.storeOrder.ResetOrder(GameManager.Instance.GetStageIndex());
        }
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
    public void gogo()
    {
        isBack = false;
        StopAllCoroutines();
        StartCoroutine(backOrder());
    }
    public void resetStart()
    {
        StopAllCoroutines();
        StartCoroutine(resetBackOrder());
    }
}
