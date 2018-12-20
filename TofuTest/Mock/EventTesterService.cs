using TofuCore.Events;
using TofuCore.Service;

namespace TofuCore.TestSupport
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
            if (eP.ContentType == "String")
            {
                string value = eP.GetContent();
                LastZarfPayload = value;
            }

            ZarfCalled++;
        }
    }
}
