using TofuCore.Glop;

namespace Assets.tofu3d.TofuTest.Mock
{
    public class FakeSubGlopManager : GlopManager {

        public int UseNextId()
        {
            return GenerateGlopId();
        }
	
    }
}
