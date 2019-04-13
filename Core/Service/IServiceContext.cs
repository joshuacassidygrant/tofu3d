﻿using TofuCore.Glops;

namespace TofuCore.Service
{
    public interface IServiceContext
    {
        void Drop(string name);
        void Bind(string name, IService service);
        dynamic Fetch(string name);
        bool Has(string name);
        void AddAlias(string key, string alias);
        Glop FindGlopById(int id);
        void FullInitialization();
        void FullInitialization(IService service);
        int LastGlopId { get; set; }
    }
}