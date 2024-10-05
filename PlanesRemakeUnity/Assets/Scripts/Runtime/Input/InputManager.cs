namespace PlanesRemake.Runtime.Input
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.InputSystem.EnhancedTouch;

    using PlanesRemake.Runtime.Utils;

    public class InputManager
    {
        private PlayerInput playerInput = null;
        private Dictionary<Type, InputController> typeInputControllerPairs = null;

        public InputManager(bool enableTouchControls)
        {
            playerInput = new GameObject("Player Input Reader").AddComponent<PlayerInput>();
            playerInput.Initialize();
            GameObject.DontDestroyOnLoad(playerInput);
            typeInputControllerPairs = new Dictionary<Type, InputController>();

            if(enableTouchControls)
            {
                EnhancedTouchSupport.Enable();
            }
        }

        public void AddInputController(params InputController[] inputControllers)
        {
            foreach(InputController inputController in inputControllers)
            {
                LoggerUtil.Assert(!typeInputControllerPairs.ContainsKey(inputController.GetType()), 
                    $"{GetType().Name}: You are trying to add the {inputController.GetType().Name} input more than once!");
                typeInputControllerPairs.Add(inputController.EntityToControlType, inputController);
            }
        }

        public void RemoveInputContoller(params InputController[] inputControllers)
        {
            foreach (InputController inputController in inputControllers)
            {
                LoggerUtil.Assert(!typeInputControllerPairs.ContainsKey(inputController.GetType()),
                    $"{GetType().Name}: You are trying to add the {inputController.GetType().Name} input more than once!");
                typeInputControllerPairs.Add(inputController.EntityToControlType, inputController);
            }
        }

        public void Dispose()
        {
            GameObject.Destroy(playerInput.gameObject);
            typeInputControllerPairs.Clear();

            if(EnhancedTouchSupport.enabled)
            {
                EnhancedTouchSupport.Disable();
            }
        }

        public InputController EnableInput(IInputControlableEntity entityToControl)
        {
            LoggerUtil.Assert(typeInputControllerPairs.TryGetValue(entityToControl.GetType(), out InputController inputControllerFound), 
                $"{GetType().Name}: No input controller was found for the {entityToControl.GetType().Name} entity.");
            playerInput.AddActiveInputController(inputControllerFound, entityToControl);
            return inputControllerFound;
        }

        public void DisableInput(IInputControlableEntity entityControlled)
        {
            LoggerUtil.Assert(typeInputControllerPairs.TryGetValue(entityControlled.GetType(), out InputController inputControllerFound),
                $"{GetType().Name}: No input controller was found for the {entityControlled.GetType()} entity.");
            playerInput.RemoveActiveInputController(inputControllerFound);
        }
    }
}
