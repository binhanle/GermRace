using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using UnityEngine.UI;

public static class GameData
{
    //Merge/Collision settings
    private static bool mergeCollide = true;

    //TextAssetHolder
    private static TextAssetHolder textAssets = GameObject.Find("TextAssets").GetComponent<TextAssetHolder>();

    //PreFabAssetHolder
    private static PrefabAssetHolder prefabAssets = GameObject.Find("PrefabAssets").GetComponent<PrefabAssetHolder>();

    //Tile XML Data
    private static string startTileName = "start";
    private static string tilesPath = "Assets/Scripts/Tiles.xml";

    //Character data
    private static Dictionary<string, GameObject> charDir = new Dictionary<string, GameObject>()
    {
        { "red",  prefabAssets.GetRedCharacter()},
        { "green",  prefabAssets.GetGreenCharacter()},
        { "blue",  prefabAssets.GetBlueCharacter()},
        { "yellow",  prefabAssets.GetYellowCharacter()}
    };

    //Tile Animation Parameters 
    private static float boardHeight = 0f;
    private static float tileSize = .95f;
    private static float animTime = 2f;
    private static float animHeight = 5f;

    //private static string menuDir = "Prefabs/Menus/";

    //camera settings
    private static Vector3 cameraOffset = new Vector3(2, 1, -5);
    private static Vector3 mainCameraRotation = new Vector3(10, -20, 0);
    private static Vector3 dieCameraPosition = new Vector3(-10, 10, 0);
    private static Vector3 dieCameraRotation = new Vector3(30, -90, 0);
    private static Vector3 winCameraOffset = new Vector3(2, 0.5f, -1.25f);
    private static Vector3 winCameraRotation = new Vector3(10, -60, 0);
    private static Vector3 homeCameraPosition = new Vector3(0.35f, 0.35f, -19);
    private static Vector3 homeCameraRotation = new Vector3(5, 180, 0);
    private static Vector3 topViewOffset = new Vector3(11, 15, -3);
    private static Vector3 topViewRotation = new Vector3(90, 0, 0);

    //active user input
    private static Player currPlayer;
    private static Character activePiece;
    //private static Camera mainCamera;
    //private static Camera dieCamera;

    //game mode settings
    private static Mode gameMode;
    public enum Mode { Home, InitialRoll, NormalRoll, RollSixOrDie, SelectMove, MovingPiece, Winner };

    //setup data
    private static int numPlayers;
    private static int numPiecesPerPlayer = 2;
    private static string[] pieceColors = { "red", "yellow", "green", "blue" };
    private static Dictionary<string, string> pieceColorMap = new Dictionary<string, string>(){ { "red", "red" }, { "yellow", "yellow" }, { "green", "green" }, { "blue", "blue" } };
    private static string[] ordinals = { "first", "second", "third", "fourth" };
    private static List<string> availableColors;

    //language data
    private static Dictionary<string, Language> languages = new Dictionary<string, Language>();
    private static string currentLanguage = "English";
    private static XmlDocument LangXMLDoc;
    private static XDocument LangXDoc;
    private static IEnumerable<XElement> LangItems;
    public static string langKeyOrdinal = "OrderWords";
    public static string langKeyColor = "Colors";


    //tile colors
    private static Dictionary<string, string> colorScheme = new Dictionary<string, string>()
    {
        { "start", "orange" },
        { "normal", "blue" },
        { "jumpAhead", "green" },
        { "jumpBack", "#f60" },
        { "jumpOther", "purple" },
        { "special", "grey" },
        { "finish", "yellow" }
    };

    //game piece and board
    private static Board board;
    private static Dictionary<string, GameObject> demoPieces;

    //visual references
    private static string materialsDir = "Images/Materials/";

    static GameData()
    {
        // initialize list of available piece colors
        availableColors = new List<string>(pieceColors);
    }

