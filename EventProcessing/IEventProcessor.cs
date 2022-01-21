namespace MeetService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}