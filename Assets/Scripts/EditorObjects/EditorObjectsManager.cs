using System.Collections.Generic;
using UnityEngine;
using EditorObjects;
using Grid;
using UnityEngine.UI;
using Simulation.Edit;
using System.Linq;
using System;
using Simulation;
using FileHandling;

namespace EditorObjects
{

    /// <summary>
    /// This class maintains the editor Objects during the Runtime
    /// I think it would be better to implement a "gameMaster" class to remove Monobehaviour inheritance
    /// 
    /// 
    /// Currently working wick mock editor objects
    /// </summary>
    public class EditorObjectsManager : MonoBehaviour
    {

        public GridManager SimulationGridManager;
        //We use the unique GridCell Position as Key
        private Dictionary<GridCell, IEditorObject> _editorObjectsDic = new Dictionary<GridCell, IEditorObject>();
        private Entity _currentSelectedEntity;



        #region outsource to UI controller
        //The settings objects can be outsourced in an  Venue properties UI class
        //public Text ObjectNameText;
        //Venue elements
        public InputField ObjectNameInputField;
        public InputField InfectionRiskInputField;
        //public InputField CurrentPeopleAtVenueInputField; //Use Text or label later, remove from ui
        //Household elements
        public InputField NumberOfPeopleInputField;
        public InputField PercantageOfWorkersInputField;
        public InputField CarefulnessInputField;
        //Workplace elements
        public Dropdown WorkplaceTypeDropdown;
        public InputField CapacityInputField;
        //Hospital elements
        public Dropdown HospitalScaleDropdown;
        public Dropdown WorkerAvailabilityDropdown;
        //private HashSet<string> _usedUiNames = new HashSet<string>(); //TODO DIALOG BOX WHEN NAME USED TWICE
        //private string _currentEditorObjectUIName = "";
        #endregion


        //counters for unique mock naming
        private int _workPlaceCounter = 1;
        private int _hospitalCounter = 1;
        private int _householdCounter = 1;



        /// <summary>
        /// Method to add an EditorObject to our system.
        /// </summary>
        /// <param name="gridCellPosition"></param>
        /// <returns>The created GameObject which can be place in the scene.</returns>
        public GameObject AddEditorObject(Vector2Int gridCellPosition)
        {

            PrefabName currentPrefabName = ModelSelector.Instance.CurrentPrefabName;
            IEditorObject editorObject = null;
            GridCell gridCell = new GridCell(gridCellPosition.x, gridCellPosition.y);
            string uiName = "";
            //TODO DEFINE LOGICAL DEFAULT CONSTRUCTOR FOR ENTITES / OR DEFAULT VALUES IN GENERAL
            switch (currentPrefabName)
            {
                case PrefabName.Workplace:
                    uiName = "Workplace Mock " + _workPlaceCounter++;
                    Workplace workplace = new Workplace(gridCell, 0.2f, WorkplaceType.Other, 200);
                    editorObject = EditorObjectFactory.Create(workplace, uiName);
                    break;
                case PrefabName.Hospital:
                    uiName = "Hospital Mock " + _hospitalCounter++;
                    Hospital hospital = new Hospital(gridCell, 0.1f, WorkplaceType.Hospital, 299, HospitalScale.Large, WorkerAvailability.Low);
                    editorObject = EditorObjectFactory.Create(hospital, uiName);
                    break;
                case PrefabName.Household:
                    uiName = "Household Mock " + _householdCounter++;
                    Household household = new Household(gridCell, 0.6f, 12, 0.7f, 0.5f, 0.3f, 2, 5);
                    editorObject = EditorObjectFactory.Create(household, uiName);
                    break;
                default:
                    Debug.LogError("Unknown prefab name");
                    break;
            }
            // _usedUiNames.Add(uiName);
            _editorObjectsDic.Add(gridCell, editorObject);
            LoadEditorObjectUI(gridCellPosition);
            return editorObject.EditorGameObject;
        }



        //We may handle different venues according the properties what can be set

