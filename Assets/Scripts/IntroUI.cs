using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    public GameObject room;

    public Text dialogText;

    public bool canStartGame = false;

    private string[] dialogs =
    {
        "Dad\nHey Skye, just got home! How are doing?",
        "Skye\nHi, nothing much to do... Just playing some videogame.",
        "Dad\nOoh, then I've got some good news! It's a new game for you!!",
        "Dad\nIt was really cheap from a weird man down the street... Anyway, looks fun!",
        "Skye\nWoow!! Exactly what I wanted! Thanks a lot dad!",
        "Dad\nTell me if this one is good later. Have fun!",
        "Skye\nLet's see what this game is about",
        "Skye\nWoow... What's going on? This dark energy... is pushing me inside!!!",
        "Skye\nAAAAAHHHH!!!"
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
        if (newDialog != currentDialog)
        {
            currentDialog = newDialog;
            dialogText.text = dialogs[currentDialog];
            Debug.Log(dialogs[currentDialog]);
        }

        if (canStartGame)
        {
            SceneManager.LoadScene("MainGame");
        }
    }

    public void StartButton()
    {
        room.GetComponent<Animator>().SetTrigger("PlayButton");
        room.GetComponent<Animator>().SetTrigger("KnockDoor");
    }


}
