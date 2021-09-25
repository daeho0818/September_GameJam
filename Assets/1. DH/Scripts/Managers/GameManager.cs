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

    GameObject WindZone = null;
    private int all_stage_count => stages.Length;
    public int current_stage_index { get; set; } = 0;

    public bool stage_clear = false;
    public bool stage_start = false;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        stages[0].SetActive(true);
        player.gameObject.SetActive(false);
        player.transform.position = GameObject.Find("Spawn Point").transform.position;

        StartCoroutine(StageAnimation());

        cameraActor = Camera.main.GetComponent<CameraActor>();
        // StartCoroutine(StartCamAct());
    }

    void Update()
    {
        if (stage_clear)
        {
            spawnDemon();
            // SoundManager.Instance.SoundPlay(SoundManager.Instance.stage_clear);
            LoadNextStage();
            // StartCoroutine(StartCamAct());
        }
    }
    IEnumerator StartCamAct()
    {
        yield return StartCoroutine(cameraActor.Zoom(true, player.transform.position + Vector3.up * 2, 3, 1));
        yield return new WaitForSeconds(1);
        StartCoroutine(cameraActor.Zoom(false, Vector2.zero, 5, 0.5f));
    }
    void LoadNextStage()
    {
        if (current_stage_index + 1 < all_stage_count)
        {
            if (current_stage_index >= 0)
                stages[current_stage_index++].SetActive(false);
            else current_stage_index++;
            stages[current_stage_index].SetActive(true);
            player.gameObject.SetActive(false);

            player.transform.position = GameObject.Find("Spawn Point").transform.position;

            WindZone = player.playerAct.WindZone;

            if (WindZone)
            {
                WindZone.tag = "WindZone";
                WindZone.transform.SetParent(stages[current_stage_index].transform);
                player.playerAct.WindZone = null;
            }

            stage_clear = false;

            StartCoroutine(StageAnimation());
        }
        else

        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
        }
    }
    public void LoadPastStage()
    {
        stages[current_stage_index].SetActive(false);
        current_stage_index -= 2;
        LoadNextStage();
    }

    IEnumerator StageAnimation()
    {
        SpriteRenderer[] renderers = stages[current_stage_index].GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
            renderer.transform.Translate(Vector2.down * 5);
        }

        foreach (var renderer in renderers)
        {
            while (true)
            {
                if (renderer.color.a >= 1)
                    break;

                renderer.color += new Color(0, 0, 0, 0.01f);
                renderer.transform.Translate(Vector2.up * 5 / 100);
                yield return new WaitForSeconds(0.0001f);
            }
        }
        player.gameObject.SetActive(true);
    }
    public int GetStageIndex()
    {
        return current_stage_index + 1;
    }
    public void spawnDemon()
    {
        GameObject demon = Instantiate(DemonPrefab);
        DemonPlayerController dpc = demon.GetComponent<DemonPlayerController>();
        dpc.myStage = current_stage_index + 1;
        dpc.startPosition = GameObject.Find("Spawn Point").transform.position;
        demon.SetActive(true);
    }
}
