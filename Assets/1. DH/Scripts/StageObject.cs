using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    public StageObject[] childObjects;
    public bool is_destroy_object;
    public int stage_number;
    void Awake()
    {
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

    void Update()
    {
    }
}
