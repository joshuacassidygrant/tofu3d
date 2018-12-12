using System;
using TUFFYCore.Service;

/*
 * AbstractResourceLibrary provides functionality to load in and retrieve content.
 */
namespace TUFFYCore.ResourceLibrary
{
    public abstract class AbstractResourceLibrary : AbstractService
    {

        protected string Prefix = "";
        protected string Path;
        protected Type Type;


        protected AbstractResourceLibrary(Type type, string path)
        {
            Type = type;
            Path = path;
        }

        public void SetPrefix(string prefix)
        {
            Prefix = prefix;
        }

        public override void Build()
        {
            base.Build();
            LoadResources();
        }

        public abstract void LoadResources();

    }
}