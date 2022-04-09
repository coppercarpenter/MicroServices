using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProecssEvent(string message);
    }
    public enum EventType
    {
        PlatformPublished,
        Undetermined
    }


}
