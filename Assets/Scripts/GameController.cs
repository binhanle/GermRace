using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    GameObject boardObject;

    //DOCUMENT
    //adjust camera so that the frustum only includes 
    static private void FrustumAdjustment(GameObject gamePart)
    {
        //the scale of the game object which will determine the frustum size
        Vector3 gamePartScale = gamePart.transform.localScale;

        //minimum height of the frustum
        float frustumHeight = gamePartScale.z;

        //minimum width of the frustum
        float frustumWidth = gamePartScale.x;

        //offset distance for camera
        float distance = 0;

        //keep track of the current frustum height and width with these variables
        var currentFrustumHeight = 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var currentFrustumWidth = currentFrustumHeight * Camera.main.aspect;

        //adjust the distance of the camera until both height and width frustum requirements are satisfied
        while (currentFrustumHeight < frustumHeight || currentFrustumWidth < frustumWidth)
        {
            distance++;
            currentFrustumHeight = 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            currentFrustumWidth = currentFrustumHeight * Camera.main.aspect;
        }

        //transform camera position 
        Camera.main.transform.position = new Vector3(gamePart.transform.position.x, gamePart.transform.position.y + distance, gamePart.transform.position.z);
    }

    // Use this for initialization
    void Start()
    {

        // set up the board
        boardObject = GameObject.Find("Board");
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
            FrustumAdjustment(boardObject);
            //Camera.main.transform.position = GameData.GetTopViewOffset();
        }
    }
}
