using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public struct DataFile
{
    public string imgName;
    public string imgURL;
    public string localization_EN_fileURL;
    public string localization_ID_fileURL;
}

public struct LocalizationData
{
    public string language;
    public string message;
}

public class DownloaderScript : MonoBehaviour
{
    DataFile _masterFile;
    public Text downloadInfo_Lbl;

    public Text imageName;
    public RawImage imageObj;

    bool hasMasterFile;
    bool imageHasDownloaded;

    public Text _greetingMessage;

    public List<LocalizationData> localizationDatas = new List<LocalizationData>();

    string jsonFileURL = "https://drive.google.com/uc?export=download&id=1t2OyAssxHnUJL8UWdoLF73rZtZ2M7RIf";

    void Start()
    {
        GetMasterFile();
    }

    void GetMasterFile()
    {
        StartCoroutine(GetMasterFile(jsonFileURL));
    }

    IEnumerator GetMasterFile(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            print("error downloading json");
        }
        else
        {
            _masterFile = JsonUtility.FromJson<DataFile>(request.downloadHandler.text);
            hasMasterFile = true;
            downloadInfo_Lbl.text = "Master file downloaded!";
        }

        // cleanup
        request.Dispose();
    }

    public void FetchImage()
    {
        if (!hasMasterFile) return;

        // load in the image
        StartCoroutine(GetImage(_masterFile.imgURL));
        downloadInfo_Lbl.text = "Downloading image file...";
    }

    IEnumerator GetImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            print("error downloading image");
        }
        else
        {
            ShowImage(((DownloadHandlerTexture)request.downloadHandler).texture);

            // update the text
            imageName.text = "Filename: " + _masterFile.imgName;
        }

        // cleanup
        request.Dispose();
        downloadInfo_Lbl.text = "Image file downloaded.";
    }

    void ShowImage(Texture2D tex)
    {
        imageObj.texture = tex;
        imageObj.CrossFadeAlpha(0, 0f, true);
        imageObj.CrossFadeAlpha(1, 1f, false);
    }

    public void FetchLocalizationFile(string languageID)
    {
        if (!hasMasterFile) return;

        string url = "";

        switch (languageID){
            case "ID":
                url = _masterFile.localization_ID_fileURL;
                break;
            case "EN":
                url = _masterFile.localization_EN_fileURL;
                break;
        }

        if (url == ""){
            print("localization ID not valid");
            return;
        }

        // load in the image
        StartCoroutine(GetLocalizationFile(url));
        downloadInfo_Lbl.text = "Downloading language pack...";
    }

    IEnumerator GetLocalizationFile(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            print("error downloading file");
        }
        else
        {
            LocalizationData localizationData = JsonUtility.FromJson<LocalizationData>(request.downloadHandler.text);
            ShowText(localizationData);
        }

        // cleanup
        request.Dispose();
        downloadInfo_Lbl.text = "Language pack downloaded.";
    }

    void ShowText(LocalizationData textData)
    {
        _greetingMessage.text = textData.message;
        _greetingMessage.CrossFadeAlpha(0, 0f, true);
        _greetingMessage.CrossFadeAlpha(1, 1f, false);
    }
}