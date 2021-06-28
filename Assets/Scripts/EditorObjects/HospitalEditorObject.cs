using UnityEngine;
using Simulation.Edit;


namespace EditorObjects
{
   
    public class HospitalEditorObject : IEditorObject
    {


        public GameObject EditorGameObject { get; set; } 
        public Entity EditorEntity { get; set; }
        public HospitalEditorObject(GameObject gameObject, Hospital editorHospital)
        {
            EditorGameObject = gameObject;
            EditorEntity = editorHospital;
        }

    }
}