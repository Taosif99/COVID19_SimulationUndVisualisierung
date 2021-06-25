
public static class DefaultInfectionParameters 
{
    /// <summary>
    /// The average values are taken from:
    /// <see cref="https://www.rki.de/DE/Content/InfAZ/N/Neuartiges_Coronavirus/Modellierung_Deutschland.pdf;jsessionid=6BEA5234400C372CBCA61A2F969FA97D.internet062?__blob=publicationFile"/>
    /// <see cref="https://www.sciencemediacenter.de/alle-angebote/fact-sheet/details/news/verlauf-von-covid-19-und-kritische-abschnitte-der-infektion/"/>
    /// </summary>
    public static class InfectionsPhaseParameters
    {

        //Following values are the averages of the RKI

        /// <summary>
        /// Amount days until a person becomes infectious.
        /// </summary>
        public const int LatencyTime = 3;

        /// <summary>
        /// Amount days a person IS infectious.
        /// </summary>
        public const int AmountDaysInfectious = 10;

        public const int EndDayInfectious = LatencyTime + AmountDaysInfectious - 1;

        //Incubation Time, whether use the median or generate a value

        /// <summary>
        /// Amount days until persons gets symptoms.
        /// </summary>
        public const int IncubationTime = 5;

        /// <summary>
        /// Amount days person HAS symptoms. (If person must not go to an hospital)
        /// </summary>
        public const int AmountDaysSymptoms = 9;

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


    }

}
