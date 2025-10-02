
using UnityEngine;
using UnityEngine.Video;


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(VideoPlayer))]
public class DisplayScreenVideo : MonoBehaviour
{
    #region  Public variable
    [Tooltip("Start 1")]
    [SerializeField] private int videoPlayerNumber;

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private MeshRenderer meshRenderer;
    [Space]
    [SerializeField] private Material blackMat;
    [SerializeField] private Material vdoMat;
    [Space]
    [SerializeField] private string[] videoClipPath;

    private int currentVideoIndex = 0;
    #endregion

    #region Unity Function
    private void OnEnable()
    {
        if (VideoManamger.Instance)
        {
            VideoManamger.Instance.OnPlayAll += PlayVideo;
            VideoManamger.Instance.OnStopAll += StopVideo;
            VideoManamger.Instance.OnGetVideoPath += LoadVideoClipPath;
        }

    }
    private void OnDisable()
    {
        if (VideoManamger.Instance)
        {
            VideoManamger.Instance.OnPlayAll -= PlayVideo;
            VideoManamger.Instance.OnStopAll -= StopVideo;
            VideoManamger.Instance.OnGetVideoPath -= LoadVideoClipPath;
        }


    }

    private void OnDestroy()
    {
        if (VideoManamger.Instance)
        {
            VideoManamger.Instance.OnPlayAll -= PlayVideo;
            VideoManamger.Instance.OnStopAll -= StopVideo;
            VideoManamger.Instance.OnGetVideoPath -= LoadVideoClipPath;
        }

    }

    private void Start()
    {
        SetUP();
    }

    #endregion

    private void SetUP()
    {
        if (!videoPlayer) videoPlayer = GetComponent<VideoPlayer>();
        if (!meshRenderer) meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = blackMat;
        // vdoMat = meshRenderer.material;
        currentVideoIndex = 0;
    }
    // ไปเอา path ของ video ที่เก็บไว้ใน videoManager
    public void LoadVideoClipPath()
    {
        videoClipPath = VideoManamger.Instance.GetVideoClipPath(videoPlayerNumber - 1);
    }

    private int GetVideoIndex()
    {
        if (currentVideoIndex < videoClipPath.Length)
        {
            return currentVideoIndex++;
        }
        else
        {
            currentVideoIndex = 0;
            return currentVideoIndex;
        }
    }

    // เล่น vdo
    public void PlayVideo()
    {
        if (videoClipPath == null || videoClipPath.Length == 0) LoadVideoClipPath();


        if (videoClipPath == null || videoClipPath.Length == 0) return;

        SetUP();

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoClipPath[GetVideoIndex()];

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepareCompleted;
        videoPlayer.loopPointReached += OnVideoLoopPointReached;
    }
    // หยุดเล่น vdo 
    public void StopVideo()
    {
        videoPlayer.prepareCompleted -= OnVideoPrepareCompleted;
        videoPlayer.loopPointReached -= OnVideoLoopPointReached;

        videoPlayer.Stop();
        SetUP();
    }
    // หลังจากตัว videoPlayer ทำการ Prepare เสร็จแล้ว
    private void OnVideoPrepareCompleted(VideoPlayer _vp)
    {
        _vp.prepareCompleted -= OnVideoPrepareCompleted;
        meshRenderer.material = vdoMat;
        _vp.Play();
    }
    // หลังจากเล่น vdo จบ
    private void OnVideoLoopPointReached(VideoPlayer _vp)
    {
        _vp.loopPointReached -= OnVideoLoopPointReached;
        _vp.url = "";
        meshRenderer.material = blackMat;
        if (VideoManamger.Instance.PlayLoop)
        {
            PlayVideo();
        }

    }
}
