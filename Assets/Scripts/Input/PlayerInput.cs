using UnityEngine;

/*
 * Handles input. TODO: specialize into command-based subclasses.
 */
public class PlayerInput : AbstractMonoService
{

    public override string[] Dependencies {
        get {
            return new string[]
            {
                "EventContext",
                "UnitManager",
                "TileMapService",
                "CameraControl"
            };
        }
    }

    private UnitManager _unitManager;
    private TileMapService _tileMapService;
    private CameraControl _cameraControl;
    private EventContext _eventContext;

    private InputMode _mode = InputMode.Default;
    private MapTile _hoverTile = null;
    private GameObject _placingObject = null;

	void Update ()
	{

	    HandleCameraMovement();

	    if (Input.GetButtonDown("Fire2"))
	    {
            CycleMode();
	    }


	    if (_mode == InputMode.Build)
	    {
	        if (_placingObject == null)
	        {
	            _placingObject = new GameObject();
	            _placingObject = Instantiate(Resources.Load<GameObject>("Towers/tower"));
	        }

	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
	        _hoverTile = _tileMapService.GetTile(worldPoint);
	        if (_hoverTile != null)
	        {
	            _placingObject.transform.position = _hoverTile.Location;



	            if (Input.GetButtonDown("Fire1"))
	            {

                    if (_hoverTile.BuildOnAble)
	                {
                        //Currently broken; Unit manager has no unit called tower.
	                    Unit tower = _unitManager.SpawnUnit("Tower", UnitType.Tower, _hoverTile);
	                    _hoverTile.BuildOn(tower);
	                }


                }

            }

        }

    }

    private void HandleCameraMovement()
    {
        Vector3 cameraVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            cameraVector += new Vector3(0, 1, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            cameraVector += new Vector3(0, -1, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            cameraVector += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            cameraVector += new Vector3(1, 0, 0);
        }

        _cameraControl.MoveBy(cameraVector);

    }

    private void CycleMode()
    {
        _mode = (InputMode)(((int) _mode + 1) % System.Enum.GetNames(typeof(InputMode)).Length);

        if (_mode != InputMode.Build)
        {
            Destroy(_placingObject);
        }
    }
}
