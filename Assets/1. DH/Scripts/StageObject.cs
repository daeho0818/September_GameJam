using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    public StageObject[] childObjects;
    public bool is_destroy_object;
    public int stage_number;

    Vector3 originalPos;
    Vector3 effectVector = new Vector3(0, 0.4f, 0);
    void Awake()
    {
        originalPos = transform.position;
        List<Transform> directChildren = new List<Transform>();
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            if (child.parent == transform && child.GetComponent<StageObject>() != null)
                directChildren.Add(child);
        childObjects = new StageObject[directChildren.Count];
        for (int i = 0; i < childObjects.Length; i++)
        {
            childObjects[i] = directChildren[i].GetComponent<StageObject>();
            childObjects[i].stage_number = stage_number;
        }

    }
    private void OnEnable()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(1, 1, 1, 0);
        transform.position = originalPos - effectVector;
    }
    public IEnumerator SpawnEffect()
    {
        float eTime = 0f;
        float duration = 0.15f;
        Color color = new Color(1, 1, 1, 0);
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.color = new Color(1, 1, 1, 0);
        transform.position = originalPos - effectVector;
        while (eTime < duration)
        {
            transform.position = Vector3.Lerp(originalPos - effectVector, originalPos, eTime/ duration);
            color = new Color(1, 1, 1, eTime / duration);
            if(spriteRenderer != null)
                spriteRenderer.color = color;
            eTime += Time.deltaTime;
            yield return null;
        }
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(1, 1, 1, 1);
        transform.position = originalPos;
    }
    public IEnumerator DestroyEffect(bool isMustDestroy)
    {
        if (!isMustDestroy && !is_destroy_object)
            yield break;
        float eTime = 0f;
        float duration = 0.4f;
        Vector3 originalPos = transform.position;
        Color color = new Color(1, 1, 1, 1);
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        while (eTime < duration)
        {
            transform.position = Vector3.Lerp(originalPos, originalPos - effectVector, eTime / duration);
            color = new Color(1, 1, 1, 1 - eTime / duration);
            if (spriteRenderer != null)
                spriteRenderer.color = color;
            eTime += Time.deltaTime;
            yield return null;
        }
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(1, 1, 1, 0);
        transform.position = originalPos;
            gameObject.SetActive(false);
    }
}
