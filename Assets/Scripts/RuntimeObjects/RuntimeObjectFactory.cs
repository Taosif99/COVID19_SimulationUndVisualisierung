﻿using EditorObjects;
using Simulation.Edit;
using UnityEngine;

namespace RuntimeObjects
{
    /// <summary>
    /// This Interface shall define all Methods/Properties an RuntimeObject must fulfill.
    /// </summary>
    static class RuntimeObjectFactory
    {
        public static Simulation.Runtime.Graph CreateGraphRuntimeObject(GraphEditorObject graphEditorObject)
        {
            //The GameObject of the Editor object
            GameObject graphRuntimeGameObject = graphEditorObject.EditorGameObject;

            //We may add here the required monobehaviours
            //...gameObject.AddComponent

            Simulation.Runtime.Graph graph = new Simulation.Runtime.Graph(graphEditorObject.EditorEntity);

            return graph;
        }

        public static Simulation.Runtime.Hospital CreateHospitalRuntimeObject(HospitalEditorObject hospitalEditorObject)
        {
            //The GameObject of the Editor object
            GameObject hospitalRuntimeGameObject = hospitalEditorObject.EditorGameObject;

            //We may add here the required monobehaviours
            //...gameObject.AddComponent

            Simulation.Runtime.Hospital hospital = new Simulation.Runtime.Hospital((Hospital)hospitalEditorObject.EditorEntity);

            return hospital;
        }

        public static Simulation.Runtime.Household CreateHouseholdRuntimeObject(HouseholdEditorObject householdEditorObject)
        {
            //The GameObject of the Editor object
            GameObject householdRuntimeGameObject = householdEditorObject.EditorGameObject;

            //We may add here the required monobehaviours
            //...gameObject.AddComponent

            Simulation.Runtime.Household household = new Simulation.Runtime.Household((Household)householdEditorObject.EditorEntity);

            return household;
        }

        public static Simulation.Runtime.Workplace CreateWorkplaceRuntimeObject(WorkplaceEditorObject workplaceEditorObject)
        {
            //The GameObject of the Editor object
            GameObject workplaceRuntimeGameObject = workplaceEditorObject.EditorGameObject;

            //We may add here the required monobehaviours
            //...gameObject.AddComponent

            Simulation.Runtime.Workplace workplace = new Simulation.Runtime.Workplace((Workplace)workplaceEditorObject.EditorEntity);

            return workplace;
        }
    }
}
