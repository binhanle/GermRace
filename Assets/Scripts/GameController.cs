﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {

        // set up the board
        GameObject boardObject = GameObject.Find("Board");
        Board board = boardObject.GetComponent<Board>();
        GameData.SetBoard(board);

        // initialize demo pieces
        Dictionary<string, GameObject> demoPieces = new Dictionary<string, GameObject>();
        foreach (string color in GameData.GetPieceColors())
        {
            demoPieces[color] = GameObject.Find(color);
        }
        GameData.SetDemoPieces(demoPieces);

        // show player count screen
        //GameGUI.ShowPlayerCountScreen();

        board.ShowMainMenu();
        //GameData.SetGameMode(GameData.Mode.NormalRoll);
        //board.RollDie(GameData.Mode.NormalRoll);
    }

    // Update is called once per frame
    void Update()
    {
        // point the camera at demo piece on setup
        if (GameData.GetGameMode() == GameData.Mode.Home)
        {
            Camera.main.transform.eulerAngles = GameData.GetHomeCameraRotation();
            Camera.main.transform.position = GameData.GetHomeCameraPosition();
        }

        // follow the current piece if active
        if (GameData.GetGameMode() == GameData.Mode.MovingPiece)
        {
            Vector3 piecePos = GameData.GetActivePiece().transform.position;
            piecePos.y = 0;
            Camera.main.transform.eulerAngles = GameData.GetMainCameraRotation();
            Camera.main.transform.position = piecePos + GameData.GetCameraOffset();
        }

        // focus on finish tile if someone wins
        if (GameData.GetGameMode() == GameData.Mode.Winner)
        {
            Tile finishTile = GameData.GetBoard().GetFinishTile();
            Vector3 tilePos = new Vector3(finishTile.GetPosition().x, 0, finishTile.GetPosition().y);
            Camera.main.transform.eulerAngles = GameData.GetWinCameraRotation();
            Camera.main.transform.position = tilePos + GameData.GetWinCameraOffset();
        }

        // switch to top view if necessary to select move
        if (GameData.GetGameMode() == GameData.Mode.SelectMove)
        {
            Camera.main.transform.eulerAngles = GameData.GetTopViewRotation();
            Camera.main.transform.position = GameData.GetTopViewOffset();
        }
    }
}
