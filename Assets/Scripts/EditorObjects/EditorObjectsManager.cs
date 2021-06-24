using System.Collections.Generic;
using UnityEngine;
using Grid;
using Simulation.Edit;
using System.Linq;
using System;
using Simulation;
using InputValidation;


namespace EditorObjects
{

    /// <summary>
    /// This class maintains the editor Objects during the Runtime
    ///
    /// TODO: Remove UI Name eventually
    /// 
    /// Currently working with mock editor objects
    /// </summary>
    public class EditorObjectsManager : MonoBehaviour
    {

        [SerializeField] private GridManager _gridManager;
        //We use the unique GridCell Position as Key
        private Dictionary<GridCell, IEditorObject> _editorObjectsDic = new Dictionary<GridCell, IEditorObject>();
        private Entity _currentSelectedEntity;


        //private HashSet<string> _usedUiNames = new HashSet<string>(); //TODO DIALOG BOX WHEN NAME USED TWICE
        //private string _currentEditorObjectUIName = "";

        //counters for unique mock naming
        private int _workPlaceCounter = 1;
        private int _hospitalCounter = 1;
        private int _householdCounter = 1;
        private int _amountPeople = 0;

        public Entity CurrentSelectedEntity { get => _currentSelectedEntity; set => _currentSelectedEntity = value; }
        public int WorkPlaceCounter { get => _workPlaceCounter; set => _workPlaceCounter = value; }
        public int HospitalCounter { get => _hospitalCounter; set => _hospitalCounter = value; }
        public int HouseholdCounter { get => _householdCounter; set => _householdCounter = value; }
        public Dictionary<GridCell, IEditorObject> EditorObjectsDic { get => _editorObjectsDic; set => _editorObjectsDic = value; }
        public int AmountPeople { get => _amountPeople; set => _amountPeople = value; }

        /// <summary>
        /// Method to add an EditorObject to our system.
        /// </summary>
        /// <param name="gridCellPosition"></param>
        /// <returns>The created GameObject which can be place in the scene.</returns>
        public GameObject AddEditorObject(PrefabName prefabName, Vector2Int gridCellPosition)
        {
            IEditorObject editorObject = null;
            GridCell gridCell = new GridCell(gridCellPosition.x, gridCellPosition.y);
            string uiName = "";
            //TODO DEFINE LOGICAL DEFAULT CONSTRUCTOR FOR ENTITES / OR DEFAULT VALUES IN GENERAL
            switch (prefabName)
            {
                case PrefabName.Workplace:
                    uiName = "Workplace Mock " + WorkPlaceCounter++;
                    Workplace workplace = new Workplace(gridCell, 0.2f, WorkplaceType.Other, 200);
                    editorObject = EditorObjectFactory.Create(workplace, uiName);
                    break;
                case PrefabName.Hospital:
                    uiName = "Hospital Mock " + HospitalCounter++;
                    Hospital hospital = new Hospital(gridCell, 0.1f, WorkplaceType.Hospital, 299, HospitalScale.Large, WorkerAvailability.Low);
                    editorObject = EditorObjectFactory.Create(hospital, uiName);
                    break;
                case PrefabName.Household:
                    uiName = "Household Mock " + HouseholdCounter++;
                    Household household = new Household(gridCell, 0.6f, 12, 0.7f, 0.5f, 0.3f, 2, 5);
                    editorObject = EditorObjectFactory.Create(household, uiName);
                    break;
                default:
                    Debug.LogError("Unknown prefab name");
                    break;
            }
            // _usedUiNames.Add(uiName);
    
            AddEditorObjectToCollection(gridCell, editorObject);
            LoadEditorObjectUI(gridCellPosition);
            return editorObject.EditorGameObject;
        }



