using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{

    public void OnClick()
    {
        List<int> coord = new List<int>();
        coord =name.Split(' ').Select(Int32.Parse).ToList();

        Text textComp = GetComponentInChildren<Text>();
        if (GameManager.Instance.isPlayerTurn && textComp.text == " ")
        {
            textComp.text = "X";
            GameManager.Instance.isPlayerTurn = false;
            if(GameManager.Instance.CheckWin("X"))
                Debug.Log("Player Win"); ;
        }
  
    }
}
