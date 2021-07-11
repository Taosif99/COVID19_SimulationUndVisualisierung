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
        /// Infefection risk:
        /// <see cref="https://academic.oup.com/cid/advance-article/doi/10.1093/cid/ciaa1846/6033728"/> 
        /// 
        /// The assumption is made, that EndDaySymptoms is >= endDayInfectious
        /// 
        /// -->   IncubationTime + AmountDaysSymptoms - 1  >=LatencyTime + AmountDaysInfectious - 1
        ///         IncubationTime + AmountDaysSymptoms   >=LatencyTime + AmountDaysInfectious 
        ///             
        ///         EndDaySymptoms >= EndDayInfectious
        ///          Must be verified !
        /// 
        /// 
        /// 
        /// </summary>
        public static class InfectionPhaseParameters
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
        /// This class stores default quarantine values for the simulation.
        /// <see cref="https://www.rki.de/DE/Content/InfAZ/N/Neuartiges_Coronavirus/Kontaktperson/Management.html;jsessionid=CE16F99FB237ECBAC11C55AB8020F12F.internet111?nn=13490888#a325"
        /// </summary>
        public static class QuarantineParameters
        {
            /// <summary>
            /// Amount days a person must be in quarantine.
            /// </summary>
            public const int QuarantineDays = 14;

            /// <summary>
            /// Amount extended quarantine days in case the person is still infected after quarantine.
            /// </summary>
            public const int AdvancedQuarantineDays = 7;
        }

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
            /// The amount days from the beginning of the symptoms
            /// to the death day.
            /// 
            /// "Zeit von Symptombeginn bis zum Tod
            ///In einer multinationalen Fallserie wird die mittlere Dauer(Median) von Symptombeginn bis zum Tod mit 18 Tagen(176) 
            ///und in einer Uebersichtsarbeit mit 16 Tagen angeben(177). Waehrend der ersten COVID-19-Welle in Deutschland betrug 
            ///diese Zeitspanne im Mittel(Median) 11 Tage(169).""
            ///<see cref="https://www.rki.de/DE/Content/InfAZ/N/Neuartiges_Coronavirus/Steckbrief.html"/>
            /// We use the average of the three above values. (16 + 18 + 11) / 3 = 15
            /// </summary>
            public const int DaysFromSymptomsBeginToDeath = 15;


            public const int DeathDay = InfectionPhaseParameters.IncubationTime + DaysFromSymptomsBeginToDeath - 1;


            //Days in intensive care must be a subset of days in hospital (hospitalization) !!! (Our Assumption)

            //Hospital

            /// <summary>
            /// Amount days a person must stay in the hospital, if he/she
            /// is there because of CoViD.
            /// </summary>
            public const int DaysInHospital = 14;



            /// <summary>
            /// Amount days from symptoms beginn to transferring a person to a hospital
            /// </summary>
            public const int DurationOfSymtombeginToHospitalization = 4;

            /// <summary>
            /// The day a person must go to the hospital.  (Must be smaller than end day symptoms ! and greater  incubation time)
            /// </summary>
            public const int DayAPersonMustGoToHospital = InfectionPhaseParameters.IncubationTime + DurationOfSymtombeginToHospitalization - 1;

            /// <summary>
            /// The first day a person can leave the hospital
            /// </summary>
            public const int DayAPersonCanLeaveTheHospital = DayAPersonMustGoToHospital + DaysInHospital;


            //Intensive Care
            /// <summary>
            /// Amount days a person must must stay in intensive care.
            /// </summary>
            public const int DaysInIntensiveCare = 10;

            /// <summary>
            /// The amount days which have to pass from hospitalization to intensive care.
            /// </summary>
            public const int DurationOfHospitalizationToIntensiveCare = 1;

            /// <summary>
            /// The day a person must go to the intensive care. (must be greater day a person must go to hospital and smaller day a person can leave the hospital)
            /// </summary>
            public const int DayAPersonMustGoToIntensiveCare = DayAPersonMustGoToHospital + DurationOfHospitalizationToIntensiveCare;

            /// <summary>
            /// The day a person can leave the intensive care. (must be smaller a person can leave the hospital)
            /// </summary>
            public const int DayAPersonCanLeaveIntensiveCare = DayAPersonMustGoToIntensiveCare + DaysInIntensiveCare;


            /// <summary>
            /// The Probability that a recovered person will be infected again.
            /// 
            /// "Basierend auf ihren Daten schaetzten die Autorinnen und Autoren das Risiko, 
            /// sich nach einer ersten Infektion erneut anzustecken, auf 0,02 Prozent." (Study which has been made in Qatar)
            /// <see cref="https://www.mdr.de/wissen/mensch-alltag/corona-zweite-infektion-risiko100.html"/>
            /// 
            /// </summary>
            public const float InfectionRiskIfRecovered = 0.0002f;
        }


        /// <summary>
        /// 
        /// We use 1-ProtectionValue, as Factor for our infection calculation
        /// 
        /// <see cref="https://www.br.de/nachrichten/wissen/masken-gegen-corona-welche-am-besten-schuetzen-kunststoff-stoff,S7XsGu7"/>
        /// </summary>
        public static class MaskFactors 
        {
            public const float FaricMaskProtectionFactor = 0.25f;
            public const float MedicalMaskProtectionFactor = 0.1f;
            public const float FFP2MedicalProtectionFactor = 0.06f;
        }



        /// <summary>
        /// We use the same assumption, as the el pais article, that the 
        /// probability of infection at any time is 3.8%.
        /// 
        /// 
        /// 
        /// <see cref="https://english.elpais.com/usa/2021-04-21/covid-19-vaccines-what-are-the-risks-and-benefits-for-each-age-group.html"/>
        /// </summary>
        public const float ProbabilityOfInfection = 0.038f;

    }
}