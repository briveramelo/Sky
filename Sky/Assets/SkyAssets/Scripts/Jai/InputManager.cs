using UnityEngine;
using GenericFunctions;
using System.Collections.Generic;

public interface IBegin
{
    void OnTouchBegin(int fingerId);
}

public interface IHold
{
    void OnTouchHeld();
}

public interface IEnd
{
    void OnTouchEnd();
}

public interface IFreezable
{
    bool IsFrozen { get; set; }
}

public interface IStickEngineId
{
    void SetStickEngineId(int id);
}

public interface IJaiId
{
    void SetJaiId(int id);
}

public class InputManager : MonoBehaviour, IFreezable, IStickEngineId, IJaiId
{
    public static Vector2 TouchSpot;

    #region Touch Handlers

    [SerializeField] private Joyfulstick _joyfulstick;
    [SerializeField] private Pauser _pauser;
    [SerializeField] private Selector[] _selectors;

    private BasketEngine _basketEngine;
    private Jai _jai;
    private List<IBegin> _beginners;
    private List<IHold> _holders;
    private List<IEnd> _enders;
    private IEnd _stickEnd;
    private IEnd _basketEnd;
    private IEnd _jaiEnd;

    #endregion

    private int _stickEngineFinger = -1;
    private int _jaiFinger = -1;

    private Vector2 _correctionPixels;
    private float _correctionPixelFactor;

    private void Start()
    {
        _jai = FindObjectOfType<Jai>();
        _basketEngine = FindObjectOfType<BasketEngine>();

        _beginners = new List<IBegin>(new IBegin[]
        {
            _joyfulstick,
            _jai,
            _pauser
        });
        _holders = new List<IHold>(new IHold[]
        {
            _basketEngine,
            _joyfulstick,
        });
        _enders = new List<IEnd>(_selectors);
        _stickEnd = _joyfulstick;
        _jaiEnd = _jai;
        _basketEnd = _basketEngine;

        var pixelFix = new Corrections(true);
        _correctionPixels = pixelFix.CorrectionPixels;
        _correctionPixelFactor = pixelFix.CorrectionPixelFactor;
    }

    #region IFreezable

    private bool _isFrozen;

    bool IFreezable.IsFrozen
    {
        get => _isFrozen;
        set => _isFrozen = value;
    }

    #endregion

    #region ISetIDs

    void IJaiId.SetJaiId(int id)
    {
        _jaiFinger = id;
    }

    void IStickEngineId.SetStickEngineId(int id)
    {
        _stickEngineFinger = id;
    }

    #endregion

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (var finger in Input.touches)
            {
                TouchSpot = (finger.position + _correctionPixels) * _correctionPixelFactor;
                //Debug.Log(touchSpot);
                if (finger.phase == TouchPhase.Began)
                {
                    _beginners.ForEach(beginner => beginner.OnTouchBegin(finger.fingerId));
                }
                else if (finger.phase == TouchPhase.Moved || finger.phase == TouchPhase.Stationary)
                {
                    if (finger.fingerId == _stickEngineFinger && !_isFrozen)
                    {
                        _holders.ForEach(holder => holder.OnTouchHeld());
                    }
                }
                else if (finger.phase == TouchPhase.Ended)
                {
                    if (finger.fingerId == _stickEngineFinger && !_isFrozen)
                    {
                        _stickEnd.OnTouchEnd();
                        _basketEnd.OnTouchEnd();
                        _stickEngineFinger = -1;
                    }
                    else if (finger.fingerId == _jaiFinger && !_isFrozen)
                    {
                        _jaiEnd.OnTouchEnd();
                        _jaiFinger = -1;
                    }
                    else
                    {
                        _enders.ForEach(ender => ender.OnTouchEnd());
                    }
                }
            }
        }
        else
        {
            TouchSpot = 10f * Constants.WorldDimensions.y * Vector2.up;
        }
    }
}