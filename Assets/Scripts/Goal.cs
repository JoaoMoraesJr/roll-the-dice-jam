using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int goalNumber = 0;
    public int positionX = 0;
    public int positionY = 0;
    private bool completed = false;
    [SerializeField] private GameObject num1;
    [SerializeField] private GameObject num2;
    [SerializeField] private GameObject num3;
    [SerializeField] private GameObject num4;
    [SerializeField] private GameObject num5;
    [SerializeField] private GameObject num6;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGoal(int goalNumber, int positionX, int positionY)
    {
        this.goalNumber = goalNumber;
        this.positionX = positionX;
        this.positionY = positionY;

        switch (goalNumber)
        {
            case 1:
                num1.SetActive(true);
                break;
            case 2:
                num2.SetActive(true);
                break;
            case 3:
                num3.SetActive(true);
                break;
            case 4:
                num4.SetActive(true);
                break;
            case 5:
                num5.SetActive(true);
                break;
            case 6:
                num6.SetActive(true);
                break;
        }

    }

    public void GoalCompleted()
    {
        if (completed) return;
        else completed = true;
    }
}