    public static void ResetAvailableColors()
    {
        //resets available colors
        availableColors = new List<string>(languages[currentLanguage].GetColors());
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

    public static Dictionary<string, GameObject> GetCharDir()
    {
        // Get the character directory
        return charDir;
    }

    /*public static string GetMenuDir()
    {
        // Get the menu directory
        return menuDir;
    }*/

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

    public static void SetNumPlayers(int num)
    {
        // Set the number of players
        numPlayers = num;
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

    public static Dictionary<string, GameObject> GetDemoPieces()
    {
        // Get the dictionary of demo pieces
        return demoPieces;
    }

    public static void SetDemoPieces(Dictionary<string, GameObject> pieces)
    {
        // Set the dictionary of demo pieces
        demoPieces = pieces;
    }

    public static string[] GetPieceColors()
    {
        // Get the list of piece colors
        return pieceColors;
    }

    public static void ChangePieceColors(string[] newColors)
    {
        for(int i = 0; i < newColors.Length; i++)
        {
            pieceColors[i] = newColors[i];
        }
    }

    public static string GetMaterialsDir()
    {
        // Get the directory of the materials
        return materialsDir;
    }

    public static float[] GetAnimationParams()
    {
        //returns default parameters for visual animations
        return new float[] { boardHeight, tileSize, animTime, animHeight }; 
    }

    //DOCUMENT
    public static string[] GetOrdinals()
    {
        //access method for ordinals
        return ordinals;
    }

    //DOCUMENT
    public static void ChangeOrdinals(string[] newOrdinals)
    {
        ordinals = newOrdinals;
    }
    //DOCUMENT
    public static void ChangeColorMap(string[] newColorLabels)
    {
        pieceColorMap = new Dictionary<string, string>() { { newColorLabels[0], "red" }, { newColorLabels[1], "yellow" }, { newColorLabels[2], "green" }, { newColorLabels[3], "blue" } };
    }

    //DOCUMENT
    public static Dictionary<string, string> GetColorMap()
    {
        return pieceColorMap;
    }

    //Document
    public static string GetLanguageText(string key)
    {
        // Find the matching language and key in the saved language xml data 
        
        foreach(var item in LangItems)
        {
            if (item.Element("Name").Value.Trim() == currentLanguage)
            {
                //Debug.Log("reached");
                return item.Element(key).Value.Trim();
            }
        }
        return "KEY ERROR: NO VALID STRING";
        /**
        Language currLang = languages[currentLanguage];
        if (currLang.GetLanguageTextMap().ContainsKey(key))
        {
            return currLang.GetLanguageTextMap()[key];
        }
        
        **/
    }



    //DOCUMENT
    public static void LoadLanguageGameObjectText()
    {
        // Load the Language XML file into Tile classes and puts them in the tile map
        XmlDocument xmlDoc = new XmlDocument();
        Debug.Log(GetTextAssetHolder().GetMenuText().ToString());
        xmlDoc.LoadXml(GetTextAssetHolder().GetMenuText().ToString());
        XDocument xDoc = XDocument.Parse(xmlDoc.OuterXml);
        IEnumerable<XElement> items = xDoc.Descendants("languages").Elements();
        //Debug.Log("languages descendents" + items);
        foreach (var item in items)
        {
            Language language = new Language();

            //set the name
            string languageLabel = item.Element("Name").Value.Trim();
            language.SetName(languageLabel);
            language.SetText("Name", languageLabel);
            string rollButton = item.Element("RollButton").Value.Trim();
            language.SetRollButton(rollButton);
            language.SetText("RollButton", rollButton);
            string rollTitle = item.Element("RollTitle").Value.Trim();
            language.SetRollTitle(rollTitle);
            language.SetText("RollTitle", rollTitle);
            string okButton = item.Element("OKButton").Value.Trim();
            language.SetOkButton(okButton);
            language.SetText("OKButton", okButton);
            string winMenu = item.Element("WinMenu").Value.Trim();
            language.SetWinMenu(winMenu);
            language.SetText("WinMenu", winMenu);
            string winTitle = item.Element("WinTitle").Value.Trim();
            language.SetWinTitle(winTitle);
            language.SetText("WinTitle", winTitle);
            string selectMoveTitle = item.Element("SelectMoveTitle").Value.Trim();
            language.SetSelectMoveTitle(selectMoveTitle);
            language.SetText("SelectMoveTitle", selectMoveTitle);
            string moveOrderTitle = item.Element("MoveOrderTitle").Value.Trim();
            language.SetMoveOrderTitle(moveOrderTitle);
            language.SetText("MoveOrderTitle", moveOrderTitle);
            string moveText = item.Element("MoveText").Value.Trim();
            language.SetMoveText(moveText);
            language.SetText("MoveText", moveText);
            string setupTitle = item.Element("SetupTitle").Value.Trim();
            language.SetSetupTitle(setupTitle);
            language.SetText("setupTitle", setupTitle);
            string setupName = item.Element("SetupName").Value.Trim();
            language.SetSetupName(setupName);
            language.SetText("SetupName", setupName);
            string setupColor = item.Element("SetupColor").Value.Trim();
            language.SetSetupColor(setupColor);
            language.SetText("SetupColor", setupColor);
            string orderWords = item.Element("OrderWords").Value.Trim();
            language.SetOrderWords(orderWords);
            language.SetText("OrderWords", orderWords);
            string colors = item.Element("Colors").Value.Trim();
            language.SetColors(colors);
            language.SetText("Colors", colors);
            string playerCountTitle = item.Element("PlayerCountTitle").Value.Trim();
            language.SetPlayerCountTitle(playerCountTitle);
            language.SetText("PlayerCountTitle", playerCountTitle);
            string mainTitle = item.Element("MainTitle").Value.Trim();
            language.SetMainTitle(mainTitle);
            language.SetText("MainTitle", mainTitle);
            string mainPlayButton = item.Element("MainPlayButton").Value.Trim();
            language.SetMainPlayButton(mainPlayButton);
            language.SetText("MainPlayButton", mainPlayButton);
            string mainRulesButton = item.Element("MainRulesButton").Value.Trim();
            language.SetMainRulesButton(mainRulesButton);
            language.SetText("MainRulesButton", mainRulesButton);
            string mainOptionsButton = item.Element("MainOptionsButton").Value.Trim();
            language.SetMainOptionsButton(mainOptionsButton);
            language.SetText("MainOptionsButton", mainOptionsButton);
            string mainCreditsButton= item.Element("MainCreditsButton").Value.Trim();
            language.SetMainCreditsButton(mainCreditsButton);
            language.SetText("MainCreditsButton", mainCreditsButton);
            string optionsMusicLabel = item.Element("OptionsMusicLabel").Value.Trim();
            language.SetOptionsMusicLabel(optionsMusicLabel);
            //language.SetText()
            string optionsLanguageLabel = item.Element("OptionsLanguageLabel").Value.Trim();
            language.SetOptionsLanguageLabel(optionsLanguageLabel);

            if (language != null)
            {
                languages.Add(languageLabel, language);
            }
            else
            {
                Debug.Log("Error " + languages.Keys.ToString());
            }
        }

    }

    //Document 
    public static Language GetCurrentLanguage()
    {
        return languages[currentLanguage];
    }

    //Document 
    public static string GetCurrentLanguageKey()
    {
        return currentLanguage;
    }

    //Document
    public static void SetCurrentLanguage(string input)
    {
        currentLanguage = input;
    }

    //Document
    public static List<string> GetLanguages()
    {
        List<string> availableLanguages = new List<string>();

        availableLanguages.Add(currentLanguage);

        foreach (string languageLabel in languages.Keys)
        {
            if (languageLabel != currentLanguage)
            {
                availableLanguages.Add(languageLabel);
            }
        }

        return availableLanguages;
    }

    public static TextAssetHolder GetTextAssetHolder()
    {
        //returns the textAssetHolder 
        return textAssets;
    }

    public static PrefabAssetHolder GetPrefabAssetHolder()
    {
        //access method for the prefabAssets
        return prefabAssets;
    }

    //Document
    public static void InitilializeLanguages()
    {
        //Load Language Data
        LangXMLDoc = new XmlDocument();
        LangXMLDoc.LoadXml(GetTextAssetHolder().GetMenuText().ToString());
        LangXDoc = XDocument.Parse(LangXMLDoc.OuterXml);
        LangItems = LangXDoc.Descendants("languages").Elements();
        Debug.Log("instantiated");
    }

    //document 
    public static void SetMergeCollide(bool val)
    {
        //sets value of mergecollide boolean to input
        mergeCollide = val;
    }

    //document
    public static bool GetMergeCollide()
    {
        //returns value of mergeCollide boolean
        return mergeCollide;
    }
}
