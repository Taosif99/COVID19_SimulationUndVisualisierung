using System.Collections.Generic;

namespace Simulation.Runtime
{
    class Hospital : Workplace
    {
        
        private HashSet<Person> _patientsInRegularBeds;
        private HashSet<Person> _patientsInIntensiveCareBeds;

        public Hospital(Edit.Hospital editorEntity) : base(editorEntity)
        {

            AmountRegularBeds = editorEntity.AmountRegularBeds;
            AmountIntensiveCareBeds = editorEntity.AmountIntensiveCareBeds;
            _patientsInRegularBeds = new HashSet<Person>();
            _patientsInIntensiveCareBeds = new HashSet<Person>();
        }

        
        public HashSet<Person> PatientsInRegularBeds { get => _patientsInRegularBeds; set => _patientsInRegularBeds = value; }
        public int AmountRegularBeds { get; set; }
        public int AmountIntensiveCareBeds { get ; set ; }

        public int AmountPeopleInRegularBeds { get { return _patientsInRegularBeds.Count; } }
        public int AmountPeopleInIntensiveBeds { get { return _patientsInIntensiveCareBeds.Count; } }

        public HashSet<Person> PatientsInIntensiveCareBeds { get => _patientsInIntensiveCareBeds; set => _patientsInIntensiveCareBeds = value; }
    }
}
