using Simulation.Edit;
using UnityEngine;

namespace EditorObjects
{
    
    public class VenueEditorObject : IEditorObject
    {

        public GameObject EditorGameObject { get; set; }
        public Vector3Int GridPosition { get; set; }
        public string UIName { get; set; }
        public Entity EditorEntity { get; set; }

        public VenueEditorObject(GameObject gameObject, Venue editorVenue, Vector3Int gridPosition, string name)
        {
            EditorGameObject = gameObject;
            EditorEntity = editorVenue;
            GridPosition = gridPosition;
            UIName = name;
        }


    }

}