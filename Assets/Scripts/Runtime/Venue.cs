﻿using System;
using System.Collections.Generic;

namespace Assets.Scripts.Runtime
{
    class Venue : Entity
    {
        private float _infectionRisk;
        private HashSet<Person> _currentPeopleAtVenue = new HashSet<Person>();

        public Venue(float infectionRisk)
        {
            _infectionRisk = infectionRisk;
        }

        public void SimulateEncounters()
        {
            throw new NotImplementedException();
        }
    }
}
