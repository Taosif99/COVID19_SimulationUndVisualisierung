
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

    }
}