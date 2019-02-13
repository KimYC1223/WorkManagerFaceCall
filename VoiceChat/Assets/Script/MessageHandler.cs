using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Academy.HoloToolkit.Sharing;
using Academy.HoloToolkit.Unity;
using UnityEngine.UI;

public class MessageHandler : Singleton<MessageHandler> {

    Texture2D texture;
    public RawImage target;
    public bool lock_flag = true;
    int MessageCount = 0;

    // Use this for initialization
    void Start () {
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
        MessageCount++;
        lock_flag = false;
        if (texture == null) {
            //texture = new Texture2D(896, 504, TextureFormat.RGB24, false);
            texture = new Texture2D(448, 252, TextureFormat., false);
            //texture = new Texture2D(160, 120, TextureFormat.RGBA32, false);
        }

            byte[] recievedByte;
            Debug.Log("This is CameraData Message.");
            recievedByte = CustomMessages.Instance.ReadByteArray(msg);
            Debug.Log("Length" + recievedByte.Length);
            texture.LoadRawTextureData(recievedByte);

            texture.Apply();
            target.texture = texture;
            CustomMessages.Instance.SendCameraDone();

    }

    
}
