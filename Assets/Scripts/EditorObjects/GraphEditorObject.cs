using Simulation.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EditorObjects
{
    
    public class GraphEditorObject : IEditorObject
    {
        public GameObject EditorGameObject { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Entity RuntimeEntity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Vector3Int RelativePosition { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string UIName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void LoadUI()
        {
            throw new NotImplementedException();
        }
    }
}