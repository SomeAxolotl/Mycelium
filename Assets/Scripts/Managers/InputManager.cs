using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Refresh Inputs")]
    public ThirdPersonActionsAsset actionsAsset;
    [SerializeField][Tooltip("Which InputActions will trigger a hint refresh in the UI")] List<InputActionReference> inputActionsThatRefreshHints = new List<InputActionReference>();

    [Header("Controllers")]
    [SerializeField][Tooltip("Default color for hints if none is selected")] public Color defaultHintColor;
    [SerializeField][Tooltip("The specified controller controls to display. The name should be Keyboard, XBox, Playstation, or Switch exactly")] List<Controller> controllers = new List<Controller>();

    public enum ControllerNames
    {
        Keyboard,
        XBox,
        Playstation,
        Switch,
        None
    }
    public ControllerNames previousLatestController = ControllerNames.None;

    SensitivityManager sensitivityManager;

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

    void Start()
    {
        //UnityEngine.Debug.Log("Hey <color=ADD8E6>guys</color>! I just <color=green>realized</color> you can do <b>rich text</b> in <i>debug</i> logs!!!");

        sensitivityManager = Camera.main.GetComponent<SensitivityManager>();
    }

    void OnEnable()
    {
        InputSystem.onDeviceChange += ForcePauseWhenDeviceUnplugged;
        InputSystem.onDeviceChange += ResetObject;

        actionsAsset = new ThirdPersonActionsAsset();
        foreach (InputAction inputAction in inputActionsThatRefreshHints)
        {
            inputAction.performed += CheckDevice;
        }

        actionsAsset?.Enable();

        RefreshHUDHints();
        UpdateCameraControls();
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= ForcePauseWhenDeviceUnplugged;
        InputSystem.onDeviceChange -= ResetObject;

        foreach (InputAction inputAction in inputActionsThatRefreshHints)
        {
            inputAction.performed -= CheckDevice;
        }

        actionsAsset?.Disable();
    }

    void ResetObject(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
    {
        if (inputDeviceChange == InputDeviceChange.Added)
        {
            OnDisable();
            OnEnable();
        }
    }

    void ForcePauseWhenDeviceUnplugged(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
    {
        if (SceneManager.GetActiveScene().name != "Main Menu" && GlobalData.isAbleToPause && inputDeviceChange == InputDeviceChange.Removed)
        {
            if (GameObject.Find("PauseMenuCanvas") != null)
            {
                GameObject.Find("PauseMenuCanvas").GetComponent<PauseMenu>().Pause();
            }
        }
    }

    public void CheckDevice(InputAction.CallbackContext actionCallback)
    {
        string deviceName = actionCallback.control.device.name;

        //XBox
        if (deviceName.Contains("XInput"))
        {
            GlobalData.latestController = ControllerNames.XBox;
        }
        //Playstation
        else if (deviceName.Contains("DualShock"))
        {
            GlobalData.latestController = ControllerNames.Playstation;
        }
        //Switch or Pro Controller
        else if (deviceName.Contains("Switch"))
        {
            GlobalData.latestController = ControllerNames.Switch;
        }
        //Catch with Keyboard
        else
        {
            GlobalData.latestController = ControllerNames.Keyboard;
        }

        if (GlobalData.latestController != previousLatestController)
        {
            RefreshHUDHints();
            UpdateCameraControls();

            previousLatestController = GlobalData.latestController;
        }
    }

    void RefreshHUDHints()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null) return;

        HUDControls hudController = hud.GetComponent<HUDControls>();
        hudController.ChangeHUDControls(GetLatestController());
    }

    void UpdateCameraControls()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && sensitivityManager != null)
        {
            sensitivityManager.UpdateCamera();
        }
    }

    public Controller GetLatestController()
    {
        foreach (Controller controller in controllers)
        {
            if (controller.controllerName == GlobalData.latestController)
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

        [SerializeField] public bool usesMouse;

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

        [SerializeField] public ColoredHint uiMenuSwapLHint;
        [SerializeField] public ColoredHint uiMenuSwapRHint;

        [SerializeField] public ColoredHint uiSubConfirmHint;
        [SerializeField] public ColoredHint uiSubCancelConfirmHint;

        [SerializeField] public ColoredHint moreInfoHint;

        public ColoredHint GetHintFromInputActionReference(InputActionReference inputActionReference)
        {
            if (inputActionReference.action.name == Instance.actionsAsset.UI.MenuSwapL.name)
            {
                return uiMenuSwapLHint;
            }
            if (inputActionReference.action.name == Instance.actionsAsset.UI.MenuSwapR.name)
            {
                return uiMenuSwapRHint;
            }
            if (inputActionReference.action.name == Instance.actionsAsset.Player.Stat_Skill_1.name)
            {
                return statSkill1Hint;
            }
            if (inputActionReference.action.name == Instance.actionsAsset.Player.Stat_Skill_2.name)
            {
                return statSkill2Hint;
            }
            if (inputActionReference.action.name == Instance.actionsAsset.UI.MoreInfo.name)
            {
                return moreInfoHint;
            }

            return null;
        }
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
                controlColor = Instance.defaultHintColor;
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
