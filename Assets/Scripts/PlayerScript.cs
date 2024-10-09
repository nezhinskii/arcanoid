using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public GameDataScript gameData;
    const int maxLevel = 30;
    [Range(1, maxLevel)]
    public int level = 1;
    AudioSource audioSrc;
    public AudioClip pointSound;
    public Dictionary<int, BallScript> ballScripts = new Dictionary<int, BallScript>();
    private GameManager gameManager;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
        else
        {
            StartCoroutine(WaitForGameManager());
        }
    }
    private IEnumerator WaitForGameManager()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }

        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.MainMenu:
                Time.timeScale = 0;
                break;
            case GameState.Paused:
                Time.timeScale = 0;
                break;
            case GameState.GameOver:
                Time.timeScale = 0;
                break;
            case GameState.Playing:
                if (gameManager.PreviousGameState != GameState.Paused)
                {
                    StartLevel();
                }
                Time.timeScale = 1;
                break;
            default:
                
                break;
        }
    }

    void Start()
    { 
        gameManager = GameManager.Instance;
        HandleGameStateChanged(gameManager.CurrentGameState);
    }

    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause"))
        {
            if (gameManager.CurrentGameState == GameState.Playing)
                gameManager.SetGameState(GameState.Paused);
            else
                gameManager.SetGameState(GameState.Playing);
        }

        if (gameManager.CurrentGameState != GameState.Playing) return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pos = transform.position;
        pos.x = mousePos.x;
        transform.position = pos;

        if (Input.GetKeyDown(KeyCode.M))
        {
            gameData.music = !gameData.music;
            SetMusic();
        }

        if (Input.GetKeyDown(KeyCode.S)) 
        {
            gameData.sound = !gameData.sound;
        }

        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     gameData.Reset();
        //     SceneManager.LoadScene("MainScene");
        // }
    }

    public float ballVelocityMult = 0.02f;
    public GameObject bluePrefab;
    public GameObject redPrefab;
    public GameObject greenPrefab;
    public GameObject yellowPrefab;
    public GameObject yellowPrefabB;
    public GameObject ballPrefab;
    static Collider2D[] colliders = new Collider2D[50];
    static ContactFilter2D contactFilter = new ContactFilter2D();

    void CreateBlocks(GameObject prefab, float xMax, float yMax, int count, int maxCount)
    {
        if (count > maxCount)
            count = maxCount;
        for (int i = 0; i < count; i++)
            for (int k = 0; k < 20; k++)
            {
                var obj = Instantiate(prefab, new Vector3((Random.value * 2 - 1) * xMax, Random.value * yMax, 0), Quaternion.identity);
                if (obj.GetComponent<Collider2D>().OverlapCollider(contactFilter.NoFilter(), colliders) == 0)
                    break;
                Destroy(obj);
            }
    }

    void CreateBalls()
    {
        int count = 2;
        if (gameData.balls == 1)
            count = 1;
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(ballPrefab);
            var ball = obj.GetComponent<BallScript>();
            ballScripts[ball.GetInstanceID()] = ball;
            ball.ballInitialForce += new Vector2(10 * i, 0);
            ball.ballInitialForce *= 1 + level * ballVelocityMult;
        }
    }

    void SetBackground()
    {
        var bg = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        bg.sprite = Resources.Load(level.ToString("d2"), typeof(Sprite)) as Sprite;
    }

    public void StartLevel()
    {
        var blocks = GameObject.FindGameObjectsWithTag("Ball");
        for (int i = 0; i < blocks.Length; i++)
        {
            Destroy(blocks[i]);
        }
       
        audioSrc = Camera.main.GetComponent<AudioSource>();
        level = gameData.level;
        SetMusic();
        SetBackground();
        var yMax = Camera.main.orthographicSize * 0.8f;
        var xMax = Camera.main.orthographicSize * Camera.main.aspect * 0.85f;
        CreateBlocks(bluePrefab, xMax, yMax, level, 8);
        CreateBlocks(redPrefab, xMax, yMax, 1 + level, 10);
        CreateBlocks(greenPrefab, xMax, yMax, 1 + level, 12);
        CreateBlocks(yellowPrefab, xMax, yMax, 2 + level, 15);
        CreateBlocks(yellowPrefabB, xMax, yMax, 2 + level, 15);
        CreateBalls();
    }

    IEnumerator BallDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
            if (gameData.balls > 0)
                CreateBalls();
            else
                gameManager.SetGameState(GameState.GameOver);
    }

    public void BallDestroyed(int ballId)
    {
        gameData.balls--;
        Debug.Log(ballScripts.Remove(ballId));
        StartCoroutine(BallDestroyedCoroutine());
    }

    IEnumerator BlockDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Block").Length == 0)
        {
            if (level < maxLevel)
                gameData.level++;
            gameManager.SetGameState(GameState.GameOver);
        }
    }

    IEnumerator BlockDestroyedCoroutine2()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.2f);
            audioSrc.PlayOneShot(pointSound, 5);
        }
    }

    public void AddPoints(int points)
    {
        gameData.points += points;
    }

    public void ChangeBallsMaterial(BallMaterial material)
    {
        foreach(BallScript ball in ballScripts.Values)
        {
            ball.ChangeMaterial(material);
        }
    }

    public void BlockDestroyed(int points)
    {
        gameData.points += points;
        if (gameData.sound)
            audioSrc.PlayOneShot(pointSound, 5);
        gameData.pointsToBall += points;
        if (gameData.pointsToBall >= requiredPointsToBall)
        {
            gameData.balls++;
            gameData.pointsToBall -= requiredPointsToBall;
            if (gameData.sound)
                StartCoroutine(BlockDestroyedCoroutine2());
        }
        StartCoroutine(BlockDestroyedCoroutine());
    }

    void SetMusic()
    {
        if (gameData.music)
            audioSrc.Play();
        else
            audioSrc.Stop();
    }

    int requiredPointsToBall{ get { return 400 + (level - 1) * 20; } }

}