        /// <summary>
        /// Method which loads Values from the UI according the correspoding Clicked Venue Object Object
        /// </summary>
        /// <param name="spawnPosition"></param>
        public void LoadEditorObjectUI(Vector2Int gridCellPosition)
        {

            GridCell gridCell = new GridCell(gridCellPosition.x, gridCellPosition.y);
            IEditorObject editorObject = _editorObjectsDic[gridCell];
            if (editorObject != null)
            {

                UIController.Instance.IsEntitySelectedUI(true);
                ObjectNameInputField.text = editorObject.UIName;

                Entity entity = editorObject.EditorEntity;

                _currentSelectedEntity = entity;
                // _currentEditorObjectUIName = editorObject.UIName;
                //Check if Graph....TODO WHEN GRAPH IS UI ELEMENT IN WORLD
                if (entity is Venue)
                {
                    Venue venue = (Venue)entity;
                    InfectionRiskInputField.text = venue.InfectionRisk.ToString(); //TODO ROUND VALUES
                    if (entity is Workplace)
                    {
                        UIController.Instance.LoadWorkplaceUI();
                        Workplace workplace = (Workplace)entity;
                        List<string> availableWorkplaceOptions = WorkplaceTypeDropdown.options.Select(option => option.text).ToList();
                        WorkplaceTypeDropdown.value = availableWorkplaceOptions.IndexOf(workplace.Type.ToString());
                        WorkplaceTypeDropdown.enabled = true;
                        CapacityInputField.text = workplace.WorkerCapacity.ToString();

                        if (entity is Hospital)
                        {
                            UIController.Instance.LoadHospitalUI();
                            Hospital hospital = (Hospital)entity;
                            List<string> availableHospitalScaleOptions = HospitalScaleDropdown.options.Select(option => option.text).ToList();
                            List<string> availableWorkerAvailabilityOptions = WorkerAvailabilityDropdown.options.Select(option => option.text).ToList();
                            HospitalScaleDropdown.value = availableHospitalScaleOptions.IndexOf(hospital.Scale.ToString());
                            WorkerAvailabilityDropdown.value = availableWorkerAvailabilityOptions.IndexOf(hospital.WorkerAvailability.ToString());

                            //At a hospital the dropdown of workplace type should not be changed !!!
                            WorkplaceTypeDropdown.enabled = false;

                        } //TODO MAKE SURE THAT HOSPITAL CANNOT BE SET

                    }
                    else if (entity is Household)
                    {
                        UIController.Instance.LoadHouseholdUI();
                        Household household = (Household)entity;
                        NumberOfPeopleInputField.text = household.NumberOfPeople.ToString();
                        CarefulnessInputField.text = household.CarefulnessTendency.ToString();
                        PercantageOfWorkersInputField.text = household.PercentageOfWorkers.ToString();
                    }
                }
            }
        }