        /// <summary>
        /// Method which loads Values from the UI according the correspoding Clicked Venue Object Object
        /// </summary>
        /// <param name="spawnPosition"></param>
        public void LoadEditorObjectUI(Vector2Int gridCellPosition)
        {
            UIController.Instance.SetEntityPropertiesVisible(true);
            
            GridCell gridCell = new GridCell(gridCellPosition.x, gridCellPosition.y);
            IEditorObject editorObject = _editorObjectsDic[gridCell];
            
            if (editorObject == null)
            {
                return;
            }

            CurrentSelectedEntity = editorObject.EditorEntity;
                
            UIController.Instance.IsEntitySelectedUI(true);
            UIController.Instance.ObjectNameInputField.text = editorObject.UIName;
                
            // _currentEditorObjectUIName = editorObject.UIName;
            //Check if Graph....TODO WHEN GRAPH IS UI ELEMENT IN WORLD
            if (!(CurrentSelectedEntity is Venue venue))
            {
                return;
            }

            UIController.Instance.InfectionRiskInputField.text = venue.InfectionRisk.ToString(); //TODO ROUND VALUES
            
            switch (CurrentSelectedEntity)
            {
                case Workplace workplace:
                {
                    UIController.Instance.LoadWorkplaceUI();

                    UIController.Instance.WorkerCapacityInputField.text = workplace.WorkerCapacity.ToString();

                    if (workplace is Hospital hospital)
                    {
                        UIController.Instance.LoadHospitalUI();
                        List<string> availableHospitalScaleOptions = UIController.Instance.HospitalScaleDropdown.options.Select(option => option.text).ToList();
                        List<string> availableWorkerAvailabilityOptions = UIController.Instance.WorkerAvailabilityDropdown.options.Select(option => option.text).ToList();
                        UIController.Instance.HospitalScaleDropdown.value = availableHospitalScaleOptions.IndexOf(hospital.Scale.ToString());
                        UIController.Instance.WorkerAvailabilityDropdown.value = availableWorkerAvailabilityOptions.IndexOf(hospital.WorkerAvailability.ToString());
                    }
                    else
                    {
                        List<string> availableWorkplaceOptions = UIController.Instance.WorkplaceTypeDropdown.options.Select(option => option.text).ToList();
                        UIController.Instance.WorkplaceTypeDropdown.value = availableWorkplaceOptions.IndexOf(workplace.Type.ToString());
                    }

                    break;
                }
                
                case Household household:
                {
                    UIController.Instance.LoadHouseholdUI();
                    UIController.Instance.NumberOfPeopleInputField.text = household.NumberOfPeople.ToString();
                    UIController.Instance.CarefulnessInputField.text = household.CarefulnessTendency.ToString();
                    UIController.Instance.PercantageOfWorkersInputField.text = household.PercentageOfWorkers.ToString();
                    break;
                }
            }
        }


        //TODO UI Name must be unique !
        /// <summary>
        /// 
        /// Method which saves changes  to editor
        /// objects. The enums are parsed with the Enum.Parse()
        /// method which may cause an exception if something goes wrong.
        /// 
        /// TODO Load save values to runtime entity
        /// Enum parsing looks ugly
        /// </summary>
        public void SaveToEntity()
        {
            float infectionRisk = 0f;
            int capacity = 0;
            byte numberOfPeople = 0;
            float carefulness = 0f;
            float percentageOfWorkers = 0f;

            if (CurrentSelectedEntity != null)
            {

                bool inputIsOkay = InputValidator.TryParseLeftInputFields(ref infectionRisk,
                ref capacity,
                ref numberOfPeople,
                ref carefulness,
                ref percentageOfWorkers,
                CurrentSelectedEntity);
                if (inputIsOkay)
                {

                    IEditorObject editorObject = _editorObjectsDic[CurrentSelectedEntity.Position];
                    if (editorObject != null)
                    {
                        //Remove old and add new one
                        editorObject.UIName = UIController.Instance.ObjectNameInputField.text;
                        //_usedUiNames.Remove(_currentEditorObjectUIName);
                        //_usedUiNames.Add(editorObject.UIName);
                        //UIController.Instance.ObjectNameInputField.text = editorObject.UIName;
                        
                        if (CurrentSelectedEntity is Venue)
                        {
                            Venue venue = (Venue)CurrentSelectedEntity;
                            venue.InfectionRisk = infectionRisk;
                            if (CurrentSelectedEntity is Workplace)
                            {
                                Workplace workplace = (Workplace)CurrentSelectedEntity;

                                if (!(CurrentSelectedEntity is Hospital))
                                {
                                    WorkplaceType workplaceType = (WorkplaceType)Enum.Parse(typeof(WorkplaceType), UIController.Instance.WorkplaceTypeDropdown.options[UIController.Instance.WorkplaceTypeDropdown.value].text);
                                    workplace.WorkerCapacity = capacity;
                                    workplace.Type = workplaceType;
                                }

                                if (CurrentSelectedEntity is Hospital)
                                {
                                    Hospital hospital = (Hospital)CurrentSelectedEntity;
                                    hospital.Type = WorkplaceType.Hospital;
                                    HospitalScale hospitalScale = (HospitalScale)Enum.Parse(typeof(HospitalScale), UIController.Instance.HospitalScaleDropdown.options[UIController.Instance.HospitalScaleDropdown.value].text);
                                    WorkerAvailability workerAvailability = (WorkerAvailability)Enum.Parse(typeof(WorkerAvailability), UIController.Instance.WorkerAvailabilityDropdown.options[UIController.Instance.WorkerAvailabilityDropdown.value].text);
                                    hospital.Scale = hospitalScale;
                                    hospital.WorkerAvailability = workerAvailability;
                                }

                            }
                            else if (CurrentSelectedEntity is Household)
                            {
                                Household household = (Household)CurrentSelectedEntity;
                                _amountPeople -= household.NumberOfPeople;
                                _amountPeople += numberOfPeople;
                                household.NumberOfPeople = numberOfPeople;
                                household.CarefulnessTendency = carefulness;
                                household.PercentageOfWorkers = percentageOfWorkers;
                            }
                        }

                    }
                }


            }
        }

