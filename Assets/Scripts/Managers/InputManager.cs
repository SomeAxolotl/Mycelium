using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Refresh Inputs")]
    public ThirdPersonActionsAsset actionsAsset;
    [SerializeField][Tooltip("Which InputActions will trigger a hint refresh in the UI")] private List<InputActionReference> inputActionsThatRefreshHints = new List<InputActionReference>();

    [Header("Controllers")]
    [SerializeField][Tooltip("The specified controller controls to display. The name should be Keyboard, XBox, Playstation, or Switch exactly")] List<Controller> controllers = new List<Controller>();

    public InputDevice latestDevice {get; private set;}

    public enum ControllerNames
    {
        Keyboard,
        XBox,
        Playstation,
        Switch,
        None
    }
    private ControllerNames previousLatestController = ControllerNames.None;
    public ControllerNames latestController {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
        InputSystem.onDeviceChange += PauseWhenDeviceChange;

        actionsAsset = new ThirdPersonActionsAsset();
        foreach (InputAction inputAction in inputActionsThatRefreshHints)
        {
            inputAction.performed += CheckDevice;
        }

        actionsAsset?.Enable();
    }
    void OnDisable()
    {
        InputSystem.onDeviceChange -= PauseWhenDeviceChange;

        foreach (InputAction inputAction in inputActionsThatRefreshHints)
        {
            inputAction.performed -= CheckDevice;
        }

        actionsAsset?.Disable();
    }

    void PauseWhenDeviceChange(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
    {
        if (SceneManager.GetActiveScene().name != "Main Menu" && GlobalData.isAbleToPause && inputDeviceChange == InputDeviceChange.Removed)
        {
            GameObject.Find("PauseMenuCanvas").GetComponent<PauseMenu>()?.Pause();
        }
    }

    public void CheckDevice(InputAction.CallbackContext actionCallback)
    {
        latestDevice = actionCallback.control.device;
        string deviceName = actionCallback.control.device.name;

        //XBox
        if (deviceName.Contains("XBox"))
        {
            latestController = ControllerNames.XBox;
        }
        //Playstation
        else if (deviceName.Contains("DualShock"))
        {
            latestController = ControllerNames.Playstation;
        }
        //Switch or Pro Controller
        else if (deviceName.Contains("Switch"))
        {
            latestController = ControllerNames.Switch;
        }
        //Catch with Keyboard
        else
        {
            latestController = ControllerNames.Keyboard;
        }

        if (latestController != previousLatestController)
        {
            UnityEngine.Debug.Log("Refreshing UI with controller name: " + deviceName);
        }

        RefreshHUDHints();

        previousLatestController = latestController;
    }

    void RefreshHUDHints()
    {
        if (latestController != previousLatestController)
        {
            GameObject.Find("HUD").GetComponent<HUDControls>().ChangeHUDControls(GetLatestController());
        }
    }

    public Controller GetLatestController()
    {
        foreach (Controller controller in controllers)
        {
            if (controller.controllerName == latestController)
            {
                return controller;
            }
        }

        UnityEngine.Debug.LogError("GetLatestController() returning null. Not good! :3");
        return null;
    }

    [System.Serializable]
    public class Controller
    {
        [SerializeField] public ControllerNames controllerName;

        [SerializeField] public ColoredHint subspeciesSkillHint; //Will have square sprite for Playstation
        [SerializeField] public ColoredHint statSkill1Hint; //Will have triangle sprite for Playstation
        [SerializeField] public ColoredHint statSkill2Hint; //Will have circle sprite for Playstation
        [SerializeField] public ColoredHint dodgeHint;

        [SerializeField] public ColoredHint interactHint;
        [SerializeField] public ColoredHint salvageHint;

        [SerializeField] public ColoredHint attackHint;
        [SerializeField] public ColoredHint goalCameraHint;
        [SerializeField] public ColoredHint orientationCameraHint;
        [SerializeField] public ColoredHint showStatsHint;

        [SerializeField] public ColoredHint uiSubConfirm;
        [SerializeField] public ColoredHint uiSubCancelConfirm;
    }

    [System.Serializable]
    public class ColoredHint
    {
        [SerializeField] public bool isHintSprite;

        [SerializeField] public string controlText;
        [SerializeField][Tooltip("Will default to white if left as is (Unity is cringe and won't default values in serialized classes shoulda pressed the work button in Unreal haha cuz unreal is so fucking amazing i love unreal i would literally end my life to work in unreal fuck unity i hate unity it does nothing that unreal can do i should just press the make it work button in unreal im a schizo!)")] public Color controlColor;

        [SerializeField] public Sprite controlSprite;

        public string GenerateColoredHintString(bool shouldRemoveBrackets = false)
        {
            if (controlColor.a == 0)
            {
                controlColor.a = 1;
            }
            if (controlColor == Color.black)
            {
                controlColor = Color.white;
            }

            string coloredText;
            if (shouldRemoveBrackets)
            {
                coloredText = $"<color=#{ColorUtility.ToHtmlStringRGB(controlColor)}>{controlText}</color>";
            }
            else
            {
                coloredText = $"[<color=#{ColorUtility.ToHtmlStringRGB(controlColor)}>{controlText}</color>]";
            }

            return coloredText;
        }
    }
}
