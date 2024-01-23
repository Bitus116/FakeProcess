using FakeProcess.Domain.Enums;

namespace FakeProcess.Domain.Interfaces
{
    public interface IProcess
    {
        int Id { get; }
        string Name { get; }
        ProcessType Type { get; }
        ProcessState State { get; }
        string ProgressLable { get; }
        void Start();
        void Stop();
    }
}
