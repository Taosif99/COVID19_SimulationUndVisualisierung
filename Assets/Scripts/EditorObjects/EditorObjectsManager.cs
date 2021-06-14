using System.Collections.Generic;
using UnityEngine;
using Grid;
using Simulation.Edit;
using System.Linq;
using System;
using Simulation;
using InputValidation;
using TMPro;

namespace EditorObjects
{

    /// <summary>
    /// This class maintains the editor Objects during the Runtime
    /// I think it would be better to implement a "gameMaster" class to remove Monobehaviour inheritance
    /// 
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

        public Entity CurrentSelectedEntity { get => _currentSelectedEntity; set => _currentSelectedEntity = value; }
        public int WorkPlaceCounter { get => _workPlaceCounter; set => _workPlaceCounter = value; }
        public int HospitalCounter { get => _hospitalCounter; set => _hospitalCounter = value; }
        public int HouseholdCounter { get => _householdCounter; set => _householdCounter = value; }
        public Dictionary<GridCell, IEditorObject> EditorObjectsDic { get => _editorObjectsDic; set => _editorObjectsDic = value; }



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
            EditorObjectsDic.Add(gridCell, editorObject);
            LoadEditorObjectUI(gridCellPosition);
            return editorObject.EditorGameObject;
        }



        /// <summary>
        /// Method which loads Values from the UI according the correspoding Clicked Venue Object Object
        /// </summary>
        /// <param name="spawnPosition"></param>
        public void LoadEditorObjectUI(Vector2Int gridCellPosition)
        {

            GridCell gridCell = new GridCell(gridCellPosition.x, gridCellPosition.y);
            IEditorObject editorObject = EditorObjectsDic[gridCell];
            if (editorObject != null)
            {

                UIController.Instance.IsEntitySelectedUI(true);
                UIController.Instance.ObjectNameInputField.text = editorObject.UIName;
                Debug.Log("Why did the name not change ??? " + editorObject.UIName);
                Entity entity = editorObject.EditorEntity;

                CurrentSelectedEntity = entity;
                // _currentEditorObjectUIName = editorObject.UIName;
                //Check if Graph....TODO WHEN GRAPH IS UI ELEMENT IN WORLD
                if (entity is Venue)
                {
                    Venue venue = (Venue)entity;
                    UIController.Instance.InfectionRiskInputField.text = venue.InfectionRisk.ToString(); //TODO ROUND VALUES
                    if (entity is Workplace)
                    {
                        UIController.Instance.LoadWorkplaceUI();
                        Workplace workplace = (Workplace)entity;


                        if (!(CurrentSelectedEntity is Hospital))
                        {
                            List<string> availableWorkplaceOptions = UIController.Instance.WorkplaceTypeDropdown.options.Select(option => option.text).ToList();
                            UIController.Instance.WorkplaceTypeDropdown.value = availableWorkplaceOptions.IndexOf(workplace.Type.ToString());
                        }
                        UIController.Instance.WorkerCapacityInputField.text = workplace.WorkerCapacity.ToString();

                        if (entity is Hospital)
                        {

                            UIController.Instance.LoadHospitalUI();
                            Hospital hospital = (Hospital)entity;
                            List<string> availableHospitalScaleOptions = UIController.Instance.HospitalScaleDropdown.options.Select(option => option.text).ToList();
                            List<string> availableWorkerAvailabilityOptions = UIController.Instance.WorkerAvailabilityDropdown.options.Select(option => option.text).ToList();
                            UIController.Instance.HospitalScaleDropdown.value = availableHospitalScaleOptions.IndexOf(hospital.Scale.ToString());
                            UIController.Instance.WorkerAvailabilityDropdown.value = availableWorkerAvailabilityOptions.IndexOf(hospital.WorkerAvailability.ToString());

                        }

                    }
                    else if (entity is Household)
                    {
                        UIController.Instance.LoadHouseholdUI();
                        Household household = (Household)entity;
                        UIController.Instance.NumberOfPeopleInputField.text = household.NumberOfPeople.ToString();
                        UIController.Instance.CarefulnessInputField.text = household.CarefulnessTendency.ToString();
                        UIController.Instance.PercantageOfWorkersInputField.text = household.PercentageOfWorkers.ToString();
                    }
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

                bool inputIsOkay = InputValidator.TryParseInputFields(ref infectionRisk,
                ref capacity,
                ref numberOfPeople,
                ref carefulness,
                ref percentageOfWorkers,
                CurrentSelectedEntity);
                if (inputIsOkay)
                {

                    IEditorObject editorObject = EditorObjectsDic[CurrentSelectedEntity.Position];
                    if (editorObject != null)
                    {
                        //Remove old and add new one
                        editorObject.UIName = UIController.Instance.ObjectNameInputField.text;
                        //_usedUiNames.Remove(_currentEditorObjectUIName);
                        //_usedUiNames.Add(editorObject.UIName);
                        //UIController.Instance.ObjectNameInputField.text = editorObject.UIName;
                        Debug.Log("Change editor name to  ###" + editorObject.UIName);
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
                IEditorObject editorObject = EditorObjectsDic[CurrentSelectedEntity.Position];
                if (editorObject != null)
                {
                    //Destroy the gameObject in the scene
                    GameObject gameObject = editorObject.EditorGameObject;
                    Destroy(gameObject);
                    //_usedUiNames.Remove(editorObject.UIName);
                    EditorObjectsDic.Remove(CurrentSelectedEntity.Position);
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
            return EditorObjectsDic.Values.ToList();
        }

        // Start is called before the first frame update
        void Start()
        {
            _gridManager.OnEditorObjectClicked += LoadEditorObjectUI;
        }
    }
}