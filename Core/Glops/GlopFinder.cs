using System.Collections;
using System.Collections.Generic;
using TofuCore.Glops;
using TofuCore.Service;
using UnityEngine;

public class GlopFinder : AbstractService
{
    private IServiceContext _serviceContext;

    public GlopFinder(IServiceContext context)
    {
        _serviceContext = context;
    }

    public Glop GetGlop(int id)
    {
        Glop glop = _serviceContext.FindGlopById(id);
        if (glop == null)
        {
            Debug.Log("No glop found for id " + id);
        }
        return glop;
    }

}
