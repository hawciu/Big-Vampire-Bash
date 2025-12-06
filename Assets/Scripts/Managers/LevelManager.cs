using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GameObject wall;

    float bounds = 35;

    float levelStartTime = 0;

    int waveNumber = -1;
    private float lastWaveTime = 0;
    private readonly float waveDuration = 5;
    private float lastEnemySpawn = 0f;
    private readonly float spawnCooldown = 0.5f;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update japa
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetupLevel()
    {
        SetupWalls();
        waveNumber = 0;
    }

    internal void StartWave()
    {
        lastWaveTime = Time.time;
        levelStartTime = Time.time;
    }

    void SetupWalls()
    {
        Vector3 nw = new Vector3(-bounds, 0, bounds);
        Vector3 ne = new Vector3(bounds, 0, bounds);
        Vector3 sw = new Vector3(-bounds, 0, -bounds);
        Vector3 se = new Vector3(bounds, 0, -bounds);
        BuildWall(nw, ne);
        BuildWall(ne, se);
        BuildWall(se, sw);
        BuildWall(sw, nw);
    }

    void BuildWall(Vector3 target, Vector3 rot)
    {
        GameObject tmp;
        tmp = Instantiate(wall, target, Quaternion.identity);
        tmp.transform.localScale = new Vector3(tmp.transform.localScale.x,
            tmp.transform.localScale.y,
            bounds * 2);//tmp.transform.localScale.z);
        tmp.transform.rotation = Quaternion.LookRotation(rot - tmp.transform.position);
        tmp.transform.position += (rot - target).normalized * bounds;
    }

    public float GetBounds()
    {
        return bounds;
    }

    public void WaveUpdate()
    {
        WaveProgressCheck();

        EnemySpawnCheck();
    }

    public void MinibossWaveUpdate()
    {
        EnemySpawnCheck();
    }

    public void BossWaveUpdate()
    {

    }

    private void GameEndCheck()
    {
        if (waveNumber >= 5)
        {
            GameManager.instance.SwitchState(GameState.ENDGAME);
        }
        else
        {
            GameManager.instance.SwitchState(GameState.WAVE);
        }
    }

    private void WaveProgressCheck()
    {
        if (GameManager.instance.IsGamePaused()) lastWaveTime += Time.deltaTime;

        if (Time.time > lastWaveTime + waveDuration)
        {
            GameManager.instance.SwitchState(GameState.MINIBOSS);
        }
    }

    private void ProgressWave()
    {
        waveNumber++;
        lastWaveTime = Time.time;
    }

    private void EnemySpawnCheck()
    {
        if (GameManager.instance.IsGamePaused()) lastEnemySpawn += Time.deltaTime;

        if (waveNumber != 5 && Time.time > lastEnemySpawn + spawnCooldown)
        {
            EnemyManager.instance.SpawnEnemy(false);
            lastEnemySpawn = Time.time;
        }
    }

    internal void OnMinibossDeath()
    {
        ProgressWave();
        GameEndCheck();
    }

    internal int GetWaveNumber()
    {
        return waveNumber;
    }
}
