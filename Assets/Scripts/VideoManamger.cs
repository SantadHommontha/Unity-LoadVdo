using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
public class VideoManamger : MonoBehaviour
{
    private static VideoManamger _instance;
    public static VideoManamger Instance
    {
        get
        {

            if (_instance == null)
            {
                _instance = FindFirstObjectByType<VideoManamger>();
                if (_instance == null)
                {
                    GameObject videoManamger = new GameObject("VideoManamger", typeof(VideoManamger));
                    _instance = videoManamger.GetComponent<VideoManamger>();
                }
                return _instance;
            }
            else
            {
                return _instance;
            }

        }
    }

    [SerializeField] private string rootVideoFolder = "Videos";
    [SerializeField] private string[] videoFolderName = { "vdo1", "vdo2", "vdo3", "vdo4" };
    [SerializeField] private string[] videoFormat = { "mp4", "mov" };
    private string[][] videoClipsPaths;

    private bool playLoop = true;
    public bool PlayLoop => playLoop;


    public Action OnPlayAll;
    public Action OnStopAll;
    public Action OnGetVideoPath;



    [Header("Test")]
    public string[] pathTest1;
    public string[] pathTest2;
    public string[] pathTest3;
    public string[] pathTest4;
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }
    void Start()
    {

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
    }
    string directory = Path.GetDirectoryName(Application.dataPath);
#if UNITY_EDITOR
    private string GetVideoFloder(string _floderName)
    {

        return Path.Combine(directory, "Assets", _floderName);
    }

#else
    private string GetVideoFloder(string _floderName)
    {
        return Path.Combine(directory, _floderName);
    }
#endif
    private void ShowTest()
    {
        pathTest1 = videoClipsPaths[0];
        pathTest2 = videoClipsPaths[1];
        pathTest3 = videoClipsPaths[2];
        pathTest4 = videoClipsPaths[3];
    }

    public string[] GetVideoClipPath(int _number)
    {
        if (_number >= videoClipsPaths.Length)
        {
            return null;
        }
        else
        {
            if (videoClipsPaths[_number].Length == 0 || videoClipsPaths[_number] == null) return null;
            else return videoClipsPaths[_number];
        }
    }



    private void LoadPath()
    {
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


}
