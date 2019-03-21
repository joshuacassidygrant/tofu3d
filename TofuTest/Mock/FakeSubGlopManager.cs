using TofuCore.Glops;

namespace TofuTests.Mock
{
    public class FakeSubGlopManager : GlopContainer<FakeGlop> {

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
