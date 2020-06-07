using System.Collections.Generic;
using TofuCore.ResourceLibrary;

namespace Scripts.AVFX
{
    public class AVFXLibrary : AbstractResourceLibrary<AVFXInstance>
    {
        public AVFXLibrary(Dictionary<string, AVFXInstance> contents)
        {
            _contents = contents;
        }
        

    }
}
