namespace BRM.Sky.WaveEditor
{
    public interface ISelectable
    {
        void Select(bool isSelected);
        bool IsSelected { get; }
    }
}