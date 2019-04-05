using TofuCore.Glops;

namespace TofuTests.TestSupport
{
    public class FakeSubGlopManager : GlopContainer<Glop> {

        public int UseNextId()
        {
            return GenerateGlopId();
        }

        public void SpawnFakeGlop()
        {
            FakeGlop glop = new FakeGlop();
            Register(glop);
        }
	
    }
}
