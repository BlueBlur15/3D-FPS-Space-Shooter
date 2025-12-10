using UnityEngine;
using TMPro;   // TMP_InputField, TextMeshProUGUI
using UnityEngine.SceneManagement;

public class TerminalController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField commandInputField;      // bottom console input
    public TextMeshProUGUI outputScreenText;      // big monitor text

    [Header("Player")]
    public FpsController playerController;        // drag your player here

    [Header("Screen Text")]
    [TextArea(3, 10)]
    public string defaultScreenText;              // initial text (commands list)

    [TextArea(3, 10)]
    public string[] creditsPages;                 // fill in Inspector

    [TextArea(3, 10)]
    public string[] instructionPages;             // fill in Inspector

    [Header("Effects")]
    public TypewriterEffect typewriter;           // attach on big monitor text, drag here
    public string playSceneName = "Level one";
    public float playDelay = 5f;
    public float quitDelay = 5f;

    private enum TerminalMode { Default, Credits, Instructions }
    private TerminalMode currentMode = TerminalMode.Default;
    private int currentPageIndex = 0;

    private bool isUsingTerminal = false;

    void Start()
    {
        // Initialize screen at start
        ShowDefaultScreen();
    }

    // --------------------------------------------------------------------
    // Enter / Exit Terminal
    // --------------------------------------------------------------------

    // Called by TerminalInteractor when pressing E
    public void EnterTerminal()
    {
        isUsingTerminal = true;

        if (playerController != null)
            playerController.SetControlsLocked(true);

        if (commandInputField != null)
        {
            commandInputField.text = string.Empty;
            commandInputField.ActivateInputField();
        }
    }

    // Called by TerminalInteractor when pressing Tab
    public void ExitTerminal()
    {
        isUsingTerminal = false;

        if (playerController != null)
            playerController.SetControlsLocked(false);

        if (commandInputField != null)
            commandInputField.DeactivateInputField();
    }

    // --------------------------------------------------------------------
    // Command input
    // --------------------------------------------------------------------

    // Hook this to the InputField's OnEndEdit / OnSubmit event
    public void OnCommandSubmitted(string rawCommand)
    {
        Debug.Log($"[Terminal] Submitted: '{rawCommand}'");

        if (string.IsNullOrWhiteSpace(rawCommand))
        {
            if (commandInputField != null)
                commandInputField.ActivateInputField();
            return;
        }

        string cmd = rawCommand.Trim().ToLowerInvariant();

        // Clear input box & refocus right away
        if (commandInputField != null)
        {
            commandInputField.text = string.Empty;
            commandInputField.ActivateInputField();
        }

        switch (cmd)
        {
            case "play":
                ShowPlayMessage();
                break;

            case "help":
                ShowDefaultScreen();
                break;

            case "credits":
                EnterCreditsMode();
                break;

            case "instructions":
            case "help2":
            case "instr":
                EnterInstructionsMode();
                break;

            case "quit":
                ShowQuitMessage();
                break;

            case "next":
                NextPage();
                break;

            case "back":
                PreviousPage();
                break;

            default:
                ShowUnknownCommand(cmd);
                break;
        }
    }

    // --------------------------------------------------------------------
    // Screen helpers
    // --------------------------------------------------------------------

    void SetScreenText(string text)
    {
        if (typewriter != null)
        {
            typewriter.TypeText(text);
        }
        else if (outputScreenText != null)
        {
            outputScreenText.text = text;
        }
    }

    void ShowDefaultScreen()
    {
        currentMode = TerminalMode.Default;
        currentPageIndex = 0;
        SetScreenText(defaultScreenText);
    }

    void EnterCreditsMode()
    {
        currentMode = TerminalMode.Credits;
        currentPageIndex = 0;
        UpdatePageText();
    }

    void EnterInstructionsMode()
    {
        currentMode = TerminalMode.Instructions;
        currentPageIndex = 0;
        UpdatePageText();
    }

    void NextPage()
    {
        if (currentMode == TerminalMode.Default) return;

        string[] pages = currentMode == TerminalMode.Credits ? creditsPages : instructionPages;
        if (pages == null || pages.Length == 0) return;

        currentPageIndex = Mathf.Clamp(currentPageIndex + 1, 0, pages.Length - 1);
        UpdatePageText();
    }

    void PreviousPage()
    {
        if (currentMode == TerminalMode.Default) return;

        string[] pages = currentMode == TerminalMode.Credits ? creditsPages : instructionPages;
        if (pages == null || pages.Length == 0) return;

        currentPageIndex = Mathf.Clamp(currentPageIndex - 1, 0, pages.Length - 1);
        UpdatePageText();
    }

    void UpdatePageText()
    {
        if (outputScreenText == null) return;

        if (currentMode == TerminalMode.Default)
        {
            SetScreenText(defaultScreenText);
            return;
        }

        string[] pages = currentMode == TerminalMode.Credits ? creditsPages : instructionPages;

        if (pages == null || pages.Length == 0)
        {
            SetScreenText("(no pages defined)");
            return;
        }

        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, pages.Length - 1);
        SetScreenText(pages[currentPageIndex]);
    }

    // --------------------------------------------------------------------
    // Messages for specific commands (play/quit/error)
    // --------------------------------------------------------------------

    void ShowPlayMessage()
    {
        SetScreenText("Starting mission in 5 seconds...");
        StartCoroutine(PlaySequence());
    }

    void ShowQuitMessage()
    {
        SetScreenText("Shutting down systems in 5 seconds...");
        StartCoroutine(QuitSequence());
    }

    void ShowUnknownCommand(string cmd)
    {
        SetScreenText($"Unknown command: {cmd}\nType HELP for a list of commands.");
    }

    private System.Collections.IEnumerator PlaySequence()
    {
        // Wait so the player can read the message
        yield return new WaitForSeconds(playDelay);

        if (!string.IsNullOrEmpty(playSceneName))
        {
            SceneManager.LoadScene(playSceneName);
        }
        else
        {
            Debug.LogWarning("[Terminal] playSCeneName is empty. Set it in the Inspector.");
        }
    }

    private System.Collections.IEnumerator QuitSequence()
    {
        yield return new WaitForSeconds(quitDelay);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in editor
#else
    Application.Quit();     // quits a build
#endif
    }
}
