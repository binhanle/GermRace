using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static string startTileName = "start";
    private static string tilesPath = "Assets/Scripts/Tiles.xml";
    private static string charDir = "Characters/Mushroomboypack1.2/3D/";
    private static Vector3 cameraOffset = new Vector3(2, 1, -5);
    private static Vector3 mainCameraRotation = new Vector3(10, -20, 0);
    private static Vector3 dieCameraPosition = new Vector3(0, 5, 0);
    private static Vector3 dieCameraRotation = new Vector3(10, -90, 0);
    private static int currPlayerId = 1;
    private static Character activePiece;
    //private static Camera mainCamera;
    //private static Camera dieCamera;
    private static Mode gameMode;
    public enum Mode { InitialRoll, NormalRoll, MovingPiece };
    private static int numPlayers = 1;
    private static int numPiecesPerPlayer = 1;

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

    public static int GetCurrPlayerId()
    {
        // Get the current player id
        return currPlayerId;
    }

    public static void SetCurrPlayerId(int id)
    {
        // Get the current player id
        currPlayerId = id;
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
}