        /// <summary>
        /// Method which deletes the current selected entity.
        /// </summary>
        public void DeleteCurrentEntity()
        {
            if (CurrentSelectedEntity != null)
            {
                IEditorObject editorObject = _editorObjectsDic[CurrentSelectedEntity.Position];
                if (editorObject != null)
                {
                    //Destroy the gameObject in the scene
                    GameObject gameObject = editorObject.EditorGameObject;
                    Destroy(gameObject);
                    //_usedUiNames.Remove(editorObject.UIName);
                    _editorObjectsDic.Remove(CurrentSelectedEntity.Position);
                    CurrentSelectedEntity = null;
                    UIController.Instance.IsEntitySelectedUI(false);
                }
            }
        }

        /// <summary>
        /// Get all editor objects which are currently placed.
        /// </summary>
        /// <returns>A list of all currently placed editor objects</returns>
        public List<IEditorObject> GetAllEditorObjects()
        {
            return _editorObjectsDic.Values.ToList();
        }

        /// <summary>
        /// Reloads all EditorObjects and Positions on the Grid.
        /// </summary>
        public void ReloadEditorObjects()
        {
            IEditorObject editorObject;
            IEditorObject reloadedEditorObject;
            GridCell position;
            Dictionary<GridCell, IEditorObject> newEditorObjects = new Dictionary<GridCell, IEditorObject>();

            _gridManager.Reset();

            foreach (KeyValuePair<GridCell, IEditorObject> pair in _editorObjects)
            {
                editorObject = pair.Value;
                position = pair.Key;

                Destroy(editorObject.EditorGameObject);

                reloadedEditorObject = EditorObjectFactory.Create(editorObject.EditorEntity, editorObject.UIName);
                _gridManager.PositionObjectInGrid(reloadedEditorObject.EditorGameObject, position.ToVector2Int());

                newEditorObjects[position] = reloadedEditorObject;
            }

            _editorObjects = newEditorObjects;
        }
        
        /// <summary>
        /// Method to add an editor object to the internal used collection which also counts
        /// internal simulation properties like the amount of people at the beginning.
        /// </summary>
        /// <param name="key">The unique gridcell position of an object in our world.</param>
        /// <param name="editorObject">The corresponding editorobject which holds the entity.</param>
        public void AddEditorObjectToCollection(GridCell key, IEditorObject editorObject)
        {

           _editorObjectsDic.Add(key,editorObject);

            Entity entity = editorObject.EditorEntity;
            if (entity is Household) 
            {
                Household household = (Household)entity;
                _amountPeople += household.NumberOfPeople;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            _gridManager.OnEditorObjectClicked += LoadEditorObjectUI;
        }
    }
}