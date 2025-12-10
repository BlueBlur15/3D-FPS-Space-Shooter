using UnityEngine;

public class TerminalInteractor : MonoBehaviour
{
    public TerminalController terminal;     // Reference to the TerminalController
    public Transform terminalTransform;     // Where the terminal is in the world
    public float useDistance = 3f;

    private bool isUsingTerminal = false;

    private FpsController fps;

    void Start()
    {
        fps = GetComponent<FpsController>();
    }

    
    void Update()
    {
        if (!isUsingTerminal)
        {
            float dist = Vector3.Distance(transform.position, terminalTransform.position);

            // Press E to enter terminal
            if (dist <= useDistance && Input.GetKeyDown(KeyCode.E))
            {
                isUsingTerminal = true;
                terminal.EnterTerminal();
                AudioManager.instance.PlaySFX(AudioManager.instance.terminalEnterSound);
            }
        }
        else
        {
            // Press Tab to exit terminal
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isUsingTerminal = false;
                terminal.ExitTerminal();
                AudioManager.instance.PlaySFX(AudioManager.instance.terminalExitSound);
            }
        }
    }
}
