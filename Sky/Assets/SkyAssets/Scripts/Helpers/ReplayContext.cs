using BRM.DebugAdapter;
using BRM.FileSerializers;
using BRM.InteractionAnalysis.TmpPlayback;
using BRM.InteractionAnalysis.UnityPlayback;
using BRM.InteractionRecorder.UnityUi.Models;
using BRM.TextSerializers;
using UnityEngine;

public class ReplayContext : MonoBehaviour
{
    [SerializeField] private ReplayController _controller;
    private void Start()
    {
        var replayerFactory = new TmpReplayInstructionFactory();
        _controller.Initialize(replayerFactory.GetInstructions());
        var fileSerializer = new TextFileSerializer(new UnityJsonSerializer(), new UnityDebugger());
        var recording = fileSerializer.Read<EventAndAppPayload>($"{Application.dataPath}/InteractionRecorder/JSON/interaction_data1.json");
        
        _controller.Replay(recording);
    }
}
