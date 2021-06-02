using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation.Runtime;
using System;

namespace EditorObjects
{
    public class HospitalEditorObject : IEditorObject
    {
        private GameObject _gameObject;
        private Hospital _runtimeHospital;
        private Vector3Int _relativePosition;
        private string _name;

        public GameObject EditorGameObject { get => _gameObject; set => _gameObject = value; }
        public Vector3Int RelativePosition { get => _relativePosition; set => _relativePosition = value; }
        public string UIName { get => _name; set => _name = value; }
        public Entity RuntimeEntity { get => _runtimeHospital; set => _runtimeHospital = (Hospital)value; }

        public HospitalEditorObject(GameObject gameObject, Hospital runtimeHospital, Vector3Int relativePosition, string name)
        {
            _gameObject = gameObject;
            _runtimeHospital = runtimeHospital;
            _relativePosition = relativePosition;
            _name = name;
        }

        public void LoadUI()
        {
            throw new NotImplementedException();
        }
    }
}