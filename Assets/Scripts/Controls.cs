// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controls.inputactions'

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
            ""name"": ""Ship"",
            ""id"": ""94ce884b-8b98-4b10-97f1-db232c8e319e"",
            ""actions"": [
                {
                    ""name"": ""LMB"",
                    ""type"": ""Button"",
                    ""id"": ""97dd3950-5fa6-4b76-bdc4-6cfdad13a044"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""9802cebf-3215-41ae-a8bf-b38f5c9a3cc5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RMB"",
                    ""type"": ""Button"",
                    ""id"": ""558ffc64-074b-4450-a4dc-059f8e1ff75b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""571c732c-c225-4e74-afbf-b8c29a17b8de"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eef1f7d3-e1d4-40b4-8ec9-2ed07aeecddf"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9654a6b2-bd6a-4c6b-8415-7a010f68eacd"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Ship
        m_Ship = asset.FindActionMap("Ship", throwIfNotFound: true);
        m_Ship_LMB = m_Ship.FindAction("LMB", throwIfNotFound: true);
        m_Ship_MousePosition = m_Ship.FindAction("MousePosition", throwIfNotFound: true);
        m_Ship_RMB = m_Ship.FindAction("RMB", throwIfNotFound: true);
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

    // Ship
    private readonly InputActionMap m_Ship;
    private IShipActions m_ShipActionsCallbackInterface;
    private readonly InputAction m_Ship_LMB;
    private readonly InputAction m_Ship_MousePosition;
    private readonly InputAction m_Ship_RMB;
    public struct ShipActions
    {
        private @Controls m_Wrapper;
        public ShipActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LMB => m_Wrapper.m_Ship_LMB;
        public InputAction @MousePosition => m_Wrapper.m_Ship_MousePosition;
        public InputAction @RMB => m_Wrapper.m_Ship_RMB;
        public InputActionMap Get() { return m_Wrapper.m_Ship; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShipActions set) { return set.Get(); }
        public void SetCallbacks(IShipActions instance)
        {
            if (m_Wrapper.m_ShipActionsCallbackInterface != null)
            {
                @LMB.started -= m_Wrapper.m_ShipActionsCallbackInterface.OnLMB;
                @LMB.performed -= m_Wrapper.m_ShipActionsCallbackInterface.OnLMB;
                @LMB.canceled -= m_Wrapper.m_ShipActionsCallbackInterface.OnLMB;
                @MousePosition.started -= m_Wrapper.m_ShipActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_ShipActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_ShipActionsCallbackInterface.OnMousePosition;
                @RMB.started -= m_Wrapper.m_ShipActionsCallbackInterface.OnRMB;
                @RMB.performed -= m_Wrapper.m_ShipActionsCallbackInterface.OnRMB;
                @RMB.canceled -= m_Wrapper.m_ShipActionsCallbackInterface.OnRMB;
            }
            m_Wrapper.m_ShipActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LMB.started += instance.OnLMB;
                @LMB.performed += instance.OnLMB;
                @LMB.canceled += instance.OnLMB;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @RMB.started += instance.OnRMB;
                @RMB.performed += instance.OnRMB;
                @RMB.canceled += instance.OnRMB;
            }
        }
    }
    public ShipActions @Ship => new ShipActions(this);
    public interface IShipActions
    {
        void OnLMB(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnRMB(InputAction.CallbackContext context);
    }
}
