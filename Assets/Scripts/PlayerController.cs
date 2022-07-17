using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 25; //350 for rolls on Update
    public int positionX = 0;
    public int positionY = 0;
    private bool isMoving;
    public int top = 3;
    private int left = 6;
    private int forward = 5;
    private int right = 1;
    private int backward = 2;
    private int bottom = 4;
    public bool canMove = false;
    public GameManager board;
    public int id;
    public AudioSource moveSound;

    //private float currentAngle = 0;
    //private Vector3 anchor;
    //private Vector3 axis;

    public void StartPlayer(int initialX, int initialY)
    {
        positionX = initialX;
        positionY = initialY;
        this.id = initialX;
    }

    public void ActivateMovement()
    {
        canMove = true;
    }

    public void StopPlayer()
    {
        canMove = false;
    }

    private void Update()
    {
        //if (!canMove) return;

        //if (isMoving)
        //{
        //    currentAngle = currentAngle + rollSpeed * Time.deltaTime;
        //    transform.RotateAround(anchor, axis, rollSpeed * Time.deltaTime);
        //    if (currentAngle >= 90)
        //    {
        //        isMoving = false;
        //        currentAngle = 0;
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}

        if (isMoving || !canMove) return;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (board.isValidPosition(positionX - 1, positionY, id))
            {
                Assemble(Vector3.left);
                int aux = left;
                left = top;
                top = right;
                right = bottom;
                bottom = aux;
                positionX--;
                moveSound.Play();
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (board.isValidPosition(positionX + 1, positionY, id))
            {
                Assemble(Vector3.right);
                int aux = right;
                right = top;
                top = left;
                left = bottom;
                bottom = aux;
                positionX++;
                moveSound.Play();
            }
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (board.isValidPosition(positionX, positionY + 1, id))
            {
                Assemble(Vector3.forward);
                int aux = forward;
                forward = top;
                top = backward;
                backward = bottom;
                bottom = aux;
                positionY++;
                moveSound.Play();
            }
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (board.isValidPosition(positionX, positionY - 1, id))
            {
                Assemble(Vector3.back);
                int aux = backward;
                backward = top;
                top = forward;
                forward = bottom;
                bottom = aux;
                positionY--;
                moveSound.Play();
            }
        }
        if(board.boardMap[positionX][positionY] =='E')
        {
            gameObject.AddComponent<Rigidbody>();
            StopPlayer();
            board.ResetStage();
        }
    }
    void Assemble(Vector3 dir)
    {
        var anchor = transform.position + (Vector3.down + dir) * 0.5f;
        var axis = Vector3.Cross(Vector3.up, dir);
        //isMoving = true;
        StartCoroutine(Roll(anchor, axis));
    }

    private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        isMoving = true;
        for (var i = 0; i < 90 / rollSpeed; i++)
        {
            transform.RotateAround(anchor, axis, rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }
        isMoving = false;
    }
}
