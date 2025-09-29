using System.IO;
using TMPro;

using UnityEngine;

using UnityEngine.Video;


[System.Serializable]
public class JsonData
{
    public string vdoLink1;
    public string vodLink2;
}
public class LoadJson : MonoBehaviour
{
    [SerializeField] private TMP_Text text_debug;

    public string[] g;
    public VideoPlayer videoPlayer;

    string directory = Path.GetDirectoryName(Application.dataPath);

    [SerializeField] private Material backMat;
    [SerializeField] private Material vdoMat;
    [SerializeField] private MeshRenderer meshRenderer;


#if UNITY_EDITOR
    private string GetVideoFloder(string _floderName)
    {
        return Path.Combine(directory, "Assets", "Videos", _floderName);
    }
#else
    private string GetVideoFloder(string _floderName)
    {
        return Path.Combine(directory, "Videos", _floderName);
    }
#endif
    void Awake()
    {
          meshRenderer.material = backMat;
        videoPlayer.url = "";
    }
    public void LoadAndPlayVideo(string _floderName)
    {
        // "file://"

        var path = GetVideoFloder(_floderName);

        if (Directory.Exists(path))
        {

            text_debug.text = "Loading File";
            string[] filePaths = Directory.GetFiles(path, "*.mp4");
            g = filePaths;

            string fileName = Path.GetFileName(filePaths[0]);
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = filePaths[0];
            // videoPlayer.Play();
            videoPlayer.Prepare();

            videoPlayer.prepareCompleted += OnPrepareCompleted;
            videoPlayer.loopPointReached += OnloopPointReached;
        }
        else
        {
            text_debug.text = $"Error: Directory not found at {path}";
        }
    }

    void OnPrepareCompleted(VideoPlayer vp)
    {
        videoPlayer.prepareCompleted -= OnPrepareCompleted;
        Debug.Log($"Load Script {gameObject.name}");
          meshRenderer.material = vdoMat;
        videoPlayer.Play();
    }
    private void OnloopPointReached(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnloopPointReached;
        meshRenderer.material = vdoMat;
    }

}
















// void LoadVideoFile()
// {
//     // 1. กำหนดเส้นทาง
//     string directory = Path.GetDirectoryName(Application.dataPath);
//     string jsonFilePath = Path.Combine(Path.Combine(directory, "Assets"), jsonFileName);

//     Debug.Log("Attempting to load JSON from: " + jsonFilePath);

//     // 2. ตรวจสอบว่าไฟล์มีอยู่หรือไม่
//     if (File.Exists(jsonFilePath))
//     {
//         try
//         {

//             string jsonString = File.ReadAllText(jsonFilePath);


//             jsonData = JsonUtility.FromJson<JsonData>(jsonString);

//             Debug.Log("Successfully loaded " + jsonData.vdoLink1);


//         }
//         catch (System.Exception e)
//         {
//             Debug.LogError("Error reading or parsing JSON file: " + e.Message);
//         }
//     }
//     else
//     {
//         Debug.LogError("JSON file not found at: " + jsonFilePath);
//     }
// }