        //TODO CATCH INPUT ERRORS, use tryparse...
        //TODO UI Name must be unique !
        /// <summary>
        /// 
        /// Method which saves changes (currently) to editor
        /// objects 
        /// 
        /// TODO Load save values to runtime entity
        /// Enum parsing looks ugly
        /// </summary>
        public void SaveToEntity()
        {

            if (_currentSelectedEntity != null)
            {
                IEditorObject editorObject = _editorObjectsDic[_currentSelectedEntity.Position];
                if (editorObject != null)
                {

                    //Remove old and add new one
                    editorObject.UIName = ObjectNameInputField.text;
                    //_usedUiNames.Remove(_currentEditorObjectUIName);
                    //_usedUiNames.Add(editorObject.UIName);
                    //_currentEditorObjectUIName = editorObject.UIName;
                    if (_currentSelectedEntity is Venue)
                    {

                        Venue venue = (Venue)_currentSelectedEntity;
                        venue.InfectionRisk = float.Parse(InfectionRiskInputField.text);
                        if (_currentSelectedEntity is Workplace)
                        {
                            Workplace workplace = (Workplace)_currentSelectedEntity;
                            WorkplaceType workplaceType = (WorkplaceType)Enum.Parse(typeof(WorkplaceType), WorkplaceTypeDropdown.options[WorkplaceTypeDropdown.value].text);
                            int capacity = int.Parse(CapacityInputField.text);
                            workplace.WorkerCapacity = capacity;
                            workplace.Type = workplaceType;

                            if (_currentSelectedEntity is Hospital)
                            {
                                Hospital hospital = (Hospital)_currentSelectedEntity;
                                HospitalScale hospitalScale = (HospitalScale)Enum.Parse(typeof(HospitalScale), HospitalScaleDropdown.options[HospitalScaleDropdown.value].text);
                                WorkerAvailability workerAvailability = (WorkerAvailability)Enum.Parse(typeof(WorkerAvailability), WorkerAvailabilityDropdown.options[WorkerAvailabilityDropdown.value].text);
                                hospital.Scale = hospitalScale;
                                hospital.WorkerAvailability = workerAvailability;
                            }

                        }
                        else if (_currentSelectedEntity is Household)
                        {
                            Household household = (Household)_currentSelectedEntity;
                            household.NumberOfPeople = byte.Parse(NumberOfPeopleInputField.text);
                            household.CarefulnessTendency = float.Parse(CarefulnessInputField.text);
                            household.PercentageOfWorkers = float.Parse(PercantageOfWorkersInputField.text);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Method which deletes the current selected entities
        /// </summary>
        public void DeleteCurrentEntity()
        {
            if (_currentSelectedEntity != null)
            {
                IEditorObject editorObject = _editorObjectsDic[_currentSelectedEntity.Position];
                if (editorObject != null)
                {
                    //Destroy the gameObject in the scene
                    GameObject gameObject = editorObject.EditorGameObject;
                    StateCounter counter = gameObject.GetComponent<StateCounter>();
                    GameObject counterGameObject = counter.CounterGameObject;
                    Destroy(counterGameObject);
                    Destroy(gameObject);
                    //_usedUiNames.Remove(editorObject.UIName);
                    _editorObjectsDic.Remove(_currentSelectedEntity.Position);
                    _currentSelectedEntity = null;
                    UIController.Instance.IsEntitySelectedUI(false);
                }
            }
        }

        /// <summary>
        /// Method which will be called if user selects in the previous scene to load a file.
        /// </summary>
        public void LoadFromFile()
        {

            //Clear old scene and load data
            foreach (IEditorObject editorObject in _editorObjectsDic.Values)
            {

                //TODO METHOD FOR THIS REPITITION
                GameObject gameObject = editorObject.EditorGameObject;
                StateCounter counter = gameObject.GetComponent<StateCounter>();
                GameObject counterGameObject = counter.CounterGameObject;
                Destroy(counterGameObject);
                Destroy(gameObject);
            }
            //reset naming counters 
            _workPlaceCounter = 1;
            _hospitalCounter = 1;
            _householdCounter = 1;
            _editorObjectsDic.Clear();
            _currentSelectedEntity = null;
            //Load Simulation
            Simulation.Edit.Simulation simulation = FileHandler.LoadData();
            if (simulation != null)
            {
                Entity[] entities = simulation.Entities;
                if (entities != null)
                {
                    foreach (Entity entity in entities)
                    {
                        if (entity is Workplace)
                        {
                            ModelSelector.Instance.SetCurrentPrefab(PrefabName.Workplace);

                            if (entity is Hospital)
                            {
                                ModelSelector.Instance.SetCurrentPrefab(PrefabName.Hospital);
                            }
                        }
                        else if (entity is Household)
                        {
                            ModelSelector.Instance.SetCurrentPrefab(PrefabName.Household);
                        }
                        IEditorObject editorObject = EditorObjectFactory.Create(entity, "serialized mock text");
                        _editorObjectsDic.Add(entity.Position, editorObject);
                        Vector2Int gridCellPosition = new Vector2Int(entity.Position.X, entity.Position.Y);
                        //Spawn in position-> spawn handler or manager
                        GameObject gameObject = editorObject.EditorGameObject;
                        SimulationGridManager.PositionObjectInGrid(gameObject, gridCellPosition);
                    }
                    ModelSelector.Instance.SetCurrentPrefab(PrefabName.None);
                    UIController.Instance.IsEntitySelectedUI(false);
                }
                else Debug.Log("No entities !");
            }
            else Debug.LogWarning("Something went wrong !");
        }
        /// <summary>
        /// Method which initiates the saving process of the program configuration.
        /// </summary>
        public void SaveToFile()
        {
            Entity[] entities = new Entity[_editorObjectsDic.Count];
            int index = 0;
            foreach (IEditorObject editorObject in _editorObjectsDic.Values)
            {
                Entity entity = editorObject.EditorEntity;
                entities[index] = entity;
                index++;
            }
            //TODO REPLACE MOCK WITH REAL SIMULATION
            Simulation.Edit.Simulation simulation = FileHandler.GetSimulationMock(entities);
            FileHandler.SaveData(simulation);
        }


        // Start is called before the first frame update
        void Start()
        {
            SimulationGridManager.OnEditorObjectClicked += LoadEditorObjectUI;
            //Getting the clicked simulation name if name exists
            string fileName = FileHandler.SelectedFileName;
            if (fileName != null)
            {
                Debug.Log("File to load: " + fileName);
                LoadFromFile();
            }
        }
    }
}