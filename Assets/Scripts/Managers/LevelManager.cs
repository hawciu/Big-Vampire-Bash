using UnityEngine;

public enum GameState
{
    NONE, SETUP, PREPARE_WAVE, WAVE, MINIBOSS, BOSS, ENDGAME, ENDLESS
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Level Boundaries")]
    public GameObject wall;
    private readonly float bounds = 35;

    [Header("Game State")]
    private GameState state = GameState.NONE;
    public bool endlessMode = false;

    [Header("Wave Settings")]
    public float spawnCooldown = 0.5f;
    private readonly float waveDuration = 5;
    private int waveNumber = -1;
    private float levelStartTime = 0;
    private float lastWaveTime = 0;
    private float lastEnemySpawn = 0f;

    [Header("Endless Scaling")]
    public float endlessHPGainPerSecond = 0.5f;
    public float endlessSpeedGainPerSecond = 0.01f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SwitchState(GameState.SETUP);
    }

    private void Update()
    {
        GameStateUpdate();
    }

    private void SwitchState(GameState targetState)
    {
        state = targetState;

        switch (state)
        {
            case GameState.SETUP:
                UIManager.instance.UpdateWaveText("Przygotowanie poziomu");
                break;

            case GameState.PREPARE_WAVE:
                UIManager.instance.UpdateWaveText($"Przygotowanie fali {waveNumber}");
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

            case GameState.ENDLESS:
                UIManager.instance.UpdateWaveText("ENDLESS MODE");
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

                if (endlessMode)
                {
                    SwitchState(GameState.ENDLESS);
                }
                else
                {
                    SwitchState(GameState.PREPARE_WAVE);
                }
                break;

            case GameState.PREPARE_WAVE:
                PrepareWave();
                SwitchState(GameState.WAVE);
                break;

            case GameState.WAVE:
                WaveUpdate();
                break;

            case GameState.MINIBOSS:
                EnemySpawnCheck();
                break;

            case GameState.BOSS:
                SwitchState(GameState.ENDGAME);
                break;

            case GameState.ENDLESS:
                EndlessUpdate();
                break;
        }
    }

    private void SetupLevel()
    {
        waveNumber = 0;
        levelStartTime = Time.time;
    }

    private void PrepareWave()
    {
        lastWaveTime = Time.time;
        lastEnemySpawn = Time.time;
    }

    private void SetupWalls()
    {
        Vector3 nw = new(-bounds, 0, bounds);
        Vector3 ne = new(bounds, 0, bounds);
        Vector3 sw = new(-bounds, 0, -bounds);
        Vector3 se = new(bounds, 0, -bounds);
        BuildWall(nw, ne);
        BuildWall(ne, se);
        BuildWall(se, sw);
        BuildWall(sw, nw);
    }

    private void BuildWall(Vector3 target, Vector3 rot)
    {
        GameObject tmp = Instantiate(wall, target, Quaternion.identity);
        tmp.transform.localScale = new Vector3(tmp.transform.localScale.x, tmp.transform.localScale.y, bounds * 2);
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

    private void WaveProgressCheck()
    {
        if (Time.time > lastWaveTime + waveDuration)
        {
            SwitchState(GameState.MINIBOSS);
        }
    }

    private void EnemySpawnCheck()
    {
        if (waveNumber != 5 && Time.time > lastEnemySpawn + spawnCooldown)
        {
            EnemyManager.instance.SpawnEnemy(false);
            lastEnemySpawn = Time.time;
        }
    }

    private void EndlessUpdate()
    {
        if (Time.time > lastEnemySpawn + spawnCooldown)
        {
            float t = Time.time - levelStartTime;
            float scaledHP = t * endlessHPGainPerSecond;
            float scaledMove = 1f + (t * endlessSpeedGainPerSecond);

            EnemyManager.instance.EndlessSpawnUpdate(scaledHP, scaledMove);

            lastEnemySpawn = Time.time;
        }
    }

    internal void OnMinibossDeath()
    {
        waveNumber++;
        if (waveNumber >= 5)
        {
            SwitchState(GameState.ENDGAME);
        }
        else
        {
            SwitchState(GameState.PREPARE_WAVE);
        }
    }

    internal int GetWaveNumber()
    {
        return waveNumber;
    }
}
