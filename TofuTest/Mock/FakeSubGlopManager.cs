using TofuCore.Glops;

namespace TofuTests.Mock
{
    public class FakeSubGlopManager : GlopContainer {

        public int UseNextId()
        {
            return GenerateGlopId();
        }

        public void SpawnFakeGlop()
        {
            int id = UseNextId();
            Contents.Add(id, new FakeGlop(id, ServiceContext));
        }
	
    }
}
