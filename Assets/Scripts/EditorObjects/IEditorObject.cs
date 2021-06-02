
using UnityEngine;
using Simulation.Runtime;

namespace EditorObjects
{
    /// <summary>
    /// This Interface shall define all Methods/Properties an EditoObject must fulfill.
    /// </summary>
    ///

    public interface IEditorObject
    {
        //The GameObject of the Editor object
        GameObject EditorGameObject { get; set; }

        //Reference to the runtime Entity
        Entity RuntimeEntity { get; set; }

        //Saving the world position
        //Vector3 WorldPosition { get; set; }

        //Saving the position on the Grid
        Vector3Int RelativePosition { get; set; }

        //Unique name of the object, not the prefab type name of the game object. Must be set in the scene.
        string UIName { get; set; }

        //Methods convcerning loading UI info can be loaded here
        void LoadUI();
    }
}