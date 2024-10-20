//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/Runtime/Input/InputActions.inputactions
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

namespace PlanesRemake.Runtime.Input
{
    public partial class @InputActions: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""UiController"",
            ""id"": ""a44f8ec9-279d-431b-8b82-e8c34cfc4328"",
            ""actions"": [
                {
                    ""name"": ""GoBack"",
                    ""type"": ""Button"",
                    ""id"": ""38529c39-3bb5-4c1d-9aa7-68c1272d5f2c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b6b94f11-1b2c-40d2-a2a9-e8f058f048bd"",
                    ""path"": ""*/{Back}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GoBack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GameplayController"",
            ""id"": ""159b419b-bd8a-4eed-ae4c-c3abd00263cf"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""c9f4a25c-d685-413c-a063-0bba91db49e1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""e5a58aa1-7f9e-4c82-acad-435a995bb908"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""27208913-8c9e-41c9-bf43-17017944822a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""db97de66-3861-4e75-bb56-3123d20eb084"",
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
                    ""id"": ""9dd08bd8-bf59-4ad8-8245-467738638857"",
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
                    ""id"": ""e509e3f0-1325-493a-a8fd-365fc5300d43"",
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
                    ""id"": ""71038687-6640-47e7-9f19-b67a0a790613"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d4507f04-3fc0-4ca9-880e-42baafd7a2d4"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca733166-6590-4573-b21e-debf9a003db0"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // UiController
            m_UiController = asset.FindActionMap("UiController", throwIfNotFound: true);
            m_UiController_GoBack = m_UiController.FindAction("GoBack", throwIfNotFound: true);
            // GameplayController
            m_GameplayController = asset.FindActionMap("GameplayController", throwIfNotFound: true);
            m_GameplayController_Movement = m_GameplayController.FindAction("Movement", throwIfNotFound: true);
            m_GameplayController_Pause = m_GameplayController.FindAction("Pause", throwIfNotFound: true);
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

        // UiController
        private readonly InputActionMap m_UiController;
        private List<IUiControllerActions> m_UiControllerActionsCallbackInterfaces = new List<IUiControllerActions>();
        private readonly InputAction m_UiController_GoBack;
        public struct UiControllerActions
        {
            private @InputActions m_Wrapper;
            public UiControllerActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @GoBack => m_Wrapper.m_UiController_GoBack;
            public InputActionMap Get() { return m_Wrapper.m_UiController; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UiControllerActions set) { return set.Get(); }
            public void AddCallbacks(IUiControllerActions instance)
            {
                if (instance == null || m_Wrapper.m_UiControllerActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_UiControllerActionsCallbackInterfaces.Add(instance);
                @GoBack.started += instance.OnGoBack;
                @GoBack.performed += instance.OnGoBack;
                @GoBack.canceled += instance.OnGoBack;
            }

            private void UnregisterCallbacks(IUiControllerActions instance)
            {
                @GoBack.started -= instance.OnGoBack;
                @GoBack.performed -= instance.OnGoBack;
                @GoBack.canceled -= instance.OnGoBack;
            }

            public void RemoveCallbacks(IUiControllerActions instance)
            {
                if (m_Wrapper.m_UiControllerActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IUiControllerActions instance)
            {
                foreach (var item in m_Wrapper.m_UiControllerActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_UiControllerActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public UiControllerActions @UiController => new UiControllerActions(this);

        // GameplayController
        private readonly InputActionMap m_GameplayController;
        private List<IGameplayControllerActions> m_GameplayControllerActionsCallbackInterfaces = new List<IGameplayControllerActions>();
        private readonly InputAction m_GameplayController_Movement;
        private readonly InputAction m_GameplayController_Pause;
        public struct GameplayControllerActions
        {
            private @InputActions m_Wrapper;
            public GameplayControllerActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_GameplayController_Movement;
            public InputAction @Pause => m_Wrapper.m_GameplayController_Pause;
            public InputActionMap Get() { return m_Wrapper.m_GameplayController; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GameplayControllerActions set) { return set.Get(); }
            public void AddCallbacks(IGameplayControllerActions instance)
            {
                if (instance == null || m_Wrapper.m_GameplayControllerActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_GameplayControllerActionsCallbackInterfaces.Add(instance);
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }

            private void UnregisterCallbacks(IGameplayControllerActions instance)
            {
                @Movement.started -= instance.OnMovement;
                @Movement.performed -= instance.OnMovement;
                @Movement.canceled -= instance.OnMovement;
                @Pause.started -= instance.OnPause;
                @Pause.performed -= instance.OnPause;
                @Pause.canceled -= instance.OnPause;
            }

            public void RemoveCallbacks(IGameplayControllerActions instance)
            {
                if (m_Wrapper.m_GameplayControllerActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IGameplayControllerActions instance)
            {
                foreach (var item in m_Wrapper.m_GameplayControllerActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_GameplayControllerActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public GameplayControllerActions @GameplayController => new GameplayControllerActions(this);
        public interface IUiControllerActions
        {
            void OnGoBack(InputAction.CallbackContext context);
        }
        public interface IGameplayControllerActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnPause(InputAction.CallbackContext context);
        }
    }
}
