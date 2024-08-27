namespace PlanesRemake.Runtime.Input
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public class InputManager
    {
        private PlayerInput playerInput = null;
        private Dictionary<Type, InputController> typeInputControllerPairs = null;

        public InputManager(params InputController[] inputControllers)
        {
            playerInput = new GameObject("Player Input Reader").AddComponent<PlayerInput>();
            playerInput.Initialize();
            GameObject.DontDestroyOnLoad(playerInput);
            typeInputControllerPairs = new Dictionary<Type, InputController>();

            foreach(InputController inputController in inputControllers)
            {
                Debug.Assert(!typeInputControllerPairs.ContainsKey(inputController.GetType()), 
                    $"{GetType().Name}: You are trying to add the {inputController.GetType().Name} input more than once!");
                typeInputControllerPairs.Add(inputController.EntityToControlType, inputController);
            }
        }

        public void EnableInput(IInputControlableEntity entityToControl)
        {
            Debug.Assert(typeInputControllerPairs.TryGetValue(entityToControl.GetType(), out InputController inputControllerFound), 
                $"{GetType().Name}: No input controller was found for the {entityToControl.GetType().Name} entity.");
            playerInput.AddActiveInputController(inputControllerFound, entityToControl);
        }

        public void DisableInput(IInputControlableEntity entityControlled)
        {
            Debug.Assert(typeInputControllerPairs.TryGetValue(entityControlled.GetType(), out InputController inputControllerFound),
                $"{GetType().Name}: No input controller was found for the {entityControlled.GetType()} entity.");
            playerInput.RemoveActiveInputController(inputControllerFound);
        }
    }
}
