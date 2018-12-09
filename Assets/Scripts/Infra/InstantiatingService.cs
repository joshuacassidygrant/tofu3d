using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatingService : AbstractMonoService
{
    public override string[] Dependencies {
        get {
            return new string[]
            {
                "EventContext"
            };
        }
    }

    public int ObjectsInstantiated = 0;
    private EventContext _eventContext;

    public override void Initialize()
    {
        base.Initialize();
        BindListener(Events.SpawnUnit, SpawnUnit, _eventContext);
    }

    public GameObject DoInstantiate(GameObject obj, Vector3 place)
    {
        ObjectsInstantiated++;
        return GameObject.Instantiate(obj, place, Quaternion.identity);

    }

    public void SpawnUnit(EventPayload payload)
    {
        if (payload.ContentType != PayloadContentType.Unit)
        {
            Debug.Log("Incorrect content type");
            return;
        }

        Unit unit = (Unit) payload.GetContent();

        UnitRenderer renderer = DoInstantiate(new GameObject(), Vector3.down).AddComponent<UnitRenderer>();
        renderer.Initialize(unit);
    }
}
