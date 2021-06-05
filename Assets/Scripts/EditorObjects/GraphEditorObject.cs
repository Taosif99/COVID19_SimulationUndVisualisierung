using Simulation.Edit;
using UnityEngine;
namespace EditorObjects
{
    
    public class GraphEditorObject : IEditorObject
    {
        public GameObject EditorGameObject { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Entity EditorEntity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Vector3Int GridPosition { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string UIName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    }
}