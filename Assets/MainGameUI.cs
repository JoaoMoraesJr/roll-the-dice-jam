using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour
{
    public GameManager game;

    public bool gameStarted = false;
    public bool gameFinished = false;

    public bool startedEndGameDialog = false;

    public Text dialogText;

    private string[] dialogs =
    {
        "Skye\nWhere am I? What a weird place is this?",
        "???\nHey, you silly little girl! Welcome to my universe MWAHAHA",
        "Skye\nWHAT!? Who are you to talk to me like that!?!? And where am I?",
        "Sr. RNG\nI'm the nightmare of all players, the randomness in person, the entropy... call me SR. RNG! MWAHAHA",
        "Sr. RNG\nAnd you are locked in my universe until you win my ultimate logic test!",
        "Skye\nWhat are you talking about? Just get me out of here!!",
        "Sr. RNG\nLet's start the chaos MWAHAHA Bad luck to you!"
    };

    private string[] dialogsEndGame =
    {
        "Sr. RNG\nNOOOO! How could you win me in my own game? You lucky girl!!!",
        "Skye\nThat's not luck, that's only skill! My dices are deterministic and your chaotic randomness will not interfere!",
        "Skye\nNow take this! The power of Skye's Dice!!!",
        "Sr.RNG\nNOOOOO, I WILL RETURN!!!",
        "Skye\nUff... that was hard...",
        "Skye\nI can finally return home safe :)",
    };

    public int newDialog = 0;
    private int currentDialog = -1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameFinished)
        {
            game.EndGame();
        }
        if (newDialog >= dialogs.Length)
        {
            if (!gameStarted)
            {
                gameStarted = true;
                game.StartGame();
                currentDialog = -1;
            }
        }
        else if (startedEndGameDialog && newDialog != currentDialog)
        {
            currentDialog = newDialog;
            dialogText.text = dialogsEndGame[currentDialog];
        }
        else if (newDialog != currentDialog)
        {
            currentDialog = newDialog;
            dialogText.text = dialogs[currentDialog];
            Debug.Log(dialogs[currentDialog]);
        }
    }


}
