using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string boardDesign = "GDGGG\n" +
                                 "GGEGG\n" +
                                 "GGDGG\n" +
                                 "WWGGG\n" +
                                 "0GGGG";

    //G - Ground;
    //E - Empty;
    //D - Dice;

    public GameObject groundPrefab;
    public GameObject wallPrefab;
    public GameObject dicePrefab;
    public GameObject goalPrefab;
    public float shakeStrenght = 5;
    public float buildSpeed = 100;
    public float creationSpeed = 0.5f;
    private int boardSizeX = 0;
    private int boardSizeY = 0;
    public string[] boardMap;
    private List<PlayerController> playerControllers;
    private List<Goal> goals;
    private string[] maps;
    public int currentMap = 0;
    private bool isGameStarted = false;
    private bool isBoardClear = true;
    public GameObject camera;
    public SoundManager sounds;
    public Animator UIAnimator;
    public bool dialogFinished = false;

    public GameObject[,] board;

    // Start is called before the first frame update
    void Start()
    {
        maps = GetComponent<BoardMaps>().GetMaps();
        boardDesign = maps[currentMap];
        //StartCoroutine(BuildBoard());
        //StartCoroutine(DestroyBoard());
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameStarted)
        {
            checkGoals();
        }
    }

    public void StartGame ()
    {
        StartCoroutine(BuildBoard());
    }

    public bool isValidPosition(int x, int y, int id)
    {
        if (x >= 0 && y >= 0 && x < boardSizeX && y < boardSizeY)
        {
            if (boardMap[x][y] != 'W')
            {
                for (int i = 0; i < playerControllers.Count; i++)
                {
                    if (x == playerControllers[i].positionX && y == playerControllers[i].positionY)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        return false;
    }

    public void checkGoals()
    {
        int completedGoals = 0;
        for (int i = 0; i < goals.Count; i++)
        {
            for (int j = 0; j < playerControllers.Count; j++)
            {
                if (goals[i].positionX == playerControllers[j].positionX && goals[i].positionY == playerControllers[j].positionY)
                {
                    if (goals[i].goalNumber == 0 || goals[i].goalNumber == playerControllers[j].top)
                    {
                        completedGoals++;
                    }
                }
            }
        }
        if (goals.Count > 0 && completedGoals == goals.Count)
        {
            sounds.PlayWinSound();
            StartCoroutine(nextStage());
        }
    }

    private IEnumerator nextStage()
    {
        isGameStarted = false;
        for (int i = 0; i < playerControllers.Count; i++)
        {
            playerControllers[i].StopPlayer();
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(DestroyBoard());
        yield return new WaitUntil(() => isBoardClear == true);
        currentMap++;
        if (currentMap < maps.Length)
        {
            boardDesign = maps[currentMap];
            StartCoroutine(BuildBoard());
        }else
        {
            Debug.Log("Finished");
            UIAnimator.SetTrigger("FinalDialogue");
        }
    }

    public void ResetStage()
    {
        currentMap--;
        ShakeStage();
        StartCoroutine(nextStage());
    }

    private void ShakeStage()
    {
        sounds.PlayBreakRocks();
        for (int i = 0; i < boardSizeX; i++)
        {
            for (int j = 0; j < boardSizeY; j++)
            {
                if (board[i,j] != null)
                {
                    board[i, j].AddComponent<BoxCollider>();
                    var rb = board[i, j].AddComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * shakeStrenght);
                }
            }
        }
    }

    private IEnumerator BuildBoard()
    {
        isBoardClear = false;
        playerControllers = new List<PlayerController>();
        goals = new List<Goal>();
        boardMap = boardDesign.Split('\n');
        boardSizeX = boardMap.Length;
        boardSizeY = boardMap[0].Length;
        board = new GameObject[boardSizeX, boardSizeY];
        StartCoroutine(MoveToPosition(camera, new Vector3(((float)boardSizeX-1) / 2, camera.transform.position.y, camera.transform.position.z)));
        Invoke("StartCreateBoardSound", 1f);
        for (int i = 0; i < boardSizeX; i++)
        {
            for (int j = 0; j < boardSizeY; j++)
            {
                if (boardMap[i][j] == 'G')
                {
                    GameObject ground = Instantiate(groundPrefab, new Vector3(50, 0, j), Quaternion.identity);
                    ground.transform.Rotate(90, 0, 0);
                    StartCoroutine(MoveToPosition(ground, new Vector3 (i, 0, j)));
                    board[i, j] = ground;
                }
                if (boardMap[i][j] == 'W')
                {
                    GameObject wall = Instantiate(wallPrefab, new Vector3(50, 0, j), Quaternion.identity);
                    StartCoroutine(MoveToPosition(wall, new Vector3(i, 0, j)));
                    board[i, j] = wall;
                }
                if (boardMap[i][j] == 'D')
                {
                    GameObject dice = Instantiate(dicePrefab, new Vector3(50, 0, j), Quaternion.identity);
                    StartCoroutine(MoveToPosition(dice, new Vector3(i, 0, j)));
                    var controller = dice.GetComponentInChildren<PlayerController>();
                    controller.StartPlayer(i, j);
                    controller.board = this;
                    playerControllers.Add(dice.GetComponentInChildren<PlayerController>());
                    board[i, j] = dice;
                }
                if (isGoalTile(i,j))
                {
                    GameObject goal = Instantiate(goalPrefab, new Vector3(50, 0, j), Quaternion.identity);
                    var goalController = goal.GetComponent<Goal>();
                    goalController.StartGoal(boardMap[i][j]-'0', i, j);
                    goals.Add(goalController);
                    StartCoroutine(MoveToPosition(goal, new Vector3(i, 0, j)));
                    board[i, j] = goal;
                }
                yield return new WaitForSeconds(creationSpeed);
            }
        }
        isGameStarted = true;
        for (int i = 0; i < playerControllers.Count; i++)
        {
            playerControllers[i].ActivateMovement();
        }
        //sounds.StopCreateBoardSound();
        Invoke("StopCreateBoardSound", 1f);
    }

    private IEnumerator DestroyBoard()
    {
        StartCreateBoardSound();
        for (int i = 0; i < boardSizeX; i++)
        {
            for (int j = 0; j < boardSizeY; j++)
            {
                if (board[i,j] != null)
                {
                    StartCoroutine(MoveToPosition(board[i,j], new Vector3(-50, 0, j)));
                    Destroy(board[i, j], 3);
                    yield return new WaitForSeconds(creationSpeed);
                }
            }
        }
        StopCreateBoardSound();
        isBoardClear = true;
    }

    private IEnumerator MoveToPosition (GameObject obj, Vector3 targetPosition)
    {
        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.001f)
        {
            var step = buildSpeed * Time.deltaTime;
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, step);
            yield return new WaitForEndOfFrame();
        }
    }

    private bool isGoalTile(int x, int y)
    {
        if (boardMap[x][y] == '0' || boardMap[x][y] == '1' || boardMap[x][y] == '2' || boardMap[x][y] == '3' || boardMap[x][y] == '4' || boardMap[x][y] == '5' || boardMap[x][y] == '6')
        {
            return true;
        }
        return false;
    }

    void StartCreateBoardSound()
    {
        sounds.StartCreateBoardSound();
    }

    void StopCreateBoardSound()
    {
        sounds.StopCreateBoardSound();
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Credits");
    }

}
