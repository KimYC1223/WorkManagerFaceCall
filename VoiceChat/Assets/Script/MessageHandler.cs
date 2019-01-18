using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Academy.HoloToolkit.Sharing;
using Academy.HoloToolkit.Unity;

public class MessageHandler : Singleton<MessageHandler> {

    // Use this for initialization
    void Start () {
        Debug.Log(Network.player.ipAddress);
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.Calling] = this.OnCalling;
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.hanging] = this.Onhanging;
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.Receiving] = this.OnReceiving;
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.Login] = this.OnLogin;
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.Logout] = this.OnLogout;
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.CameraData] = this.OnCameraData;

        SharingSessionTracker.Instance.SessionJoined += Instance_SessionJoined;
    }

    private void Instance_SessionJoined ( object sender , SharingSessionTracker.SessionJoinedEventArgs e ) {
  
    }

    void OnCalling(NetworkInMessage msg) {
        msg.ReadInt64();

        Debug.Log("This is Calling Message.");
        Debug.Log("srcIP : "+CustomMessages.Instance.ReadString(msg));
        Debug.Log("desIP : "+CustomMessages.Instance.ReadString(msg));
    }

    void Onhanging(NetworkInMessage msg) {
        msg.ReadInt64();

        Debug.Log("This is hanging Message.");
        Debug.Log("srcIP : " + CustomMessages.Instance.ReadString(msg));
        Debug.Log("desIP : " + CustomMessages.Instance.ReadString(msg));
    }

    void OnReceiving(NetworkInMessage msg) {
        msg.ReadInt64();

        Debug.Log("This is Receiving Message.");
        Debug.Log("srcIP : " + CustomMessages.Instance.ReadString(msg));
        Debug.Log("desIP : " + CustomMessages.Instance.ReadString(msg));
    }

    void OnLogin(NetworkInMessage msg) {
        msg.ReadInt64();

        Debug.Log("This is Login Message.");
        Debug.Log("srcIP : " + CustomMessages.Instance.ReadString(msg));
    }

    void OnLogout(NetworkInMessage msg) {
        msg.ReadInt64();

        Debug.Log("This is Logout Message.");
        Debug.Log("srcIP : " + CustomMessages.Instance.ReadString(msg));
    }

    void OnCameraData(NetworkInMessage msg) {
        msg.ReadInt64();

        Debug.Log("This is CameraData Message.");
        Debug.Log("data : " + CustomMessages.Instance.ReadByteArray(msg));
    }

}
