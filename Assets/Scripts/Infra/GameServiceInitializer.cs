using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * Initializes the game scene by creating a ServiceContext and
 * binding services to it.
 */
public class GameServiceInitializer : MonoBehaviour
{
    private GameObject _tileMapObject;
    private GameObject _cameraObject;

    private ServiceContext _serviceContext;

    public void Awake()
    {

        SpawnUnityObjects();
        BindServices();
        InitServices();


    }

    private void BindServices()
    {


        _serviceContext = new ServiceContext();
        ServiceFactory serviceFactory = new ServiceFactory(_serviceContext);

        new EventContext().BindServiceContext(_serviceContext);
        new UnitManager().BindServiceContext(_serviceContext);
        new TileMapService().BindServiceContext(_serviceContext);
        NewSO("InstantiatingService").AddComponent<InstantiatingService>().BindServiceContext(_serviceContext);
        _tileMapObject.AddComponent<MapRenderer>().BindServiceContext(_serviceContext);
        _cameraObject.AddComponent<CameraControl>().BindServiceContext(_serviceContext);
        NewSO("PlayerInput").AddComponent<PlayerInput>().BindServiceContext(_serviceContext);

    }

    private void SpawnUnityObjects()
    {
        _tileMapObject = Instantiate(Resources.Load<GameObject>("Services/Grid"));
        _tileMapObject.name = "Grid";
        _cameraObject = Camera.main.gameObject;


    }

    private void InitServices()
    {
        _serviceContext.FullInitialization();

    }

    private GameObject NewSO(string typeName)
    {
        GameObject obj = new GameObject();
        obj.name = typeName;
        return obj;
    }
}
