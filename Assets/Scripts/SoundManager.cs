using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioSource breakRocksSound;
    [SerializeField] private AudioSource createBoardSound;
    [SerializeField] private AudioSource winSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBreakRocks()
    {
        breakRocksSound.Play();
    }

    public void StartCreateBoardSound()
    {
        createBoardSound.Play();
    }

    public void StopCreateBoardSound()
    {
        createBoardSound.Stop();
    }

    public void PlayWinSound()
    {
        winSound.Play();
    }
}
