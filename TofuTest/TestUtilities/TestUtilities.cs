using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using TofuCore.Service;
using UnityEngine;

public class TestUtilities
{
    public static IServiceContext BuildSubServiceContextWithServices(Dictionary<string, object> services)
    {
        IServiceContext subContext = Substitute.For<IServiceContext>();

        foreach (KeyValuePair<string, object> entry in services)
        {
            subContext.Has(entry.Key).Returns(true);
            subContext.Fetch(entry.Key).Returns(entry.Value);
        }

        return subContext;
    }
}
