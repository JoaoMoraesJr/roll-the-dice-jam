using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _rollSpeed = 5;
    public float delayToStart = 3;
    public int positionX = 0;
    public int positionY = 0;
    private bool _isMoving;
    public int top = 3;
    private int left = 6;
    private int forward = 5;
    private int right = 1;
    private int backward = 2;
    private int bottom = 4;
    public bool canMove = false;
    public GameManager board;
    public int id;

    public void StartPlayer(int initialX, int initialY)
    {
        positionX = initialX;
        positionY = initialY;
        this.id = initialX;
        Invoke("ActivateMovement", delayToStart);
    }

    private void ActivateMovement()
    {
        canMove = true;
    }

    public void StopPlayer()
    {
        canMove = false;
    }

    private void Update()
    {
        if (_isMoving || !canMove) return;

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
            }
        }




        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 0.5f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }
    }

    private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        _isMoving = true;
        for (var i = 0; i < 90 / _rollSpeed; i++)
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }
        _isMoving = false;
    }
}
