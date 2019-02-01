using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Academy.HoloToolkit.Sharing;

public class ButtonHandler : MonoBehaviour {

	public enum MainUI : byte {
        MIN,
        HEAD_LogOut,
        HEAD_WebCam,
        SIDE_Exit,
        SIDE_Call_1,
        SIDE_Call_2,
        SIDE_Call_3,
        SIDE_Quit_1,
        SIDE_Quit_2,
        SIDE_Quit_3,
        SIDE_OK,
        SIDE_Cancel,
        MAX
    }

    public MainUI button;
    
    public void OnClick() {

        switch ( button ) {
            case MainUI.HEAD_LogOut:
                Debug.Log("HEAD_LogOut");
                break;

            case MainUI.HEAD_WebCam:
                Debug.Log("HEAD_WebCam");
                break;

            case MainUI.SIDE_Exit:
                Debug.Log("SIDE_Exit");
                break;

            case MainUI.SIDE_Call_1:
                Debug.Log("SIDE_Call_1");
                break;

            case MainUI.SIDE_Call_2:
                Debug.Log("SIDE_Call_2");
                break;

            case MainUI.SIDE_Call_3:
                Debug.Log("SIDE_Call_3");
                break;

            case MainUI.SIDE_Quit_1:
                Debug.Log("SIDE_Quit_1");
                break;

            case MainUI.SIDE_Quit_2:
                Debug.Log("SIDE_Quit_2");
                break;

            case MainUI.SIDE_Quit_3:
                Debug.Log("SIDE_Quit_3");
                break;

            case MainUI.SIDE_OK:
                Debug.Log("SIDE_OK");
                break;

            case MainUI.SIDE_Cancel:
                Debug.Log("SIDE_Cancel");
                break;
        }
    }
}
