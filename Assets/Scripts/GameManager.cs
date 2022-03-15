using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Xsprite;
    public GameObject Osprite;

    public GameObject ButtonPrefab;

    public GameObject[,] Board=new GameObject[3,3];

    public static GameManager Instance;

    public bool isPlayerTurn;

    Text ButtonText;

    public int difficulty = 3;

    private void Awake()
    {
        Instance = this;
        isPlayerTurn = true;
    }
    void Start()
    {
        GenerateGrid();

        ButtonText = ButtonPrefab.GetComponentInChildren<Text>();
    }


    private void Update()
    {

        if (!isPlayerTurn)
        {
            makeAiMove();
        }

            
    }

    void GenerateGrid()
    {
        for(int i=-1;i<2;i++)
        {
            for (int j = -1; j < 2; j++)
            {
                RectTransform rect = ButtonPrefab.GetComponent<RectTransform>();
                

                Vector2 pos = new Vector2(i*rect.rect.width+Screen.width/2, j * rect.rect.width+Screen.height/2);

                GameObject instantiateButton = Instantiate(ButtonPrefab);
                instantiateButton.transform.SetParent(transform);
                instantiateButton.name = i + " " + j;

                instantiateButton.transform.position = pos;


                instantiateButton.GetComponentInChildren<Text>().text = " ";

                Board[i + 1, j + 1] = instantiateButton;

            }
        }
    }


    public bool CheckWin(string symbol)
    {
        bool win = false;
        for(int i =0; i<3;i++)
        {
            if(CheckCol(i, symbol))
            {
                win = true;
            }
            else if(CheckRow(i, symbol))
            {
                win = true;
            }
        }

        return win;
    }
    bool CheckCol(int col,string symbol)
    {
        int sum = 0;
        for(int i = 0;i<3;i++)
        {
            if (Board[col, i].GetComponentInChildren<Text>().text == symbol)
                sum++;
        }

        if (sum == 3)
            return true;
        else return false;
    }

    bool CheckRow(int row, string symbol)
    {
        int sum = 0;
        for (int i = 0; i < 3; i++)
        {
            if (Board[i, row].GetComponentInChildren<Text>().text == symbol)
                sum++;
        }

        if (sum == 3)
            return true;
        else return false;
    }



    public void makeAiMove()
    {
        int bestVal = -11;
        int bestMove= -1;
        char[] newBoard = new char[9];


        int index = 0;
        foreach(var elem in Board)
        {
            newBoard[index] = char.Parse(elem.GetComponentInChildren<Text>().text);
            index++;
        }

        for (int i = 0; i < newBoard.Length; i++)
        {
            if (newBoard[i] == ' ')
            {
                newBoard[i] = 'O';
                int value = Minimax(newBoard, difficulty, false);

                if (value > bestVal)
                {
                    bestVal = value;
                    bestMove = i;
                }
                newBoard[i] = ' ';
            }
        }

        setValue(bestMove, 'O');

        if(CheckWin("O"))
            Debug.Log("Bot Win");

        isPlayerTurn = !isPlayerTurn;
    }

    private void setValue(int bestMove, char v)
    {
        int index = 0;
        foreach(var elem in Board)
        {
            if(bestMove == index)
            {
                elem.GetComponentInChildren<Text>().text = v.ToString();
            }
            index++;
        }
    }

    
    int Minimax(char[] board,  int depth , bool maximizingPlayer)
    {
        int score = scoreBoard(board, depth);
        if (depth == 0 || isTerminating(board) || score != 0)
            return score;
        if (maximizingPlayer)
        {
            int value = -10;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == ' ')
                {
                    board[i] = 'O';

                    value = Math.Max(value, Minimax(board, depth - 1, false));
                    Debug.Log("O:" + value.ToString());
                    board[i] = ' ';
                }
            }
            return value;
        }
        else
        {
            int value = 10;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == ' ')
                {
                    board[i] = 'X';
                    value = Math.Min(value, Minimax(board, depth - 1, true));
                    Debug.Log("X:" + value.ToString());
                    board[i] = ' ';
                }
            }
            return value;
        }
    }


    int scoreBoard(char[] board, int depth)
    {
        string currentPlayer = "O";
        for (int i = 0; i < 4; i++)
        {
            if (CheckArrayWin(board,char.Parse(currentPlayer)).Length == 3)
            {
                if (currentPlayer == "O")
                    return 10 - (difficulty - depth);
                else
                    return -10 + (difficulty - depth);
            }
            currentPlayer = "X";
        }
        return 0;
    }

    bool isTerminating(char[] board)
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == ' ')
                return false;
        }
        return true;
    }

    int[] CheckArrayWin(char[] board , char player )
    {
        int[] winsequence = new int[3];
        int[,] PosToCheck = new int[,]{ { 0, 1, 2 }, { 3, 4 ,5}, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 } };

        for(int i = 0; i <PosToCheck.Length; i ++ )
        {
            int sum = 0;

            for(int j = 0;i<3;i++)
            {
                if(board[PosToCheck[i, j]] == player)
                {
                    sum++;
                }
            }
            if(sum == 3)
            {
                for (int x = 0; x < 3; x++)
                {
                    winsequence[i] = PosToCheck[i, x];
                }
            }
        }
        return winsequence;
    }

}
