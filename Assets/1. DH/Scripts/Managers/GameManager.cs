using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;
    public GameObject DemonPrefab;
    public CameraActor cameraActor;

    public GameObject[] stages;
    [SerializeField] PlayerController player;
    [SerializeField] DemonPlayerController demonPlayer;

    Coroutine coroutine = null;

    private int all_stage_count => stages.Length;
    public int current_stage_index { get; set; } = 0;

    public bool stage_clear = false;
    public bool stage_start = false;

    bool stage_reset = false;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        stages[0].SetActive(true);
        player.gameObject.SetActive(false);
        player.transform.position = GameObject.Find("Spawn Point").transform.position;

        player.playerAct.stage_number = 1;

        StartCoroutine(StageAnimation());

        cameraActor = Camera.main.GetComponent<CameraActor>();
        // StartCoroutine(StartCamAct());
    }

    void Update()
    {
        if (stage_clear)
        {
            // SoundManager.Instance.SoundPlay(SoundManager.Instance.stage_clear);
            if (coroutine == null)
                coroutine = StartCoroutine(LoadNextStage());
            // StartCoroutine(StartCamAct());
        }
        else if (player.IsDestroy)
        {
            player.IsDestroy = false;
            // player.playerAnimation.SetAnimatorState(5);
            Invoke("GoToSpawnPoint", 2);
        }
        if (stage_reset)
        {
            demonPlayer.resetStart();
            stage_reset = false;
        }
    }
    void GoToSpawnPoint()
    {
        player.transform.position = GameObject.Find("Spawn Point").transform.position;
    }
    // IEnumerator StartCamAct()
    // {
    //     yield return StartCoroutine(cameraActor.Zoom(true, player.transform.position + Vector3.up * 2, 3, 1));
    //     yield return new WaitForSeconds(1);
    //     StartCoroutine(cameraActor.Zoom(false, Vector2.zero, 5, 0.5f));
    // }
    IEnumerator LoadNextStage()
    {
        yield return new WaitForSeconds(2);
        if (current_stage_index + 1 < all_stage_count)
        {
            if (current_stage_index >= 0)
            {
                StageObject[] objects = stages[current_stage_index].GetComponent<StageObject>().childObjects;
                foreach (var obj in objects)
                {
                    if (obj.is_destroy_object) obj.gameObject.SetActive(false);
                }
            }

            stages[++current_stage_index].SetActive(true);
            if (true)
            {
                StageObject[] objects = stages[current_stage_index].GetComponent<StageObject>().childObjects;
                foreach (var obj in objects)
                {
                    obj.gameObject.SetActive(true);
                }
            }

            spawnDemon();

            player.playerAct.stage_number = current_stage_index + 1;
            player.playerMove.storeOrder.ResetOrder(current_stage_index);
            StartCoroutine(StageAnimation());
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
        }
    }
    public void LoadPastStage()
    {

        GameObject[] demons = GameObject.FindGameObjectsWithTag("Demon");
        foreach (var demon in demons)
        {

            if (demon.GetComponent<DemonPlayerController>().myStage == current_stage_index)
            {
                demonPlayer.storeOrder = demon.GetComponent<DemonPlayerController>().storeOrder;
                Destroy(demon);
            }
        }
        StageObject[] objects = stages[current_stage_index].GetComponent<StageObject>().childObjects;
        foreach (var obj in objects)
        {
            obj.gameObject.SetActive(false);
        }

        stages[current_stage_index--].SetActive(false);

        objects = stages[current_stage_index].GetComponent<StageObject>().childObjects;
        foreach (var obj in objects)
        {
            obj.gameObject.SetActive(true);
        }

        player.playerAct.stage_number = current_stage_index + 1;
        StartCoroutine(StageAnimation());
    }
    public void setControl()
    {

        Time.timeScale = 1;

        player.enabled = true;
        demonPlayer.enabled = false;
    }
    IEnumerator StageAnimation()
    {
        SpriteRenderer[] renderers = stages[current_stage_index].GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
            if (!renderer.CompareTag("Flag"))
                renderer.transform.Translate(Vector2.down * 5);
        }

        foreach (var renderer in renderers)
        {
            while (true)
            {
                if (renderer.color.a >= 1)
                    break;

                renderer.color += new Color(0, 0, 0, 0.01f);
                if (!renderer.CompareTag("Flag"))
                    renderer.transform.Translate(Vector2.up * 5 / 100);
                yield return new WaitForSeconds(0.0001f);
            }
        }

        stage_clear = false;
        player.gameObject.SetActive(true);
        coroutine = null;
    }
    public int GetStageIndex()
    {
        return current_stage_index + 1;
    }
    public void spawnDemon()
    {
        GameObject demon = Instantiate(DemonPrefab);
        DemonPlayerController dpc = demon.GetComponent<DemonPlayerController>();
        dpc.myStage = current_stage_index; // 인덱스를 더한 뒤 불리기 때문에..
        dpc.startPosition = GameObject.Find("Spawn Point").transform.position;
        dpc.stage_number = current_stage_index;
        demon.SetActive(true);
    }
    public void onResetButtonClicked()
    {
        Time.timeScale = 2;
        player.enabled = false;
        demonPlayer.myStage = GetStageIndex();
        demonPlayer.startPosition = player.transform.position;
        demonPlayer.startRotation = player.transform.rotation;
        demonPlayer.enabled = true;
        stage_reset = true;
        GameObject[] demons = GameObject.FindGameObjectsWithTag("Demon");
        foreach (var demon in demons)
        {
            demon.GetComponent<DemonPlayerController>().resetStart();
        }

    }


}
