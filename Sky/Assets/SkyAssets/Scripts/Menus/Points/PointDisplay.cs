using TMPro;
using UnityEngine;

public interface IPointsDisplayable : IDisplayable
{
    void DisplayPoints(int points);
}

public interface IDisplayable
{
    void ToggleDisplay(bool show);
}

public abstract class PointDisplay : MonoBehaviour, IPointsDisplayable
{
    [SerializeField] protected TextMeshProUGUI _myText;

    void IPointsDisplayable.DisplayPoints(int points)
    {
        DisplayPoints(points);
    }

    protected abstract void DisplayPoints(int points);
    public void ToggleDisplay(bool show)
    {
        _myText.enabled = show;
    }
}