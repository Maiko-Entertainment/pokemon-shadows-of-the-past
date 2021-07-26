// GENERATED AUTOMATICALLY FROM 'Assets/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player Character"",
            ""id"": ""f1ab8124-d006-4733-be5f-78b6cbbc1bca"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""22f5ccfd-38e1-4a03-96d2-3c32d4f729ae"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""3f936620-252d-4116-b75d-d335d8ee7f53"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""82e61c7c-abca-4c5c-8094-367259b13fae"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""83fa6a9b-95c3-4d53-9f94-1a2701b54493"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""83ac22d6-be67-4150-a992-158b27cd3b33"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e304b3ad-ddd3-43e2-951f-49210e3f55ad"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player Character
        m_PlayerCharacter = asset.FindActionMap("Player Character", throwIfNotFound: true);
        m_PlayerCharacter_Movement = m_PlayerCharacter.FindAction("Movement", throwIfNotFound: true);
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

    // Player Character
    private readonly InputActionMap m_PlayerCharacter;
    private IPlayerCharacterActions m_PlayerCharacterActionsCallbackInterface;
    private readonly InputAction m_PlayerCharacter_Movement;
    public struct PlayerCharacterActions
    {
        private @Controls m_Wrapper;
        public PlayerCharacterActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerCharacter_Movement;
        public InputActionMap Get() { return m_Wrapper.m_PlayerCharacter; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerCharacterActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerCharacterActions instance)
        {
            if (m_Wrapper.m_PlayerCharacterActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerCharacterActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerCharacterActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerCharacterActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_PlayerCharacterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public PlayerCharacterActions @PlayerCharacter => new PlayerCharacterActions(this);
    public interface IPlayerCharacterActions
    {
        void OnMovement(InputAction.CallbackContext context);
    }
}
