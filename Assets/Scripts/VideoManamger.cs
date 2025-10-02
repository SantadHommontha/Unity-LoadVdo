using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;


[System.Serializable]
public class VideoSetting
{
    public bool playLoop = true;
    public string RootVideoFolder = "Videos";
    public string[] videoFolderName = { "vdo1", "vdo2", "vdo3", "vdo4" };
    public string[] videoFormat = { "mp4", "mov" };
}
public class VideoManamger : MonoBehaviour
{
    #region Static variable
    private static VideoManamger _instance;
    public static VideoManamger Instance
    {
        get
        {
            return _instance;
            // if (_instance == null)
            // {
            //     _instance = FindFirstObjectByType<VideoManamger>();
            //     if (_instance == null)
            //     {
            //         GameObject videoManamger = new GameObject("VideoManamger", typeof(VideoManamger));
            //         _instance = videoManamger.GetComponent<VideoManamger>();
            //     }
            //     return _instance;
            // }
            // else
            // {
            //     return _instance;
            // }

        }
    }
    #endregion
    #region Privete variable
    [SerializeField] private string rootVideoFolder = "Videos";
    [SerializeField] private string[] videoFolderName = { "vdo1", "vdo2", "vdo3", "vdo4" };
    [SerializeField] private string[] videoFormat = { "mp4", "mov" };
    private string[][] videoClipsPaths;
    private static bool applicationIsQuitting = false;
    private bool playLoop = true;

    private VideoSetting videoSetting;
    private string directory = Path.GetDirectoryName(Application.dataPath);
    #endregion
    #region  Public variable
    public Action OnPlayAll;
    public Action OnStopAll;
    public Action OnGetVideoPath;
    public bool PlayLoop => playLoop;
    #endregion


    [Header("Test")]
    public string[] pathTest1;
    public string[] pathTest2;
    public string[] pathTest3;
    public string[] pathTest4;

    #region  Unity funcetion
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }
    void Start()
    {
        LoadSetting();
        
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            LoadPath();
        }
        if (Input.GetKey(KeyCode.P))
        {
            OnPlayAll?.Invoke();
        }
        if (Input.GetKey(KeyCode.S))
        {
            OnStopAll?.Invoke();
        }
        if (Input.GetKey(KeyCode.G))
        {
            OnGetVideoPath?.Invoke();
        }
        if (Input.GetKey(KeyCode.L))
        {
            LoadSetting();
        }
    }


    void OnDestroy()
    {
        applicationIsQuitting = true;
    }
    #endregion
    #region Load and save setting
    // โหลดตัวตั่งค่า
    private void LoadSetting()
    {
        var path = Path.Combine(GetVideoSetting(), "VideoSetting.json");
        VideoSetting _videoSetting = new VideoSetting();
        if (File.Exists(path))
        {
            var content = File.ReadAllText(path);
            _videoSetting = JsonUtility.FromJson<VideoSetting>(content);
        }
        else
        {
            // _videoSetting = new VideoSetting();
            // var jsonData = JsonUtility.ToJson(_videoSetting);

            //      File.WriteAllText(path, jsonData);
            SaveSetting(_videoSetting, path);
        }

        Setting(_videoSetting);

    }
    // Save ตั่งค่า
    private void SaveSetting(VideoSetting _videoSetting, string _path)
    {
        JsonUtility.ToJson(_videoSetting);
        var jsonData = JsonUtility.ToJson(_videoSetting);
        File.WriteAllText(_path, jsonData);
    }
     // สร้างตัวไฟล์ Save
    [ContextMenu("CreateSave")]
    private void CreateSave()
    {
        SaveSetting(new VideoSetting(), Path.Combine(GetVideoSetting(), "VideoSetting.json"));
    }
    // เอาค่า settinh อ่านมาไปใส่นตัวแปร
    private void Setting(VideoSetting _videoSetting)
    {
        videoSetting = _videoSetting;

        playLoop = _videoSetting.playLoop;
        rootVideoFolder = _videoSetting.RootVideoFolder;
        videoFolderName = _videoSetting.videoFolderName;
        videoFormat = _videoSetting.videoFormat;
    }
    #endregion

    #region  Get path
#if UNITY_EDITOR
    private string GetVideoFloder(string _floderName)
    {

        return Path.Combine(directory, "Assets", _floderName);
    }
    private string GetVideoSetting()
    {
        return Path.Combine(directory, "Assets");
    }

#else
    private string GetVideoFloder(string _floderName)
    {
        return Path.Combine(directory, _floderName);
    }

    private string GetVideoSetting()
    {
        return directory;
    }
#endif

    #endregion


    #region Debug Value
   // ดูค่าใน videoClipsPaths
    private void ShowTest()
    {
        pathTest1 = videoClipsPaths[0];
        pathTest2 = videoClipsPaths[1];
        pathTest3 = videoClipsPaths[2];
        pathTest4 = videoClipsPaths[3];
    }
    #endregion


    #region  Load and get path
    // เอาข้อมูลใน videoClipsPaths ส่งออกไป
    public string[] GetVideoClipPath(int _number, int _l = 1)
    {
         Debug.Log("TT " + _l);
        if (videoClipsPaths == null)
        {
            LoadPath(); 
        }
        if (_number > videoClipsPaths.Length)
            {
                return null;
            }
            else
            {
                if (videoClipsPaths[_number].Length == 0 || videoClipsPaths[_number] == null)
                {
                    Debug.Log("LL " + _l);
                    if (_l > 0)
                    {
                        LoadPath();
                        var rt = GetVideoClipPath(_number, 0);
                        return rt;
                    }

                    return null;

                }
                else
                {
                    Debug.Log("BL " + _l);
                    return videoClipsPaths[_number];
                }
            }
    }


    // โหลดตัว path vdo ในอยู่ในโฟรเดอร์  vdo1,vdo2...
    private void LoadPath()
    {
        LoadSetting();
        var rootPath = GetVideoFloder(rootVideoFolder);

        videoClipsPaths = new string[4][];

        if (Directory.Exists(rootPath))
        {
            Debug.Log("Load file at" + rootPath);

            for (int i = 0; i < videoFolderName.Length; i++)
            {
                var filePath = Path.Combine(rootPath, videoFolderName[i]);
                List<string> allfilePath = new List<string>();
                for (int j = 0; j < videoFormat.Length; j++)
                {
                    var afp = Directory.GetFiles(filePath, "*" + videoFormat[j]);
                    foreach (var T in afp)
                    {
                        allfilePath.Add(T);
                    }
                }
                videoClipsPaths[i] = allfilePath.ToArray();
            }
            ShowTest();
        }
        else
        {
            Debug.Log("Not found file in " + rootPath);

        }


    }
    #endregion

}
