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

        byte[] recievedByte;
        recievedByte = CustomMessages.Instance.ReadByteArray(msg);

        int img_width = CustomMessages.Instance.ReadInt(msg);
        int img_height = CustomMessages.Instance.ReadInt(msg);
        int type = CustomMessages.Instance.ReadInt(msg);
        byte[] YUVtoRGB;

        if (texture == null) {
            if (type == 0)     // 홀로렌즈일 경우
                texture = new Texture2D(img_width, img_height, TextureFormat.RGB24, false);
            else                // 일반 유니티 에디터일 경우
                texture = new Texture2D(img_width, img_height, TextureFormat.RGBA32, false);
        }

        // 받은 데이터가 홀로렌즈일 경우
        // YUV를 RGB로 변환하는 과정
        if (type == 0) {
            int img_size = img_width * img_height;
            YUVtoRGB = new byte[ img_size * 3 ];

            for (int proc_height = 0; proc_height < img_height ; proc_height++) {
                for (int proc_width=0; proc_width < img_width ; proc_width++ ) {
                    byte img_y = recievedByte[ ( proc_height * img_width ) + proc_width];
                    byte img_u =
                        recievedByte[img_size +
                        (proc_height >> 1) * img_width+
                        ((proc_width >> 1) * 2)];
                    byte img_v =
                        recievedByte[img_size +
                        (proc_height >> 1) * img_width +
                        ((proc_width >> 1) * 2) + 1];

                    int temp_r = (int)(img_y + 1.596 * (img_v - 128));
                    int temp_g = (int)(img_y - 0.813 * (img_u - 128) - 0.391 * (img_v - 128));
                    int temp_b = (int)(img_y + 2.018 * (img_u - 128));

                    byte img_r = (temp_r > 255 ? 255 : (temp_r < 0 ? 0 : temp_r));
                }
            }

            
        }
        
        

        texture.LoadRawTextureData(recievedByte);

        texture.Apply();
        target.texture = texture;
        CustomMessages.Instance.SendCameraDone();
    }

    
}
