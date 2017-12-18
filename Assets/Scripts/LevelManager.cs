using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Spawns the player, and 
/// </summary>
public class LevelManager : Singleton<LevelManager>
{
    [Header("玩家 X 軸移動的邊界值")]
    public float MinBound = -3f;
    public float MaxBound = 3f;

    [Header("能生成的敵人種類")]
    public List<GameObject> Enemys;
    [Header("生成的敵人間個時間")]
    public float EnemyDelay = 5f;
    protected float _Time = 0f;
    [Header("生成的敵人啟始與結束位置")]
    public float EnemyStart = 6f;
    public float EnemyEnd = -6f;
    [Header("生成的敵人隨機 X 軸")]
    public bool EnemyRandX = true;

    [Header("一次生成的敵人次數")]
    public int EnemyRandCount = 1;

    [Header("玩家隻數")]
    public int Lifes = 3;

    [Header("遊戲結束時間")]
    public float EndTime = 60f;

    // 時間中終止到指定關卡
    [Space(10)]
    [Header("時間中終止到指定關卡")]
    public string NextLevel = "Level2";

    // 死亡後到指定關卡
    [Space(10)]
    [Header("死亡後到指定關卡")]
    public string ResetLevel = "Level1";

    // 到達終點後到指定關卡
    [Space(10)]
    [Header("到達終點後到指定關卡")]
    public string EndLevel = "Level1";

    [Header("Prefabs")]
    public GameObject StartingPosition;

    [Header("河岸")]
    public BackGroundLoop RiverBank;
    [Header("水底")]
    public BackGroundLoop Underwater;

    /// the playable characters - use this to tell what characters you want in your level, don't access that at runtime
    /// 指定玩家 Prefabs 用來動態生成玩家的 GameObject
    public PlayableCharacter PlayableCharacters;

    /// the playable characters currently instantiated in the game - use this to know what characters ARE currently in your level at runtime
    /// 玩家動態生成的 GameObject
    public PlayableCharacter CurrentPlayableCharacters { get; set; }

    [Header("玩家是否自動移動")]
    public bool AotuMove = true;

    [Header("玩家過關需要移動的距離")]
    public int Distance = 999;

    // 距離減一
    public virtual void LessDistance()
    {
        Distance -= 1;
        GUIManager.Instance.UpdateDistance(Distance);

        if (Distance <= 0)
        {
            // 到達終點
            GoToEndLevel();
        }
    }

    // 水流是否流動
    public virtual void WaterFlows(bool flows)
    {
        if (RiverBank != null)
        {
            if (flows == true)
            {
                RiverBank.Play();
            }
            else
            {
                RiverBank.Stop();
            }
            //RiverBank.enabled = flows;
        }

        if (Underwater != null)
        {
            if (flows == true)
            {
                Underwater.Play();
            }
            else
            {
                Underwater.Stop();
            }
            //Underwater.enabled = flows;
        }
    }

    public virtual void LossLifes()
    {
        Lifes -= 1;
        
        if (Lifes <= 0)
        {
            GoToResetLevel();
        }
        else
        {
            GUIManager.Instance.UpdateLifes(Lifes);
        }
    }

    public virtual void ObtainLifes()
    {
        Lifes += 1;

        GUIManager.Instance.UpdateLifes(Lifes);
    }

    /// <summary>
    /// Gets the player to the specified level
    /// </summary>
    /// <param name="levelName">Level name.</param>
    public virtual void GotoLevel(string levelName)
    {
        StartCoroutine(GotoLevelCo(levelName));
    }

    public virtual void GoToNextLevel()
    {
        StartCoroutine(GotoLevelCo(NextLevel));
    }

    public virtual void GoToResetLevel()
    {
        StartCoroutine(GotoLevelCo(ResetLevel));
    }

    public virtual void GoToEndLevel()
    {
        StartCoroutine(GotoLevelCo(EndLevel));
    }

    /// <summary>
    /// Waits for a short time and then loads the specified level
    /// </summary>
    /// <returns>The level co.</returns>
    /// <param name="levelName">Level name.</param>
    protected virtual IEnumerator GotoLevelCo(string levelName)
    {
        yield return new WaitForSecondsRealtime(1f);

        if (string.IsNullOrEmpty(levelName))
        {
            yield return null;
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }

    }

    /// <summary>
    /// Initialization
    /// </summary>
    protected virtual void Start()
    {
        InstantiateCharacters();
    }

    protected virtual IEnumerator StartCoroutineInstantiateEnemys(GameObject obj)
    {
        yield return new WaitForSecondsRealtime(1f);

        InstantiateEnemys();
    }
    protected virtual void Update()
    {
        GUIManager.Instance.UpdateLifes(Lifes);
        GUIManager.Instance.UpdateDistance(Distance);

        if (_Time != 0)
        {
            if (Time.time - _Time > EnemyDelay)
            {
                InstantiateEnemys();

                for (int i = 1; i < EnemyRandCount; ++i)
                {
                    StartCoroutine(StartCoroutineInstantiateEnemys(gameObject));
                }
            }
        }
        else
        {
            _Time = Time.time;
        }
    }

    protected virtual void InstantiateEnemys()
    {
        if (Enemys == null)
        {
            return;
        }

        if (Enemys.Count == 0)
        {
            return;
        }

        int i_ = Random.Range(0, Enemys.Count);
        GameObject instance_ = (GameObject)Instantiate( Enemys[i_] );

        float x_ = 0f;
        if (EnemyRandX == true)
        {
            // 隨機敵人的 X 軸
            x_ = Random.Range(MinBound, MaxBound);
        }
        instance_.transform.position = new Vector3( x_, EnemyStart, instance_.transform.position.z);// 魚 z 是 -3 可以蓋住魚

        _Time = 0f;
    }

    /// <summary>
    /// Instantiates all the playable characters and feeds them to the gameManager
    /// </summary>
    protected virtual void InstantiateCharacters()
    {
        if (PlayableCharacters == null)
        {
            return;
        }

        PlayableCharacter instance_ = (PlayableCharacter)Instantiate(PlayableCharacters);
        // we position it based on the StartingPosition point
        instance_.transform.position = new Vector3(StartingPosition.transform.position.x, StartingPosition.transform.position.y, instance_.transform.position.z);// z 用原來的 Prefabs 的

        // we feed it to the game manager
        CurrentPlayableCharacters = instance_;
    }


}
