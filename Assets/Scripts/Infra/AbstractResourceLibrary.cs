using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * AbstractResourceLibrary provides functionality to load in and retrieve content.
 */
public abstract class AbstractResourceLibrary: AbstractService
{

    protected string Prefix = "";
    protected string Path;
    protected Type Type;


    public AbstractResourceLibrary(Type type, string path)
    {
        Type = type;
        Path = path;
    }

    public void SetPrefix(string prefix)
    {
        Prefix = prefix;
    }

    public override void Initialize()
    {
        LoadResources();
    }

    public abstract void LoadResources();

}
