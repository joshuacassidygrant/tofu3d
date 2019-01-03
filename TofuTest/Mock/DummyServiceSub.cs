using TofuCore.Service;

namespace TofuCore.TestSupport
{

    public class DummyServiceSub : DummyServiceOne {

        [Dependency("DummyLibrary2")] protected DummyLibrary DummyLibrary2;

        public bool HasDummyLibrary2()
        {
            return DummyLibrary2 != null;
        }

    }
}

