using BRM.EventAnalysis.TmpPlayback;
using BRM.EventAnalysis.UnityPlayback;
using BRM.EventRecorder.UnityUi.Models;
using BRM.TextSerializers;
using UnityEngine;

public class ReplayContext : MonoBehaviour
{
    [SerializeField] private ReplayController _controller;
    [SerializeField] private TextAsset _eventText;
    
    private void Start()
    {
        var replayerFactory = new TmpReplayInstructionFactory();
        _controller.Initialize(replayerFactory.GetInstructions());
        
        var serializer = new UnityJsonSerializer();
        var recording = serializer.AsObject<EventAndAppPayload>(_eventText.text);
        _controller.Replay(recording);
    }
}
