using Simulation.Edit;
using UnityEngine;
namespace EditorObjects
{
   
    public class WorkplaceEditorObject : IEditorObject
    {

        public GameObject EditorGameObject { get; set; }
        public Vector3Int GridPosition { get; set; }
        public string UIName { get; set; }
        public Entity EditorEntity { get; set; }

        public WorkplaceEditorObject(GameObject gameObject, Workplace editorWorkplace, Vector3Int gridPosition, string name)
        {
           EditorGameObject = gameObject;
           EditorEntity = editorWorkplace;
           GridPosition= gridPosition;
           UIName = name;
        }

    }
}