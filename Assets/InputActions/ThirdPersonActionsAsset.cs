//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/InputActions/ThirdPersonActionsAsset.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @ThirdPersonActionsAsset: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ThirdPersonActionsAsset()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ThirdPersonActionsAsset"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""ad6c9061-a17b-4bbb-b693-75c06f942608"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""6ef2843e-221c-46b0-920c-7b4d0aa92079"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""76a983db-5e7d-4f06-9d42-7fe8e5d9120f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e06246f0-2ed7-4071-9912-d25a43f1199c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwapItem"",
                    ""type"": ""Button"",
                    ""id"": ""46a2a84f-16ce-4bbf-801d-c9e3e48d4a58"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""fb084cd0-eb7b-42d2-97ca-2783668cddd2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""7bed76e5-63fe-4f26-ba4e-649f5424b201"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Subspecies_Skill"",
                    ""type"": ""Button"",
                    ""id"": ""45ca3fbf-9c0e-46f1-b1ea-d1e171eaaa31"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Stat_Skill_1"",
                    ""type"": ""Button"",
                    ""id"": ""b53d187c-901a-40fe-8f2b-b595b51e99fe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Stat_Skill_2"",
                    ""type"": ""Button"",
                    ""id"": ""940a43f3-82a3-4bc6-87cd-a5d440c3e614"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""GetStats"",
                    ""type"": ""Button"",
                    ""id"": ""da9bf860-e95b-46b3-bb79-f49b10e91eb5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7ce41e12-d75e-4a63-8551-50173d533421"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""ec59fc6f-4b5a-42eb-b4d1-31f7dcf06a16"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""615e07ca-fe76-423a-be12-513e82891463"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0229a339-6fd3-4067-82db-4ba102ac5a5a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4da572c0-d36e-4e9b-b427-169638bdcc55"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a45fe2a1-ed42-4cf3-abdd-8949174d766e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a56ade99-d032-4ea0-9ce7-97800c50d70c"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd05b313-90e0-438b-b2bd-d7465f9c770b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e2070ae-3011-46f4-982a-1c0365a3463d"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b879776-532f-4faa-b216-65f2edf22b7a"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0824eb60-acfa-416e-b8a2-177f96593b9a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f06d2021-09b2-480b-ae62-134ec7baba13"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c401335-110a-49ba-ab63-5bfd6c604e67"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""12a0a30d-cf85-43ef-930d-2d57281c74d6"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3131bb6d-cb91-4738-a22e-d70f6465b749"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""537acb34-7553-4844-8c4d-a410e0eae08a"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""377b46e5-de93-4a2c-bba1-f05e27bca449"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97fc51d3-2810-44cf-a26e-6165b554501d"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Subspecies_Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d0d8ec7-6993-4c3d-b482-808f992abaf8"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Subspecies_Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7183fdda-91a8-445a-8e9d-2c3e2896b4e6"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Stat_Skill_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1a80f90a-8116-4306-8975-001d08536da4"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Stat_Skill_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27678f01-2fd1-4366-a6ae-fd37548af753"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Stat_Skill_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""572a5ced-d7f5-4b80-b5fe-761ee17799bd"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Stat_Skill_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3d54ab4-8096-4ebf-b945-417ff1c0ca8a"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GetStats"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""53b22d5b-24cd-4967-b15e-e6510d0bb632"",
            ""actions"": [
                {
                    ""name"": ""Menu Up"",
                    ""type"": ""Button"",
                    ""id"": ""b1aa17b9-5bda-45b6-8d59-447e11764913"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Menu Down"",
                    ""type"": ""Button"",
                    ""id"": ""c2fa1394-0b7a-45a1-ab71-77c3fe467d72"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Menu Left"",
                    ""type"": ""Button"",
                    ""id"": ""cbb3c128-045c-4f23-aa47-035a521401ac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Menu Right"",
                    ""type"": ""Button"",
                    ""id"": ""5e9057fd-0070-421b-a308-62964d1a2286"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f7b25911-69dd-43ab-9d4a-6e37ec6ff6a2"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d4015b8-160d-4fd8-986b-be9fd5c25703"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b29a244f-526a-4b38-850e-1c9f1c825671"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eaaca855-3338-41cb-a56f-8762e8156eaf"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e84c146-55d3-42a0-bd10-f7c9dd02367d"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_SwapItem = m_Player.FindAction("SwapItem", throwIfNotFound: true);
        m_Player_Dodge = m_Player.FindAction("Dodge", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_Subspecies_Skill = m_Player.FindAction("Subspecies_Skill", throwIfNotFound: true);
        m_Player_Stat_Skill_1 = m_Player.FindAction("Stat_Skill_1", throwIfNotFound: true);
        m_Player_Stat_Skill_2 = m_Player.FindAction("Stat_Skill_2", throwIfNotFound: true);
        m_Player_GetStats = m_Player.FindAction("GetStats", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_MenuUp = m_Menu.FindAction("Menu Up", throwIfNotFound: true);
        m_Menu_MenuDown = m_Menu.FindAction("Menu Down", throwIfNotFound: true);
        m_Menu_MenuLeft = m_Menu.FindAction("Menu Left", throwIfNotFound: true);
        m_Menu_MenuRight = m_Menu.FindAction("Menu Right", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_SwapItem;
    private readonly InputAction m_Player_Dodge;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_Subspecies_Skill;
    private readonly InputAction m_Player_Stat_Skill_1;
    private readonly InputAction m_Player_Stat_Skill_2;
    private readonly InputAction m_Player_GetStats;
    public struct PlayerActions
    {
        private @ThirdPersonActionsAsset m_Wrapper;
        public PlayerActions(@ThirdPersonActionsAsset wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @SwapItem => m_Wrapper.m_Player_SwapItem;
        public InputAction @Dodge => m_Wrapper.m_Player_Dodge;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @Subspecies_Skill => m_Wrapper.m_Player_Subspecies_Skill;
        public InputAction @Stat_Skill_1 => m_Wrapper.m_Player_Stat_Skill_1;
        public InputAction @Stat_Skill_2 => m_Wrapper.m_Player_Stat_Skill_2;
        public InputAction @GetStats => m_Wrapper.m_Player_GetStats;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @SwapItem.started += instance.OnSwapItem;
            @SwapItem.performed += instance.OnSwapItem;
            @SwapItem.canceled += instance.OnSwapItem;
            @Dodge.started += instance.OnDodge;
            @Dodge.performed += instance.OnDodge;
            @Dodge.canceled += instance.OnDodge;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @Subspecies_Skill.started += instance.OnSubspecies_Skill;
            @Subspecies_Skill.performed += instance.OnSubspecies_Skill;
            @Subspecies_Skill.canceled += instance.OnSubspecies_Skill;
            @Stat_Skill_1.started += instance.OnStat_Skill_1;
            @Stat_Skill_1.performed += instance.OnStat_Skill_1;
            @Stat_Skill_1.canceled += instance.OnStat_Skill_1;
            @Stat_Skill_2.started += instance.OnStat_Skill_2;
            @Stat_Skill_2.performed += instance.OnStat_Skill_2;
            @Stat_Skill_2.canceled += instance.OnStat_Skill_2;
            @GetStats.started += instance.OnGetStats;
            @GetStats.performed += instance.OnGetStats;
            @GetStats.canceled += instance.OnGetStats;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @SwapItem.started -= instance.OnSwapItem;
            @SwapItem.performed -= instance.OnSwapItem;
            @SwapItem.canceled -= instance.OnSwapItem;
            @Dodge.started -= instance.OnDodge;
            @Dodge.performed -= instance.OnDodge;
            @Dodge.canceled -= instance.OnDodge;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @Subspecies_Skill.started -= instance.OnSubspecies_Skill;
            @Subspecies_Skill.performed -= instance.OnSubspecies_Skill;
            @Subspecies_Skill.canceled -= instance.OnSubspecies_Skill;
            @Stat_Skill_1.started -= instance.OnStat_Skill_1;
            @Stat_Skill_1.performed -= instance.OnStat_Skill_1;
            @Stat_Skill_1.canceled -= instance.OnStat_Skill_1;
            @Stat_Skill_2.started -= instance.OnStat_Skill_2;
            @Stat_Skill_2.performed -= instance.OnStat_Skill_2;
            @Stat_Skill_2.canceled -= instance.OnStat_Skill_2;
            @GetStats.started -= instance.OnGetStats;
            @GetStats.performed -= instance.OnGetStats;
            @GetStats.canceled -= instance.OnGetStats;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private List<IMenuActions> m_MenuActionsCallbackInterfaces = new List<IMenuActions>();
    private readonly InputAction m_Menu_MenuUp;
    private readonly InputAction m_Menu_MenuDown;
    private readonly InputAction m_Menu_MenuLeft;
    private readonly InputAction m_Menu_MenuRight;
    public struct MenuActions
    {
        private @ThirdPersonActionsAsset m_Wrapper;
        public MenuActions(@ThirdPersonActionsAsset wrapper) { m_Wrapper = wrapper; }
        public InputAction @MenuUp => m_Wrapper.m_Menu_MenuUp;
        public InputAction @MenuDown => m_Wrapper.m_Menu_MenuDown;
        public InputAction @MenuLeft => m_Wrapper.m_Menu_MenuLeft;
        public InputAction @MenuRight => m_Wrapper.m_Menu_MenuRight;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void AddCallbacks(IMenuActions instance)
        {
            if (instance == null || m_Wrapper.m_MenuActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MenuActionsCallbackInterfaces.Add(instance);
            @MenuUp.started += instance.OnMenuUp;
            @MenuUp.performed += instance.OnMenuUp;
            @MenuUp.canceled += instance.OnMenuUp;
            @MenuDown.started += instance.OnMenuDown;
            @MenuDown.performed += instance.OnMenuDown;
            @MenuDown.canceled += instance.OnMenuDown;
            @MenuLeft.started += instance.OnMenuLeft;
            @MenuLeft.performed += instance.OnMenuLeft;
            @MenuLeft.canceled += instance.OnMenuLeft;
            @MenuRight.started += instance.OnMenuRight;
            @MenuRight.performed += instance.OnMenuRight;
            @MenuRight.canceled += instance.OnMenuRight;
        }

        private void UnregisterCallbacks(IMenuActions instance)
        {
            @MenuUp.started -= instance.OnMenuUp;
            @MenuUp.performed -= instance.OnMenuUp;
            @MenuUp.canceled -= instance.OnMenuUp;
            @MenuDown.started -= instance.OnMenuDown;
            @MenuDown.performed -= instance.OnMenuDown;
            @MenuDown.canceled -= instance.OnMenuDown;
            @MenuLeft.started -= instance.OnMenuLeft;
            @MenuLeft.performed -= instance.OnMenuLeft;
            @MenuLeft.canceled -= instance.OnMenuLeft;
            @MenuRight.started -= instance.OnMenuRight;
            @MenuRight.performed -= instance.OnMenuRight;
            @MenuRight.canceled -= instance.OnMenuRight;
        }

        public void RemoveCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMenuActions instance)
        {
            foreach (var item in m_Wrapper.m_MenuActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MenuActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnSwapItem(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnSubspecies_Skill(InputAction.CallbackContext context);
        void OnStat_Skill_1(InputAction.CallbackContext context);
        void OnStat_Skill_2(InputAction.CallbackContext context);
        void OnGetStats(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnMenuUp(InputAction.CallbackContext context);
        void OnMenuDown(InputAction.CallbackContext context);
        void OnMenuLeft(InputAction.CallbackContext context);
        void OnMenuRight(InputAction.CallbackContext context);
    }
}
