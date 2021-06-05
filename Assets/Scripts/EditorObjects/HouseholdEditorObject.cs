using Simulation.Edit;
using UnityEngine;

namespace EditorObjects
{
 
    public class HouseholdEditorObject : IEditorObject
    {



        public GameObject EditorGameObject { get; set; }
        public string UIName { get; set; }
        public Entity EditorEntity { get; set; }

        public HouseholdEditorObject(GameObject gameObject, Household editorHousehold, string name)
        {
            EditorGameObject = gameObject;
            EditorEntity = editorHousehold;
            UIName = name;
        }

    }
}