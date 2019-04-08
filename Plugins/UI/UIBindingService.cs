using System.Collections;
using System.Collections.Generic;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.UI {
    /*
     * This makes sure that all UI elements receive access to the ServiceContext.
     */

    public class UIBindingService : AbstractMonoService
    {
        public IServiceContext GetServiceContext()
        {
            return ServiceContext;
        }
    }
}

