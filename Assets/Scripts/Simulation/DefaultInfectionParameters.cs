namespace Simulation
{
    public static class DefaultInfectionParameters
    {
        /// <summary>
        /// This class stores default infections values for the simulation.
        /// The average values are taken from:
        /// <see cref="https://www.rki.de/DE/Content/InfAZ/N/Neuartiges_Coronavirus/Modellierung_Deutschland.pdf;jsessionid=6BEA5234400C372CBCA61A2F969FA97D.internet062?__blob=publicationFile"/>
        /// <see cref="https://www.sciencemediacenter.de/alle-angebote/fact-sheet/details/news/verlauf-von-covid-19-und-kritische-abschnitte-der-infektion/"/>
        /// 
        /// 
        /// The assumption is made, that EndDaySymptoms is >= endDayInfectious
        /// 
        /// -->   IncubationTime + AmountDaysSymptoms - 1  >=LatencyTime + AmountDaysInfectious - 1
        ///         IncubationTime + AmountDaysSymptoms   >=LatencyTime + AmountDaysInfectious 
        ///             
        ///          Must be verified !
        /// 
        /// We count the first day =1, second day = 2,...
        /// 
        /// </summary>
        public static class InfectionsPhaseParameters
        {

            /// <summary>
            /// Amount days until a person becomes infectious.
            /// </summary>
            public const int LatencyTime = 3;

            /// <summary>
            /// Amount days a person IS infectious.
            /// </summary>
            public const int AmountDaysInfectious = 10;

            /// <summary>
            /// The last day a person is infectious.
            /// </summary>
            public const int EndDayInfectious = LatencyTime + AmountDaysInfectious - 1;

            //Incubation Time, whether use the median or generate a value

            /// <summary>
            /// Amount days until a person gets symptoms.
            /// </summary>
            public const int IncubationTime = 5;

            /// <summary>
            /// Amount days a person HAS symptoms. (If person must not go to an hospital)
            /// We use the assumption that most persons will be recovered when there are no symptoms.
            /// </summary>
            public const int AmountDaysSymptoms = 9;

            /// <summary>
            /// The last day a person has symptoms.
            /// </summary>
            public const int EndDaySymptoms = IncubationTime + AmountDaysSymptoms - 1;


        }

        /// <summary>
        /// TODO USE IN HOSPITAL
        /// </summary>
        public static class HealthPhaseParameters
        {

            /// <summary>
            /// Probability that an infected person recovers,
            /// else person must go to hospital !
            /// </summary>
            public const float RecoveringProbability = 0.955f;
            /// <summary>
            /// Probability that a person does recover in hospital,
            /// else person must go to intensive care.
            /// </summary>
            public const float RecoveringInHospitalProbability = 0.75f;

            /// <summary>
            /// Person dies, if he/she does not survive in the intensive care.
            /// </summary>
            public const float PersonSurvivesIntensiveCareProbability = 0.5f;

            /// <summary>
            /// The amount days from the beginning of the symtoms
            /// to the death say.
            /// 
            /// "Zeit von Symptombeginn bis zum Tod
            ///In einer multinationalen Fallserie wird die mittlere Dauer(Median) von Symptombeginn bis zum Tod mit 18 Tagen(176) 
            ///und in einer Uebersichtsarbeit mit 16 Tagen angeben(177). Waehrend der ersten COVID-19-Welle in Deutschland betrug 
            ///diese Zeitspanne im Mittel(Median) 11 Tage(169).""
            ///<see cref="https://www.rki.de/DE/Content/InfAZ/N/Neuartiges_Coronavirus/Steckbrief.html"/>
            /// We use the average of the three above values.
            /// </summary>
            public const int DaysFromSymptomsBeginToDeath = 15;


        }

    }
}