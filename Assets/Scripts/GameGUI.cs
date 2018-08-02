using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class GameGUI : MonoBehaviour
{
    public delegate void TestDelegate(); // This defines what type of method you're going to call.
    private static Canvas explanationScreen;
    private static Canvas rollScreen;
    private static Canvas messageScreen;
    private static Text messageText;
    private static Canvas winScreen;
    private static Text winTitleText;
    private static Canvas selectMoveScreen;
    private static Button nextTurnButton;
    private static Button defaultOKButton;
    private static Button specialOKButton;
    private static Canvas moveOrderScreen;
    private static Text moveOrderText;
    private static Canvas setupScreen;
    private static Text setupTitleText;
    private static InputField nameInputField;
    private static Dropdown colorDropdown;
    private static Dropdown languageDropdown;
    private static Canvas playerCountScreen;
    private static Canvas infoScreen;
    private static Text infoTitleText;
    private static Text infoText;
    private static Canvas mainScreen;
    private static Canvas optionsScreen;
    private static Button doMoveButton;
    private static Button noMoveButton;
    private static Image messageCharacterSprite;
    private static SpriteHolder spriteSource;

    //GameObjects' Text
    private static Text rollButtonText;
    private static Text rollTitleText;
    private static Text winMenuText;
    private static Text selectMoveTitleText;
    private static Text moveOrderTitleText;
    private static Text playerCountTitleText;
    private static Text mainTitleText;
    private static Text mainPlayButtonText;
    private static Text mainRuleButtonText;
    private static Text mainOptionsButtonText;
    private static Text mainCreditsButtonText;
    private static Text nextTurnOKButtonText;
    private static Text moveOrderOKButtonText;
    private static Text optionsTitleText;
    private static Text optionsOKButtonText;
    private static Text optionsMusicLabel;
    private static Text optionsLanguageLabel;
    private static Text explanationOKButtonText;
    private static Text setupNameText;
    private static Text setupColorText;
    private static Text explanationText;
    private static Text setupOKButton;
    private static Text messageDefaultOKButton;
    private static Text messageSpecialOKButton;
    private static Text optionsMergeCollideLabel;
    private static List<Text> GUITextComponents;

    //analytics
    public static GameStarted analyticsGameStarted;
    public static ApplicationExited analyticsApplicationExited;
    public static GameCompleted analyticsGameCompleted;
    public static InitialPathChosen analyticsInitialPathChosen;

    //Characer Sprites
    public static Dictionary<string, Sprite> characterSprites = new Dictionary<string, Sprite>();
    
   
    public static void ShowExplanationScreen(string key, UnityEngine.Events.UnityAction func)
    {
        //set text
        string text = GameData.GetLanguageText(key);
        text = text.Replace("\\n", "\n");
        explanationText.text = text;

        //resize rectangle 
        RectTransform rt = explanationText.GetComponent<RectTransform>();

        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, explanationText.preferredHeight);

        //show screen
        explanationScreen.enabled = true;

        //set button transition function 
        explanationScreen.GetComponentInChildren<Button>().onClick.AddListener(func);


    }

    public static void ShowExplanationScreenV2(TextAsset textFile, UnityEngine.Events.UnityAction func)
    {
        RectTransform rt = explanationText.GetComponent<RectTransform>();

        string[] lines = textFile.ToString().Split('\n');

        // The first line is the title
        explanationText.text = "\n<b>" + lines[0] + "</b>\n\n";

        //Filling the rest of the info
        for (int i = 1; i < lines.Length; i++)
        {
            explanationText.text += lines[i] + "\n";
        }

        //Change size of text box to hold the text
        //Debug.Log(rt.rect.width);

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
        ShowPieceColor(GameData.GetColorMap()[GameGUI.GetDropDownColor()]);
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
        rollTitleText.text = currPlayer.GetName() + " " +GameData.GetCurrentLanguage().GetRollTitle();
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
        ShowExplanationScreen("RulesText", HideExplanationScreenShowMain);
    }

    public void ShowCredits()
    {
        HideMainScreen();
        ShowExplanationScreen("CreditsText", HideExplanationScreenShowMain);
    }

    public static void ShowMessageScreen(Tile currTile)
    {
        // Displays the message screen
        messageScreen.enabled = true;

        // Show the text on the tile
        messageText.text = currTile.GetText(GameData.GetCurrentLanguageKey());

        //hide move buttons
        doMoveButton.gameObject.SetActive(false);
        noMoveButton.gameObject.SetActive(false);

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

        //change character sprite to match player's pieces 
        //characterSprites = new Dictionary<string, Sprite>() { { "redCharacter", spriteSource.redImage }, { "blueCharacter", spriteSource.blueImage }, { "yellowCharacter", spriteSource.yellowImage }, { "greenCharacter", spriteSource.greenImage } };
        //messageCharacterSprite.sprite = characterSprites[GameData.GetCurrPlayer().GetCharType()];
    }

    public static void PreviewMessageScreen(Tile possTile)
    {
        // Displays the message screen
        messageScreen.enabled = true;

        // Show the text on the tile
        messageText.text = possTile.GetText(GameData.GetCurrentLanguageKey());

        //hide buttons
        defaultOKButton.gameObject.SetActive(false);
        specialOKButton.gameObject.SetActive(false);

        //show move buttons
        doMoveButton.gameObject.SetActive(true);
        noMoveButton.gameObject.SetActive(true);

        //show character sprite
        messageCharacterSprite.gameObject.SetActive(true);

        //change character sprite to match player's pieces 
        messageCharacterSprite.sprite = characterSprites[GameData.GetCurrPlayer().GetCharType()];
    }

    public static void ShowCollisionScreen(Character smallerPiece, Player otherPlayer)
    {
        // Displays the message screen
        messageScreen.enabled = true;

        //create collision text
        string collisionText = GameData.GetLanguageText("CollistionText");
        
        if (smallerPiece.Equals(GameData.GetActivePiece()))
        {
            collisionText = collisionText + GameData.GetLanguageText("CollisionReturnText");
        }
        else
        {
            collisionText = collisionText + GameData.GetLanguageText("CollisionStayText");
        }

        // Show the text on the tile
        messageText.text = collisionText;
        
        //show the proper ok button
        defaultOKButton.gameObject.SetActive(true);
        specialOKButton.gameObject.SetActive(false);

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
        winTitleText.text = GameData.GetCurrPlayer().GetName() + " " + GameData.GetCurrentLanguage().GetWinTitle();
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
            Debug.Log("Please select moves");
            selectMoveTitleText.GetComponent<LocalizedText>().updateLanguage();
            nextTurnButton.gameObject.SetActive(false);
        }
        else
        {
            selectMoveTitleText.text = "You are stuck!";
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
        setupTitleText.text = defaultName + " " + GameData.GetCurrentLanguage().GetSetupTitle();
        nameInputField.text = defaultName;
        nameInputField.Select();

        // populate the dropdown menu
        colorDropdown.ClearOptions();
        colorDropdown.AddOptions(GameData.GetAvailableColors());
        //Debug.Log("available colors " + GameData.GetAvailableColors()[0]);
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

        //populate the language dropdown
        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(GameData.GetLanguages());
        languageDropdown.value = 0;
    }

    public static void HideOptionsScreen()
    {
        // Hides the options screen
        optionsScreen.enabled = false;
    }

    public void AskHowManyPlayers()
    {
        // shows the player count screen
        GameGUI.HideMainScreen();
        GameGUI.ShowPlayerCountScreen();
    }


    public void ShowOptions()
    {
        // shows the options screen
        GameGUI.HideMainScreen();
        GameGUI.ShowOptionsScreen();
    }

    //document
    public static string GetDropDownLanguage()
    {
        // Returns the color selected by the dropdown
        return languageDropdown.options[languageDropdown.value].text;
    }

    //document
    public static Button GetMoveButton()
    {
        return doMoveButton;
    }

    //document 
    public static Button GetNoMoveButton()
    {
        return noMoveButton;
    }

    //Document
    public void SetMergeCollide(Toggle toggle)
    {
        // enables or disables the merges and collisions
        GameData.SetMergeCollide(toggle.isOn);
        Debug.Log(GameData.GetMergeCollide());
    }

    //DOCUMENT
    public static void UpdateLanguageOptions()
    {
        //update the language for gameObject
        GameData.SetCurrentLanguage(GetDropDownLanguage());
        Language currentLanguage = GameData.GetCurrentLanguage();

        //update the GameObject's Text's
        /**
        rollButtonText.text = currentLanguage.GetRollButton();
        rollTitleText.text = currentLanguage.GetRollTitle();
        winMenuText.text = currentLanguage.GetWinMenu();
        winTitleText.text = currentLanguage.GetWinTitle();
        selectMoveTitleText.text = currentLanguage.GetSelectMoveTitle();
        nextTurnOKButtonText.text = currentLanguage.GetOkButton();
        moveOrderTitleText.text = currentLanguage.GetMoveOrderTitle();
        moveOrderText.text = currentLanguage.GetMoveText();
        moveOrderOKButtonText.text = currentLanguage.GetOkButton();
        setupTitleText.text = currentLanguage.GetSetupTitle();
        playerCountTitleText.text = currentLanguage.GetPlayerCountTitle();
        mainTitleText.text = currentLanguage.GetMainTitle();
        mainPlayButtonText.text = currentLanguage.GetMainPlayButton();
        mainRuleButtonText.text = currentLanguage.GetMainRulesButton();
        mainOptionsButtonText.text = currentLanguage.GetMainOptionsButton();
        mainCreditsButtonText.text = currentLanguage.GetMainCreditsButton();
        optionsTitleText.text = currentLanguage.GetMainOptionsButton();
        optionsMusicLabel.text = currentLanguage.GetOptionsMusicLabel();
        optionsOKButtonText.text = currentLanguage.GetOkButton();
        optionsLanguageLabel.text = currentLanguage.GetOptionsLanguageLabel();
        explanationOKButtonText.text = currentLanguage.GetOkButton();
        setupNameText.text = currentLanguage.GetSetupName();
        setupColorText.text = currentLanguage.GetSetupColor();
        **/
        foreach (Text text in GUITextComponents)
        {
            try
            {
                //Debug.Log(text + " worked");
                text.GetComponent<LocalizedText>().updateLanguage();
            }
            catch(Exception e)
            {
                Debug.Log(text + ", " + text.text);
            }
        }

        /**
        GameData.ChangeOrdinals(currentLanguage.GetOrderWords());
        GameData.ChangeColorMap(currentLanguage.GetColors());
        GameData.ChangePieceColors(currentLanguage.GetColors());
        **/
        GameData.ChangeOrdinals(GameData.GetLanguageText(GameData.langKeyOrdinal).Split(','));
        GameData.ChangeColorMap(GameData.GetLanguageText(GameData.langKeyColor).Split(','));
        GameData.ChangePieceColors(GameData.GetLanguageText(GameData.langKeyColor).Split(','));
        GameData.ResetAvailableColors();
        
    }

    //document
    void Start()
    {
        explanationScreen = GameObject.Find("Explanation Screen").GetComponent<Canvas>();
        rollScreen = GameObject.Find("Roll Screen").GetComponent<Canvas>();
        messageScreen = GameObject.Find("Message Screen").GetComponent<Canvas>();
        winScreen = GameObject.Find("Win Screen").GetComponent<Canvas>();
        selectMoveScreen = GameObject.Find("Select Move Screen").GetComponent<Canvas>();
        nextTurnButton = GameObject.Find("Next Turn Button").GetComponent<Button>();
        defaultOKButton = GameObject.Find("Default OK Button").GetComponent<Button>();
        specialOKButton = GameObject.Find("Special OK Button").GetComponent<Button>();
        moveOrderScreen = GameObject.Find("Move Order Screen").GetComponent<Canvas>();
        setupScreen = GameObject.Find("Setup Screen").GetComponent<Canvas>();
        nameInputField = GameObject.Find("Name Input Field").GetComponent<InputField>();
        colorDropdown = GameObject.Find("Color Dropdown").GetComponent<Dropdown>();
        languageDropdown = GameObject.Find("Language Dropdown").GetComponent<Dropdown>();
        playerCountScreen = GameObject.Find("Player Count Screen").GetComponent<Canvas>();
        infoScreen = GameObject.Find("Info Screen").GetComponent<Canvas>();
        doMoveButton = GameObject.Find("Move Button").GetComponent<Button>();
        noMoveButton = GameObject.Find("No Move Button").GetComponent<Button>();
        //infoTitleText = GameObject.Find("Info Title Text").GetComponent<Text>();
        //infoText = GameObject.Find("Info Text").GetComponent<Text>();
        mainScreen = GameObject.Find("Main Screen").GetComponent<Canvas>();
        optionsScreen = GameObject.Find("Options Screen").GetComponent<Canvas>();
        GameData.LoadLanguageGameObjectText();
        GameData.InitilializeLanguages();
        messageText = GameObject.Find("Message Text").GetComponent<Text>();
        rollButtonText = GameObject.Find("Roll Button Text").GetComponent<Text>();
        rollTitleText = GameObject.Find("Roll Title Text").GetComponent<Text>();
        winMenuText = GameObject.Find("Win to Menu Text").GetComponent<Text>();
        winTitleText = GameObject.Find("Win Text").GetComponent<Text>();
        selectMoveTitleText = GameObject.Find("Select Move Title Text").GetComponent<Text>(); ;
        nextTurnOKButtonText = GameObject.Find("Next Turn Button Text").GetComponent<Text>(); ;
        moveOrderTitleText = GameObject.Find("Move Order Title Text").GetComponent<Text>(); ;
        moveOrderText = GameObject.Find("Move Order Text").GetComponent<Text>();
        moveOrderOKButtonText = GameObject.Find("Move Order OK Text").GetComponent<Text>();
        setupTitleText = GameObject.Find("Setup Title Text").GetComponent<Text>();
        playerCountTitleText = GameObject.Find("Player Count Title Text").GetComponent<Text>();
        mainTitleText = GameObject.Find("Main Title Text").GetComponent<Text>();
        mainPlayButtonText = GameObject.Find("Main Play Text").GetComponent<Text>();
        mainRuleButtonText = GameObject.Find("Main Rules Text").GetComponent<Text>();
        mainOptionsButtonText = GameObject.Find("Main Options Text").GetComponent<Text>();  
        mainCreditsButtonText = GameObject.Find("Main Credits Text").GetComponent<Text>();
        optionsTitleText = GameObject.Find("Options Title Text").GetComponent<Text>();
        optionsOKButtonText = GameObject.Find("Options OK Button Text").GetComponent<Text>();
        optionsMusicLabel = GameObject.Find("Option Music Label").GetComponent<Text>();
        optionsLanguageLabel = GameObject.Find("Option Language Label").GetComponent<Text>();
        explanationOKButtonText = GameObject.Find("Explanation OK Button Text").GetComponent<Text>();
        setupNameText = GameObject.Find("Setup Name Text").GetComponent<Text>();
        setupColorText = GameObject.Find("Setup Color Text").GetComponent<Text>();
        explanationText = GameObject.Find("Explanation Text").GetComponent<Text>();
        setupOKButton = GameObject.Find("Setup OK Button").GetComponent<Text>();
        messageDefaultOKButton = GameObject.Find("Message Default OK Button Text").GetComponent<Text>();
        messageSpecialOKButton = GameObject.Find("Message Special OK Button Text").GetComponent<Text>();
        optionsMergeCollideLabel = GameObject.Find("Option Merge Collide Label").GetComponent<Text>();
        messageCharacterSprite = GameObject.Find("Message Character Image").GetComponent<Image>();
        spriteSource = GameObject.Find("Sprite Holder").GetComponent<SpriteHolder>();        
        GUITextComponents = new List<Text>(){
                rollButtonText,
                rollTitleText,
                winMenuText,
                winTitleText,
                selectMoveTitleText,
                nextTurnOKButtonText,
                moveOrderTitleText,
                moveOrderText,
                moveOrderOKButtonText,
                setupTitleText,
                playerCountTitleText,
                mainTitleText,
                mainPlayButtonText,
                mainRuleButtonText,
                mainOptionsButtonText,
                mainCreditsButtonText,
                optionsTitleText,
                optionsMusicLabel,
                optionsOKButtonText,
                optionsLanguageLabel,
                explanationOKButtonText,
                setupNameText,
                setupColorText,
                setupOKButton,
                messageSpecialOKButton,
                messageDefaultOKButton,
                optionsMergeCollideLabel
        };
        //analytics
        analyticsGameStarted = setupScreen.GetComponent<GameStarted>();
        analyticsApplicationExited = this.gameObject.GetComponent<ApplicationExited>();
        analyticsGameCompleted = GameObject.Find("Win Screen").GetComponent<GameCompleted>();
        analyticsInitialPathChosen = GameObject.Find("Message Screen").GetComponent<InitialPathChosen>();


    }

    // Update is called once per frame
    void Update()
    {

    }
}
