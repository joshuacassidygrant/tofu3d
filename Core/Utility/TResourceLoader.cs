using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TResourceLoader<T> where T:UnityEngine.Object
{
    private List<T> _resources;
    public TResourceLoader(string path)
    {
        
        _resources = Resources.LoadAll<T>(path).ToList();
    }

    public List<T> GetContents()
    {
        return _resources;
    }
	
}
