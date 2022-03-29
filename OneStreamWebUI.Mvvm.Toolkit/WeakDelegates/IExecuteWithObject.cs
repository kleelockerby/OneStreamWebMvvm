
namespace OneStreamWebUI.Mvvm.Toolkit
{
    public interface IExecuteWithObject
    {
        object Target { get; }
        void ExecuteWithObject(object parameter);
        void MarkForDeletion();
    }
}
