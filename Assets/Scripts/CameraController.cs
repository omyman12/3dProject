using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject maincam;
    public GameObject thirdcam;
    public int curCamera;

    public void ChangeCam(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curCamera == 0)
        {
            Thirdcam();
            curCamera = 1;
            Debug.Log("3rdcam");
        }
        else if (context.phase == InputActionPhase.Started && curCamera == 1)
        {
            Maincam();
            curCamera = 0;
            Debug.Log("maincam");
        }
    }
    void Maincam()
    {
        maincam.gameObject.SetActive(true);
        thirdcam.gameObject.SetActive(false);
    }

    void Thirdcam()
    {
        maincam.gameObject.SetActive(false);
        thirdcam.gameObject.SetActive(true);
    }
}
