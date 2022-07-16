using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private string boardDesign = "GDGGG\n" +
                                 "GGEGG\n" +
                                 "GGDGG\n" +
                                 "WWGGG\n" +
                                 "GGGGG";

    //G - Ground;
    //E - Empty;
    //D - Dice;

    public GameObject groundPrefab;
    public GameObject wallPrefab;
    public GameObject dicePrefab;
    public float buildSpeed = 100;
    public float creationSpeed = 0.5f;
    private int boardSize = 0;
    private string[] boardMap;
    private List<PlayerController> playerControllers;

    public GameObject[,] board;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BuildBoard(boardDesign));
        //StartCoroutine(DestroyBoard());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isValidPosition(int x, int y, int id)
    {
        if (x >= 0 && y >= 0 && x < boardMap.Length && y < boardMap.Length)
        {
            if (boardMap[x][y] != 'E' && boardMap[x][y] != 'W')
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

    private IEnumerator BuildBoard(string boardDesign)
    {
        playerControllers = new List<PlayerController>();
        boardMap = boardDesign.Split('\n');
        boardSize = boardMap.Length;
        board = new GameObject[boardSize, boardSize];
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
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
                yield return new WaitForSeconds(creationSpeed);
            }
        }
    }

    private IEnumerator DestroyBoard()
    {
        yield return new WaitForSeconds(15);
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (board[i,j] != null)
                {
                    StartCoroutine(MoveToPosition(board[i,j], new Vector3(-50, 0, j)));
                    Destroy(board[i, j], 5);
                    yield return new WaitForSeconds(creationSpeed);
                }
            }
        }
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
}
