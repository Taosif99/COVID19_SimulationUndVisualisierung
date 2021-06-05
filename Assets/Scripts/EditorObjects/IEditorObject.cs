
using UnityEngine;
using Simulation.Edit;

namespace EditorObjects
{
    /// <summary>
    /// This Interface shall define all Methods/Properties an EditorObject must fulfill.
    /// </summary>
    ///

    public interface IEditorObject
    {
        //The GameObject of the Editor object
        GameObject EditorGameObject { get; set; }

        //Reference to the editor Entity
        Entity EditorEntity { get; set; }

        //Saving the position on the Grid
        //Vector2Int GridPosition { get; set; }

        //Unique name of the object, not the prefab type name of the game object. Must be set in the scene.
        string UIName { get; set; }

    }
}