using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // die roll test
        GameObject boardObject = GameObject.Find("Board");
        Board board = boardObject.GetComponent<Board>();
        GameData.SetGameMode(GameData.Mode.NormalRoll);
        board.RollDie();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
