using Simulation.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorObjects
{
    public class HouseholdEditorObject : IEditorObject
    {
        private GameObject _gameObject;
        private Household _runtimeHousehold;
        private Vector3Int _relativePosition;
        private string _name;

        public GameObject EditorGameObject { get => _gameObject; set => _gameObject = value; }
        public Vector3Int RelativePosition { get => _relativePosition; set => _relativePosition = value; }
        public string UIName { get => _name; set => _name = value; }
        public Entity RuntimeEntity { get => _runtimeHousehold; set => _runtimeHousehold = (Household)value; }

        public HouseholdEditorObject(GameObject gameObject, Household runtimeHousehold, Vector3Int relativePosition, string name)
        {
            _gameObject = gameObject;
            _runtimeHousehold = runtimeHousehold;
            _relativePosition = relativePosition;
            _name = name;
        }

        public void LoadUI()
        {
            throw new NotImplementedException();
        }
    }
}