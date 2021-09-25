using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Camera가 갖고 있는 컴포넌트
public class CameraActor : MonoBehaviour
{
    new Camera camera;
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {

    }

    public IEnumerator Zoom(bool zoom_in, Vector3 position, float zoom_size, float time = 1.0f)
    {
        float distance = Mathf.Abs(camera.orthographicSize - zoom_size);
        float distance_per_sec = distance / time;
        Vector2 target_per_sec = (position - camera.transform.position) / time;
        Debug.Log(target_per_sec);
        while (true)
        {
            if (Mathf.Abs(camera.orthographicSize - zoom_size) <= 0.05f)
            {
                camera.orthographicSize = zoom_size;
                break;
            }
            camera.orthographicSize += distance_per_sec / 100 * (zoom_in ? -1 : 1);
            camera.transform.Translate(target_per_sec / 100);
            yield return new WaitForSeconds(0.001f);
        }
    }
}
