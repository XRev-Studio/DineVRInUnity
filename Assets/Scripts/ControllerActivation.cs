using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ControllerActivation : MonoBehaviour
{
    public ActionBasedController controller;

    private void OnEnable()
    {
        //controller.activateAction.action.performed += Action_performed;
      
    }
   
    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            // Controller is activated
            Debug.Log("Controller Activated");
        }
        else if (context.canceled)
        {
            // Controller is deactivated
            Debug.Log("Controller Deactivated");
        }
    }

    private void OnDisable()
    {
        controller.activateAction.action.performed -= Action_performed;
    }

}
