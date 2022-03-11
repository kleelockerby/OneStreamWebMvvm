
namespace OneStreamWebUI.Mvvm.Toolkit
{
    internal interface IWeakEventListener
    {
        bool IsAlive { get; }
        object? Source { get; }
        Delegate? Handler { get; }
        void StopListening();
    }
}
