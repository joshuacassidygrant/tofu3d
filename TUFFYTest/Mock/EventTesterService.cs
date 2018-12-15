using TUFFYCore.Events;
using TUFFYCore.Service;

namespace Tests.Mock
{
    public class EventTesterService : AbstractService
    {

        public int MollycoddleCalled = 0;
        public int ZarfCalled = 0;
        public string LastZarfPayload;

        [Dependency] private EventContext _eventContext;


        public void Mollycoddled(EventPayload eP)
        {
            MollycoddleCalled++;
        }

        public void Zarfed(EventPayload eP)
        {
            string value = (string)eP.GetContent(PayloadContentType.String);
            LastZarfPayload = value;
            ZarfCalled++;
        }
    }
}
