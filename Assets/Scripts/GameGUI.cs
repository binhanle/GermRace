﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameGUI : MonoBehaviour
{
    public delegate void TestDelegate(); // This defines what type of method you're going to call.
    private static Canvas explanationScreen;
    private static Canvas rollScreen;
    private static Text titleText;
    private static Canvas messageScreen;
    private static Text messageText;
    private static Canvas winScreen;
    private static Text winText;
    private static Canvas selectMoveScreen;
    private static Text selectMoveText;
    private static Button nextTurnButton;
    private static Button defaultOKButton;
    private static Button specialOKButton;
    private static Canvas moveOrderScreen;
    private static Text moveOrderText;
    private static Canvas setupScreen;
    private static Text setupTitleText;
    private static InputField nameInputField;
    private static Dropdown colorDropdown;
    private static Canvas playerCountScreen;
    private static Canvas infoScreen;
    private static Text infoTitleText;
    private static Text infoText;
    private static Canvas mainScreen;
    private static Canvas optionsScreen;

    public static void ShowExplanationScreen(TextAsset textFile, UnityEngine.Events.UnityAction func)
    {
        UnityEngine.UI.Text explanationText = GameObject.Find("Explanation Text").GetComponent<Text>();
        UnityEngine.RectTransform rt = GameObject.Find("Explanation Text").GetComponent<RectTransform>();

        string[] lines = textFile.ToString().Split('\n');

        // The first line is the title
        explanationText.text = "\n<b>" + lines[0] + "</b>\n\n";

        //Filling the rest of the info
        for (int i = 1; i < lines.Length; i++)
        {
            explanationText.text += lines[i] + "\n";
        }

        //Change size of text box to hold the text
        Debug.Log(rt.rect.width);

        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, explanationText.preferredHeight);

        explanationScreen.enabled = true;

        explanationScreen.GetComponentInChildren<Button>().onClick.AddListener(func);
    }

    public static void ShowPieceColor(string pieceColor)
    {
        // shows demo piece of specific color and hides the rest
        Dictionary<string, GameObject> demoPieces = GameData.GetDemoPieces();
        foreach (string color in demoPieces.Keys)
        {
            if (color.Equals(pieceColor))
            {
                demoPieces[color].SetActive(true);
            }
            else
            {
                demoPieces[color].SetActive(false);
            }
        }
    }

    public static void HideExplanationShowSetup()
    {
        //Hides Explanation Screen
        explanationScreen.enabled = false;
        explanationScreen.GetComponentInChildren<Button>().onClick.RemoveListener(HideExplanationShowSetup);

        //Changes scene to the piece set up screen for player 1
        GameGUI.ShowSetupScreen("Player1");
        GameGUI.ShowCurrentPieceColor();
    }

    public static void HideExplanationScreenShowMain()
    {
        explanationScreen.enabled = false;
        explanationScreen.GetComponentInChildren<Button>().onClick.RemoveListener(HideExplanationScreenShowMain);
        mainScreen.enabled = true;
    }

    public static void ShowCurrentPieceColor()
    {
        // shows demo piece of color specified by dropdown
        ShowPieceColor(GameGUI.GetDropDownColor());
    }

    public static void ShowRollScreen()
    {
        //allows use of roll button
        UnityEngine.UI.Button rollButton = GameObject.Find("Roll Button").GetComponent<UnityEngine.UI.Button>();
        rollButton.interactable = true;

        // Displays the roll screen
        rollScreen.enabled = true;

        // Show who's rolling
        Player currPlayer = GameData.GetCurrPlayer();
        titleText.text = currPlayer.GetName() + " roll";
    }

    public static void HideRollScreen()
    {
        // Hides the roll screen
        rollScreen.enabled = false;
    }

    public void ShowRules()
    {
        // shows the rules
        HideMainScreen();
        ShowExplanationScreen(GameData.GetTextAssetHolder().GetRulesText(), HideExplanationScreenShowMain);
    }

    public void ShowCredits()
    {
        HideMainScreen();
        ShowExplanationScreen(GameData.GetTextAssetHolder().GetCreditsText(), HideExplanationScreenShowMain);
    }

    public static void ShowMessageScreen(Tile currTile)
    {
        // Displays the message screen
        messageScreen.enabled = true;

        // Show the text on the tile
        messageText.text = currTile.GetText();

        // If tile is special, use special button
        if (currTile.GetTileType() == "special")
        {
            defaultOKButton.gameObject.SetActive(false);
            specialOKButton.gameObject.SetActive(true);
        }
        else
        {
            defaultOKButton.gameObject.SetActive(true);
            specialOKButton.gameObject.SetActive(false);
        }
    }

    public static void PreviewMessageScreen(Tile possTile)
    {
        // Displays the message screen
        messageScreen.enabled = true;

        // Show the text on the tile
        messageText.text = possTile.GetText();

        //hide buttons
        defaultOKButton.gameObject.SetActive(false);
        specialOKButton.gameObject.SetActive(false);

        //highlight destination tile

    }

    public static void ShowCollisionScreen(Character smallerPiece, Player otherPlayer)
    {
        // Displays the message screen
        messageScreen.enabled = true;

        //create collision text
        string collisionText = "There was a collision with " + otherPlayer.GetName() + "\'s germ. \n\n";
        
        if (smallerPiece.Equals(GameData.GetActivePiece()))
        {
            collisionText = collisionText + "Your germ is smaller so it returns to start.";
        }
        else
        {
            collisionText = collisionText + "Your germ is larger so  " + otherPlayer.GetName() + "\'s germ returns to start.";
        }

        // Show the text on the tile
        messageText.text = collisionText;
        
        //show the proper ok button
        defaultOKButton.gameObject.SetActive(true);
        specialOKButton.gameObject.SetActive(false);

        //resolve the collision HEREHERE need to change to implement size logic
        Board board = GameData.GetBoard();
        Tile startTile = board.GetStartTile();
        smallerPiece.JumpToTile(startTile);
    }

    public static void ShowMergeScreen(Tile currTile)
    {
        // Displays the message screen
        messageScreen.enabled = true;

        // Show the text on the tile
        messageText.text = "Your germs came together and merged into a larger germ!";

        // If tile is special, use special button
        if (currTile.GetTileType() == "special")
        {
            defaultOKButton.gameObject.SetActive(false);
            specialOKButton.gameObject.SetActive(true);
        }
        else
        {
            defaultOKButton.gameObject.SetActive(true);
            specialOKButton.gameObject.SetActive(false);
        }
    }

    public static void HideMessageScreen()
    {
        // Hides the message screen
        messageScreen.enabled = false;
    }

    public static void ShowWinScreen()
    {
        // Displays the win screen
        winScreen.enabled = true;

        // Show who won
        winText.text = GameData.GetCurrPlayer().GetName() + " wins!";
    }

    public static void HideWinScreen()
    {
        // Hides the win screen
        winScreen.enabled = false;
    }

    public static void ShowSelectMoveScreen(bool hasLegalMoves)
    {
        // Shows the select move screen
        // Check if there are legal moves
        if (hasLegalMoves)
        {
            selectMoveText.text = "Please select a move";
            nextTurnButton.gameObject.SetActive(false);
        }
        else
        {
            selectMoveText.text = "You are stuck!";
            nextTurnButton.gameObject.SetActive(true);
        }

        // Show screen
        selectMoveScreen.enabled = true;
    }

    public static void HideSelectMoveScreen()
    {
        // Hides the select move screen
        selectMoveScreen.enabled = false;
    }

    public static void ShowMoveOrderScreen()
    {
        // Shows the move order screen
        moveOrderScreen.enabled = true;

        // show the move order
        moveOrderText.text = GameData.GetBoard().GetMoveOrderString();
    }

    public static void HideMoveOrderScreen()
    {
        // Hides the move order screen
        moveOrderScreen.enabled = false;
    }

    public static void ShowSetupScreen(string defaultName)
    {
        // Shows the player setup screen
        setupScreen.enabled = true;

        // display the default name
        setupTitleText.text = defaultName + " setup";
        nameInputField.text = defaultName;
        nameInputField.Select();

        // populate the dropdown menu
        colorDropdown.ClearOptions();
        colorDropdown.AddOptions(GameData.GetAvailableColors());
        colorDropdown.value = 0;
    }

    public static void HideSetupScreen()
    {
        // Hides the player setup screen
        setupScreen.enabled = false;
    }

    public static string GetInputFieldName()
    {
        // Returns the text inside the name input field
        return nameInputField.text;
    }

    public static string GetDropDownColor()
    {
        // Returns the color selected by the dropdown
        return colorDropdown.options[colorDropdown.value].text;
    }

    public static void ShowPlayerCountScreen()
    {
        // Displays the player count screen
        playerCountScreen.enabled = true;
    }

    public static void HidePlayerCountScreen()
    {
        // Hides the player count screen
        playerCountScreen.enabled = false;
    }

    public static void ShowInfoScreen(TextAsset textFileAsset)
    {
        // Shows the info screen
        // Open the text file
        string[] lines = textFileAsset.ToString().Split('\n');

        // The first line is the title
        infoTitleText.text = lines[0];

        // The rest go in the info text box
        infoText.text = "";
        for (int i = 1; i < lines.Length; i++)
        {
            infoText.text += lines[i] + "\n";
        }

        // Show the info screen
        infoScreen.enabled = true;
    }

    public static void HideInfoScreen()
    {
        // Hides the info screen
        infoScreen.enabled = false;
    }

    public static void ShowMainScreen()
    {
        // Shows the main menu
        mainScreen.enabled = true;
    }

    public static void HideMainScreen()
    {
        // Hides the main menu
        mainScreen.enabled = false;
    }

    public static void HideEveryScreen()
    {
        // Hides every screen
        GameObject guiObject = GameObject.Find("GUI");
        foreach (Canvas screen in guiObject.GetComponentsInChildren<Canvas>())
        {
            if (screen != guiObject.GetComponent<Canvas>())
            {
                screen.enabled = false;
            }
        }
    }

    public static void ShowOptionsScreen()
    {
        // Shows the options screen
        optionsScreen.enabled = true;
    }

    public static void HideOptionsScreen()
    {
        // Hides the options screen
        optionsScreen.enabled = false;
    }

    // Use this for initialization
    void Awake()
    {
        explanationScreen = GameObject.Find("Explanation Screen").GetComponent<Canvas>();
        rollScreen = GameObject.Find("Roll Screen").GetComponent<Canvas>();
        titleText = GameObject.Find("Title Text").GetComponent<Text>();
        messageScreen = GameObject.Find("Message Screen").GetComponent<Canvas>();
        messageText = GameObject.Find("Message Text").GetComponent<Text>();
        winScreen = GameObject.Find("Win Screen").GetComponent<Canvas>();
        winText = GameObject.Find("Win Text").GetComponent<Text>();
        selectMoveScreen = GameObject.Find("Select Move Screen").GetComponent<Canvas>();
        selectMoveText = GameObject.Find("Select Move Text").GetComponent<Text>();
        nextTurnButton = GameObject.Find("Next Turn Button").GetComponent<Button>();
        defaultOKButton = GameObject.Find("Default OK Button").GetComponent<Button>();
        specialOKButton = GameObject.Find("Special OK Button").GetComponent<Button>();
        moveOrderScreen = GameObject.Find("Move Order Screen").GetComponent<Canvas>();
        moveOrderText = GameObject.Find("Move Order Text").GetComponent<Text>();
        setupScreen = GameObject.Find("Setup Screen").GetComponent<Canvas>();
        setupTitleText = GameObject.Find("Setup Title Text").GetComponent<Text>();
        nameInputField = GameObject.Find("Name Input Field").GetComponent<InputField>();
        colorDropdown = GameObject.Find("Color Dropdown").GetComponent<Dropdown>();
        playerCountScreen = GameObject.Find("Player Count Screen").GetComponent<Canvas>();
        infoScreen = GameObject.Find("Info Screen").GetComponent<Canvas>();
        infoTitleText = GameObject.Find("Info Title Text").GetComponent<Text>();
        infoText = GameObject.Find("Info Text").GetComponent<Text>();
        mainScreen = GameObject.Find("Main Screen").GetComponent<Canvas>();
        optionsScreen = GameObject.Find("Options Screen").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
