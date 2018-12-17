﻿using System;
using System.Collections.Generic;
using TUFFYCore.Service;
using UnityEngine;

/*
 * AbstractResourceLibrary provides functionality to load in and retrieve content.
 */
namespace TUFFYCore.ResourceLibrary
{
    public abstract class AbstractResourceLibrary<T> : AbstractService
    {

        protected string Prefix = "";
        protected string Path;
        protected Type Type;

        protected Dictionary<string, T> _contents;


        protected AbstractResourceLibrary(string path)
        {
            Type = typeof(T);
            Path = path;
            _contents = new Dictionary<string, T>();
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


        public Dictionary<string, T> GetCatalogue()
        {
            return _contents;
        }

        public T Get(string id)
        {
            if (Contains(id)) return _contents[id];

            return default(T);
        }

        public int CountMembers()
        {
            return _contents.Count;
        }


        public bool Contains(string id)
        {
            return _contents.ContainsKey(id);
        }

    }
}