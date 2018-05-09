using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameController : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {
        // die roll test
        GameObject boardObject = GameObject.Find("Board");
        Board board = boardObject.GetComponent<Board>();
        GameData.SetBoard(board);
        GameData.SetGameMode(GameData.Mode.NormalRoll);
        board.RollDie(GameData.Mode.NormalRoll);
	}

    // Update is called once per frame
    void Update()
    {
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
            Tile currTile = GameData.GetActivePiece().GetCurrTile();
            Vector3 tilePos = new Vector3(currTile.GetPosition().x, 0, currTile.GetPosition().y);
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
