using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PcToHoloScreenSender : MonoBehaviour {
    public RawImage myScreen;
    public RawImage targetScreen;
    WebCamTexture webCamTexture;

    // Use this for initialization
    void Start () {
        webCamTexture = new WebCamTexture();
        myScreen.texture = webCamTexture;
        webCamTexture.Play();
        InvokeRepeating("sendData", 0.1f, 0.03f);
	}
	
	// Update is called once per frame
	void sendData () {
        try {
            // Image Resizing... W/2 , H/2
            int re_x = webCamTexture.width >> 2;
            int re_y = webCamTexture.height >> 2;
            
            Texture2D currentTexture = new Texture2D(re_x, re_y);
            Color[] colorArr = new Color[re_x * re_y];

            // 짝수번째 픽셀만 받아옴. 0,2,4,6 ...
            for (int j = 0; j < re_y; j++)
                for (int i = 0; i < re_x; i++)
                    colorArr[j * (re_x) + i] = webCamTexture.GetPixel(i << 2, j << 2);

            // 리사이징 결과를 currentTexture에 전달
            currentTexture.SetPixels(colorArr);

            // JPG로 인코딩
            byte[] sendData = currentTexture.EncodeToJPG(10);
            Debug.Log("Lenght = " + sendData.Length);

            //확인하는 부분
            Texture2D recieveTex = new Texture2D(re_x, re_y);
            recieveTex.LoadImage(sendData);
            targetScreen.texture = recieveTex;

            CustomMessages.Instance.SendCameraData(sendData, re_x, re_y);


        } catch ( System.Exception e) {
            Debug.Log(e.ToString());
        }
    }
}
