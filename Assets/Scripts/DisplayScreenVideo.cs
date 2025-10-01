using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(VideoPlayer))]
public class DisplayScreenVideo : MonoBehaviour
{
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


    private void OnEnable()
    {
        VideoManamger.Instance.OnPlayAll += PlayVideo;
        VideoManamger.Instance.OnStopAll += StopVideo;
        VideoManamger.Instance.OnGetVideoPath += LoadVideoClipPath;
    }
    private void OnDisable()
    {
        VideoManamger.Instance.OnPlayAll -= PlayVideo;
        VideoManamger.Instance.OnStopAll -= StopVideo;
        VideoManamger.Instance.OnGetVideoPath -= LoadVideoClipPath;
    }

    private void OnDestroy()
    {
        VideoManamger.Instance.OnPlayAll -= PlayVideo;
        VideoManamger.Instance.OnStopAll -= StopVideo;
        VideoManamger.Instance.OnGetVideoPath -= LoadVideoClipPath;
    }

    private void Start()
    {
        SetUP();
    }



    private void SetUP()
    {
        if (!videoPlayer) videoPlayer = GetComponent<VideoPlayer>();
        if (!meshRenderer) meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = blackMat;
       // vdoMat = meshRenderer.material;
        currentVideoIndex = 0;
    }

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


    public void PlayVideo()
    {
        if (videoClipPath == null) GetVideoIndex();


        if (videoClipPath == null) return;



        meshRenderer.material = blackMat;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoClipPath[GetVideoIndex()];

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepareCompleted;
        videoPlayer.loopPointReached += OnVideoLoopPointReached;
    }
    public void StopVideo()
    {
        videoPlayer.prepareCompleted -= OnVideoPrepareCompleted;
        videoPlayer.loopPointReached -= OnVideoLoopPointReached;

        videoPlayer.Stop();
    }

    private void OnVideoPrepareCompleted(VideoPlayer _vp)
    {
        _vp.prepareCompleted -= OnVideoPrepareCompleted;
        meshRenderer.material = vdoMat;
        _vp.Play();
    }

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
