using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    NONE,
    SETUP,
    WAVE,
    MINIBOSS,
    BOSS,
    ENDGAME,
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GameObject wall;

    float bounds = 35;

    GameState state = GameState.NONE;

    int waveNumber = -1;
    float levelStartTime = 0;
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
        SwitchState(GameState.SETUP);
    }

    // Update is called once per frame
    void Update()
    {
        GameStateUpdate();
    }

    void SwitchState(GameState targetState)
    {
        print("switch state "+targetState.ToString() + " " + waveNumber);
        state = targetState;
        switch (state)
        {
            case GameState.SETUP:
                UIManager.instance.UpdateWaveText("Przygotowanie poziomu");
                //coins = SaveManager.instance.LoadCoinsAmount();
                //UIManager.instance.UpdateCoinsText();
                break;

            case GameState.WAVE:
                UIManager.instance.UpdateWaveText($"=== Fala {waveNumber} ===");
                lastWaveTime = Time.time;
                break;

            case GameState.MINIBOSS:
                UIManager.instance.UpdateWaveText("To jest fala Minibossa!");
                EnemyManager.instance.SpawnEnemy(true);
                break;

            case GameState.BOSS:
                UIManager.instance.UpdateWaveText("To jest fala Bossa!");
                break;

            case GameState.ENDGAME:
                UIManager.instance.UpdateWaveText("Wszystkie fale zakoñczone.");
                break;
        }
    }

    private void GameStateUpdate()
    {
        switch (state)
        {
            case GameState.SETUP:
                SetupWalls();
                SetupLevel();
                SwitchState(GameState.WAVE);
                break;

            case GameState.WAVE:
                WaveUpdate();
                break;

            case GameState.MINIBOSS:
                MinibossWaveUpdate();
                break;

            case GameState.BOSS:
                //currently skipped
                SwitchState(GameState.ENDGAME);
                break;

            case GameState.ENDGAME:
                break;
        }
    }

    void SetupLevel()
    {
        waveNumber = 0;
        levelStartTime = Time.time;
        UIManager.instance.UpdateWaveText($"=== Fala {waveNumber} ===");
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

    void MinibossWaveUpdate()
    {
        EnemySpawnCheck();
    }

    void BossWaveUpdate()
    {

    }

    private void GameEndCheck()
    {
        if (waveNumber >= 5)
        {
            SwitchState(GameState.ENDGAME);
        }
        else
        {
            SwitchState(GameState.WAVE);
        }
    }

    private void WaveProgressCheck()
    {
        if (Time.time > lastWaveTime + waveDuration)
        {
            SwitchState(GameState.MINIBOSS);
        }
    }

    private void ProgressWave()
    {
        waveNumber++;
    }

    private void EnemySpawnCheck()
    {
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
