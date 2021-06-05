using Simulation.Edit;
using UnityEngine;

namespace EditorObjects
{
    
    public class VenueEditorObject : IEditorObject
    {

        public GameObject EditorGameObject { get; set; }
        public string UIName { get; set; }
        public Entity EditorEntity { get; set; }

        public VenueEditorObject(GameObject gameObject, Venue editorVenue, string name)
        {
            EditorGameObject = gameObject;
            EditorEntity = editorVenue;
            UIName = name;
        }


    }

}