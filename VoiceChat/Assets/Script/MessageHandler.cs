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

    double[][] DCT_table = new double[8][];
    byte[][] quantizationTable_Luminance = new byte[8][];
    byte[][] quantizationTable_chrominance = new byte[8][];
    // Use this for initialization
    void Start () {
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.Calling] = this.OnCalling;
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.hanging] = this.Onhanging;
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.Receiving] = this.OnReceiving;
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.Login] = this.OnLogin;
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.Logout] = this.OnLogout;
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.CameraData] = this.OnCameraData;

        SharingSessionTracker.Instance.SessionJoined += Instance_SessionJoined;

        DCT_table[0] = new double[8] { 0.3536, 0.3536, 0.3536, 0.3536, 0.3536, 0.3536, 0.3536, 0.3536 };
        DCT_table[1] = new double[8] { 0.4904, 0.4157, 0.2778, 0.0975, -0.0975, -0.2778, -0.4157, -0.4904 };
        DCT_table[2] = new double[8] { 0.4619, 0.1913, -0.1913, -0.4619, -0.4619, -0.1913, 0.1913, 0.4619 };
        DCT_table[3] = new double[8] { 0.4157, -0.0975, -0.4904, -0.2778, 0.2778, 0.4904, 0.0975, -0.4157 };
        DCT_table[4] = new double[8] { 0.3536, -0.3536, -0.3536, 0.3536, 0.3536, -0.3536, -0.3536, 0.3536 };
        DCT_table[5] = new double[8] { 0.2778, -0.4904, 0.0975, 0.4157, -0.4157, -0.0975, 0.4904, -0.2778 };
        DCT_table[6] = new double[8] { 0.1913, -0.4619, 0.4619, -0.1913, -0.1913, 0.4619, -0.4619, 0.1913 };
        DCT_table[7] = new double[8] { 0.0975, -0.2778, 0.4157, -0.4904, 0.4904, -0.4157, 0.2778, -0.0975 };

        quantizationTable_Luminance[0] = new byte[8] { 16, 11, 10, 16, 24, 40, 51, 61 };
        quantizationTable_Luminance[1] = new byte[8] { 12, 12, 14, 19, 26, 58, 60, 66 };
        quantizationTable_Luminance[2] = new byte[8] { 14, 13, 16, 24, 40, 57, 69, 57 };
        quantizationTable_Luminance[3] = new byte[8] { 14, 17, 22, 29, 51, 87, 80, 62 };
        quantizationTable_Luminance[4] = new byte[8] { 18, 22, 37, 56, 68, 109, 103, 77 };
        quantizationTable_Luminance[5] = new byte[8] { 24, 36, 55, 64, 81, 104, 113, 92 };
        quantizationTable_Luminance[6] = new byte[8] { 49, 64, 78, 87, 103, 121, 120, 101 };
        quantizationTable_Luminance[7] = new byte[8] { 72, 92, 95, 98, 112, 100, 103, 99 };

        quantizationTable_chrominance[0] = new byte[8] { 17, 18, 24, 47, 99, 99, 99, 99 };
        quantizationTable_chrominance[1] = new byte[8] { 18, 21, 26, 66, 99, 99, 99, 99 };
        quantizationTable_chrominance[2] = new byte[8] { 24, 26, 56, 99, 99, 99, 99, 99 };
        quantizationTable_chrominance[3] = new byte[8] { 47, 66, 99, 99, 99, 99, 99, 99 };
        quantizationTable_chrominance[4] = new byte[8] { 99, 99, 99, 99, 99, 99, 99, 99 };
        quantizationTable_chrominance[5] = new byte[8] { 99, 99, 99, 99, 99, 99, 99, 99 };
        quantizationTable_chrominance[6] = new byte[8] { 99, 99, 99, 99, 99, 99, 99, 99 };
        quantizationTable_chrominance[7] = new byte[8] { 99, 99, 99, 99, 99, 99, 99, 99 };
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

            byte[][] DE_quantiByte = new byte[8][];
            for (int i = 0; i < 8; i++)
                DE_quantiByte[i] = new byte[8];

            byte[] IDCT_Byte = new byte[recievedByte.Length];

            float tmp = (img_height / 8.0f) + 0.5f;
            int DCT_H_MAX = Mathf.RoundToInt(tmp);

            tmp = (img_width / 8.0f) + 0.5f;
            int DCT_W_MAX = Mathf.RoundToInt(tmp);

            tmp = (img_width / 16.0f) + 0.5f;
            int DCT_UV_H_MAX = Mathf.RoundToInt(tmp);

            tmp = (img_width / 16.0f) + 0.5f;
            int DCT_UV_W_MAX = Mathf.RoundToInt(tmp);

            byte[][] IDCT_hor = new byte[8][];
            for (int i = 0; i < 8; i++)
                IDCT_hor[i] = new byte[8];

            byte[][] IDCT_ver = new byte[8][];
            for (int i = 0; i < 8; i++)
                IDCT_ver[i] = new byte[8];

            int offset;

            for (int proc_height = 0; proc_height < DCT_H_MAX; proc_height++) {
                for (int proc_width = 0; proc_width < DCT_W_MAX; proc_width++) {

                    for (int t = 0; t < 64; t++) {
                        offset = (img_width * (t / 8) + t % 8) +
                                    (proc_width * 8 + proc_height * img_width * 8);

                        if (offset < img_size)
                            DE_quantiByte[t / 8][t % 8] = (byte)
                                    (recievedByte[offset] * quantizationTable_Luminance[t / 8][t % 8]);
                    }


                }
            }


            for (int proc_height = 0; proc_height < img_height ; proc_height++) {
                for (int proc_width=0; proc_width < img_width ; proc_width++ ) {
                    byte img_y = IDCT_Byte[ ( proc_height * img_width ) + proc_width];
                    byte img_u =
                        IDCT_Byte[img_size +
                        (proc_height >> 1) * img_width+
                        ((proc_width >> 1) * 2)];
                    byte img_v =
                        IDCT_Byte[img_size +
                        (proc_height >> 1) * img_width +
                        ((proc_width >> 1) * 2) + 1];

                    int temp_r = (int)(img_y + 1.596 * (img_v - 128));
                    int temp_g = (int)(img_y - 0.813 * (img_u - 128) - 0.391 * (img_v - 128));
                    int temp_b = (int)(img_y + 2.018 * (img_u - 128));

                    byte img_r = (byte)(temp_r > 255 ? 255 : (temp_r < 0 ? 0 : temp_r));
                    byte img_g = (byte)(temp_g > 255 ? 255 : (temp_g < 0 ? 0 : temp_g));
                    byte img_b = (byte)(temp_b > 255 ? 255 : (temp_b < 0 ? 0 : temp_b));

                    YUVtoRGB[ ( proc_height * img_width + proc_width ) * 3 ] = img_r;
                    YUVtoRGB[ ( proc_height * img_width + proc_width ) * 3 + 1 ] = img_g;
                    YUVtoRGB[ ( proc_height * img_width + proc_width ) * 3 + 2 ] = img_b;
                }
            }

            texture.LoadRawTextureData(YUVtoRGB);
        }
        // 만약 그냥 웹캠에서 RGB로 보낸다면, 받은 어레이를 바로 바꿈
        else {
            texture.LoadRawTextureData(recievedByte);
        }
            
        texture.Apply();
        target.texture = texture;
        CustomMessages.Instance.SendCameraDone();
    }

    
}
