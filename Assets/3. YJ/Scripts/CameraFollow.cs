using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float topOffset;
    [SerializeField] float bottomOffset;
    [SerializeField] float lerpSpeed;

    Transform flag = null;

    private void Update()
    {
        if (flag == null || !flag.gameObject.activeInHierarchy)
            flag = GameObject.FindGameObjectWithTag("Flag").transform;
        transform.position = new Vector3(transform.position.x, 
            Mathf.Lerp(transform.position.y, (flag.position.y + playerTransform.position.y)/2, lerpSpeed * Time.deltaTime),
            transform.position.z);
    }
}
