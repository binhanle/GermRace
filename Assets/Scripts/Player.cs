using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    //private static int playerCount = 0;
    //private int playerID;
    private string playerName;
    private Character[] playerPieces;
    private static Character activePiece;
    private const int NumPieces = 1;
    //private const string charDir = "Mushroomboypack1.2/3D/";
    private static Tile startTile;
    private static int numPlayers;

    public string GetName()
    {
        // returns the name of the player
        return playerName;
    }

    public void SetName(string name)
    {
        // sets the name of the player
        playerName = name;
    }

    public void SetupPieces(string charName)
    {
        // sets up the player's pieces using specified character
        for (int i = 0; i < NumPieces; i++)
        {
            // create the piece
            GameObject pieceObject = Instantiate((GameObject)Resources.Load(GameData.GetCharDir() + charName, typeof(GameObject)));
            pieceObject.AddComponent<Character>();
            Character piece = pieceObject.GetComponent<Character>();

            // rotate the piece 90 degrees clockwise
            pieceObject.transform.Rotate(new Vector3(0, 90, 0));

            // add piece to the piece list
            playerPieces[i] = piece;
            piece.MoveToTile(startTile);
            //piece.MoveToNextTile(0);
            //piece.JumpToNextTile(0);
            //piece.DoFinish();
            //piece.MoveSpaces(6, 0);
        }
    }
    
    /*public Character getCharacter()
    {
        // returns the character of this player
        return playerPiece;
    }*/
    
    public void Move(int numSpaces, int pathIndex)
    {
        // moves character based on die roll
        //playerPiece.Move(x, y);
        activePiece = playerPieces[0];
        GameData.SetActivePiece(activePiece);
        activePiece.MoveSpaces(numSpaces, pathIndex);
    }

    public static void SetStartTile(Tile tile)
    {
        // sets start tile
        startTile = tile;
    }

    public static Character GetActivePiece()
    {
        // gets the active piece
        return activePiece;
    }

    public static void SetActivePiece(Character piece)
    {
        // sets the active piece
        activePiece = piece;
    }

    public bool IsAllDone()
    {
        // returns true if all of the player's pieces reached the end
        foreach (Character piece in playerPieces)
        {
            if (!piece.IsDone())
            {
                return false;
            }
        }
        return true;
    }

    public void Celebrate()
    {
        // makes all the player's pieces dance
        foreach (Character piece in playerPieces)
        {
            piece.DoHappy();
        }
    }

    public void Awake()
    {
        // set up the player
        numPlayers++;
        playerName = "Player " + numPlayers;
        //playerID = playerCount;

        // set up the pieces
        playerPieces = new Character[NumPieces];
        SetupPieces(GameData.PickColor());

        // test
        //activePiece = playerPieces[0];
        //GameData.SetActivePiece(activePiece);
    }

    public void Update()
    {
        // follow the current piece if active
        if (GameData.GetGameMode() == GameData.Mode.MovingPiece && activePiece == GameData.GetActivePiece())
        {
            Vector3 piecePos = activePiece.transform.position;
            piecePos.y = 0;
            Camera.main.transform.eulerAngles = GameData.GetMainCameraRotation();
            Camera.main.transform.position = piecePos + GameData.GetCameraOffset();
        }

        // focus on finish tile if someone wins
        if (GameData.GetGameMode() == GameData.Mode.Winner)
        {
            Tile currTile = activePiece.GetCurrTile();
            Vector3 tilePos = new Vector3(currTile.GetPosition().x, 0, currTile.GetPosition().y);
            Camera.main.transform.eulerAngles = GameData.GetWinCameraRotation();
            Camera.main.transform.position = tilePos + GameData.GetWinCameraOffset();
        }
    }
}