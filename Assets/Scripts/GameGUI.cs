using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour
{
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

    public static void ShowRollScreen()
    {
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

    // Use this for initialization
    void Awake()
    {
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
