using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static string startTileName = "start";
    private static string tilesPath = "Assets/Scripts/Tiles.xml";
    private static string charDir = "Characters/Mushroomboypack1.2/3D/";
    //private static string menuDir = "Prefabs/Menus/";
    private static string linePath = "VolumetricLines/Prefabs/Line";
    private static Vector3 cameraOffset = new Vector3(2, 1, -5);
    private static Vector3 mainCameraRotation = new Vector3(10, -20, 0);
    private static Vector3 dieCameraPosition = new Vector3(-10, 10, 0);
    private static Vector3 dieCameraRotation = new Vector3(30, -90, 0);
    private static Player currPlayer;
    private static Character activePiece;
    //private static Camera mainCamera;
    //private static Camera dieCamera;
    private static Vector3 winCameraOffset = new Vector3(2, 0.5f, -1.25f);
    private static Vector3 winCameraRotation = new Vector3(10, -60, 0);
    private static Mode gameMode;
    public enum Mode { Home, InitialRoll, NormalRoll, RollSixOrDie, SelectMove, MovingPiece, Winner };
    private static int numPlayers = 4;
    private static int numPiecesPerPlayer = 2;
    private static string[] pieceColors = { "red", "yellow", "green", "blue" };
    private static List<string> availableColors;
    private static Vector3 topViewOffset = new Vector3(11, 15, -3);
    private static Vector3 topViewRotation = new Vector3(90, 0, 0);
    private static Dictionary<string, string> colorScheme = new Dictionary<string, string>()
    {
        { "start", "orange" },
        { "normal", "blue" },
        { "jumpAhead", "green" },
        { "jumpBack", "red" },
        { "jumpOther", "purple" },
        { "special", "grey" },
        { "finish", "yellow" }
    };
    private static Board board;
    private static Vector3 homeCameraPosition = new Vector3(0.35f, 0.25f, -19);
    private static Vector3 homeCameraRotation = new Vector3(5, 180, 0);
    private static Dictionary<string, GameObject> demoPieces;

    static GameData()
    {
        // initialize list of available piece colors
        availableColors = new List<string>(pieceColors);
    }

    public static string GetStartTileName()
    {
        // Get the start tile name
        return startTileName;
    }

    public static string GetTilesPath()
    {
        // Get the path of the tile xml
        return tilesPath;
    }

    public static string GetCharDir()
    {
        // Get the character directory
        return charDir;
    }

    /*public static string GetMenuDir()
    {
        // Get the menu directory
        return menuDir;
    }*/

    public static string GetLinePath()
    {
        // Get the path of the line prefab
        return linePath;
    }

    public static Vector3 GetCameraOffset()
    {
        // Get the distance from camera to active piece
        return cameraOffset;
    }

    public static Vector3 GetMainCameraRotation()
    {
        // Get the rotation of the camera when tracking active piece
        return mainCameraRotation;
    }

    public static Vector3 GetDieCameraPosition()
    {
        // Get the position of the camera for die rolling
        return dieCameraPosition;
    }

    public static Vector3 GetDieCameraRotation()
    {
        // Get the rotation of the camera for die rolling
        return dieCameraRotation;
    }

    public static Player GetCurrPlayer()
    {
        // Gets the current player
        return currPlayer;
    }

    public static void SetCurrPlayer(Player player)
    {
        // Sets the current player
        currPlayer = player;
    }

    public static Character GetActivePiece()
    {
        // Get the active piece
        return activePiece;
    }

    public static void SetActivePiece(Character piece)
    {
        // Get the current player id
        activePiece = piece;
    }

    /*public static Camera GetMainCamera()
    {
        // Get the main camera
        return mainCamera;
    }

    public static void SetMainCamera(Camera camera)
    {
        // Set the main camera
        mainCamera = camera;
    }

    public static Camera GetDieCamera()
    {
        // Get the die camera
        return mainCamera;
    }

    public static void SetDieCamera(Camera camera)
    {
        // Set the die camera
        dieCamera = camera;
    }*/

    public static Vector3 GetWinCameraOffset()
    {
        // Get the offset of the win camera
        return winCameraOffset;
    }

    public static Vector3 GetWinCameraRotation()
    {
        // Get the rotation of the win camera
        return winCameraRotation;
    }

    public static Mode GetGameMode()
    {
        // Get the game mode
        return gameMode;
    }

    public static void SetGameMode(Mode mode)
    {
        // Set the game mode
        gameMode = mode;
    }

    public static int GetNumPlayers()
    {
        // Get the number of players
        return numPlayers;
    }

    public static int GetNumPiecesPerPlayer()
    {
        // Get the number of pieces per player
        return numPiecesPerPlayer;
    }

    /*public static string PickColor()
    {
        // Pick a piece color
        return availableColors.Dequeue();
    }*/

    public static Vector3 GetTopViewOffset()
    {
        // Get the offset of the top view
        return topViewOffset;
    }

    public static Vector3 GetTopViewRotation()
    {
        // Get the rotation of the top view
        return topViewRotation;
    }

    public static Dictionary<string, string> GetColorScheme()
    {
        // Get the color scheme for tiles
        return colorScheme;
    }

    public static Board GetBoard()
    {
        // Get the board
        return board;
    }

    public static void SetBoard(Board gameBoard)
    {
        // Set the board
        board = gameBoard;
    }

    public static List<string> GetAvailableColors()
    {
        // Get available piece colors
        return availableColors;
    }

    public static void RemoveAvailableColor(string color)
    {
        // Removes a color from available piece colors
        availableColors.Remove(color);
    }

    public static Vector3 GetHomeCameraPosition()
    {
        // Get the offset of the home camera
        return homeCameraPosition;
    }

    public static Vector3 GetHomeCameraRotation()
    {
        // Get the rotation of the home camera
        return homeCameraRotation;
    }

    public static void SetDemoPieces(Dictionary<string, GameObject> pieces)
    {
        // Set the dictionary of demo pieces
        demoPieces = pieces;
    }

    public static Dictionary<string, GameObject> GetDemoPieces()
    {
        // Get the dictionary of demo pieces
        return demoPieces;
    }

    public static string[] GetPieceColors()
    {
        // Get the list of piece colors
        return pieceColors;
    }
}
