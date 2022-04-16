using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField]
    private DebugGrid debugGrid;

    [SerializeField]
    private Selector selector;

    [SerializeField]
    private ScenceUI scenceUI;

    private StepData stepData;

    private void StartStep()
    {
        selector.Locked = true;

        debugGrid.Move(stepData.Sphere.transform.position, stepData.Cell.transform.position);

        stepData.Cell.Highlight(true);

        stepData.Sphere.OnMoveComplete += Sphere_OnMoveComplete;

        stepData.Sphere.Move(stepData.Path);
    }

    private void CompleteStep()
    {
        selector.Locked = false;

        stepData.Sphere.OnMoveComplete -= Sphere_OnMoveComplete;

        stepData.Cell.Highlight(false);

        scenceUI.Point.Value += debugGrid.DestroyLines(stepData.Cell.transform.position);

        debugGrid.Generate(10); // generate ball by turn

        SetActiveMenu( debugGrid.GetEmptyCoords().Count == 0); //lose

    }

    private void Sphere_OnMoveComplete(Sphere sphere)
    {
        CompleteStep();
    }


    private void Selector_OnSelected(Sphere sphere, Cell cell)
    {
        var spherePos = sphere.transform.position;
        var cellPos = cell.transform.position;

        if(spherePos.x == cellPos.x && spherePos.z == cellPos.z)
        {
            return;
        }

        var path = debugGrid.GetPath(spherePos, cellPos);

        if(path.Count > 0) 
        {
            stepData = new StepData(sphere, cell, path);

            StartStep();
        }
    }
    private void MainMenu_OnClick(MainMenu.ButtonType buttonType)
    {
        GameState state = null;
        switch (buttonType)
        {
            case MainMenu.ButtonType.New:
                NewGama();
                break;
            case MainMenu.ButtonType.Save:
                state = new GameState(debugGrid.GetCells(), scenceUI.Point.Value);
                GameSaver.Save(state);
                break;
            case MainMenu.ButtonType.Load:
                state = GameSaver.Load();

                if(state != null)
                {
                    stepData?.Cell.Highlight(false);
                    
                    scenceUI.Point.Value = state.Points;
                    debugGrid.SetState(state);
                    scenceUI.MainMenu.Active = false;
                }
                else
                {

                }
                break;
            case MainMenu.ButtonType.Exit:
                Application.Quit();
                break;
        }
    }

    private void NewGama()
    {
        debugGrid.Clear();

        debugGrid.Generate(3);

        scenceUI.Point.Value = 0;

        scenceUI.MainMenu.Active = false;
    }

    private void SetActiveMenu(bool active)
    {
        scenceUI.MainMenu.Active = active;

        if (scenceUI.MainMenu.Active)
            scenceUI.MainMenu.EnableButton(MainMenu.ButtonType.Save, debugGrid.GetEmptyCoords().Count != 0);
    }

    void Start()
    {
        selector.OnSelected += Selector_OnSelected;

        scenceUI.MainMenu.OnClick += MainMenu_OnClick;

        NewGama();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SetActiveMenu(!scenceUI.MainMenu.Active);
        }
    }
}

public class StepData
{
    public Sphere Sphere 
    {
        get;
        private set;
    }

    public Cell Cell
    {
        get;
        private set;
    }

    public List<Vector3> Path
    {
        get;
        private set;
    }

    public StepData(Sphere sphere, Cell cell, List<Vector3> path)
    {
        Sphere = sphere;
        Cell = cell;
        Path = path;
    }
}