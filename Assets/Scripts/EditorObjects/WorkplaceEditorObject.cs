using Simulation.Edit;
using UnityEngine;
namespace EditorObjects
{
   
    public class WorkplaceEditorObject : IEditorObject
    {

        public GameObject EditorGameObject { get; set; }
        public Entity EditorEntity { get; set; }

        public WorkplaceEditorObject(GameObject gameObject, Workplace editorWorkplace)
        {
           EditorGameObject = gameObject;
           EditorEntity = editorWorkplace;
        }

    }
}