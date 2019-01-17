using TofuCore.Glops;

namespace TofuTests.Mock
{
    public class FakeSubGlopManager : GlopManager {

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
