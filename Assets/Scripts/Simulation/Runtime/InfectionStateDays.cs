using System;
namespace Simulation.Runtime
{
    public static class InfectionStateDays
    {
        /*Die Inkubationszeit gibt die Zeit von der Ansteckung bis zum Beginn der Erkrankung an. 
         * Die mittlere Inkubationszeit (Median) wird in den meisten Studien mit 5-6 Tagen angegeben. 
         * In verschiedenen Studien wurde berechnet, zu welchem Zeitpunkt 95% der Infizierten Symptome entwickelt hatten, dabei lag das 95. Perzentil der Inkubationszeit bei 10-14 Tagen.
         * Quelle: https://www.rki.de/DE/Content/InfAZ/N/Neuartiges_Coronavirus/Steckbrief.html;jsessionid=4F9C8FBD1A6B20D6B25BDFC1EBCC726B.internet082?nn=13490888#doc13776792bodyText5 (aufgerufen 05.06.2021)
        */
        public const int IncubationMinDay = 5;
        public const int IncubationMaxDay = 6;
        public const int SymptomsMinDay = 10;
        public const int SymptomsMaxDay = 14;

        /*
         * c) Zeitspanne der Infektiosität
         * Zwei Studien geben Startpunkt der Infektiosität mit circa 2,5 Tagen vor Symptombeginn an an, mit einem Maximum an Viruslast also Infektiosität 0,6 Tage vor Symptombeginn
         * 
         * https://www.sciencemediacenter.de/alle-angebote/fact-sheet/details/news/verlauf-von-covid-19-und-kritische-abschnitte-der-infektion/ (aufgerufen 05.06.2021)
         * 
         */
        public const int InfectiousMinDay = 7;
        public const int InfectiousMaxDay = 11;

        /*
         * f) Zeit von ersten Symptomen bis zur Erholung milder Fälle
         * Nach 14 Tagen ab Symptombeginn kann die häusliche Quarantäne beendet werden, wenn der Patient für 48 Stunden symptomfrei war 
         * 
         * https://www.sciencemediacenter.de/alle-angebote/fact-sheet/details/news/verlauf-von-covid-19-und-kritische-abschnitte-der-infektion/ (aufgerufen 05.06.2021)
        */
        public const int RecoveringMinDay = 24;
        public const int RecoveringMaxDay = 28;


    }
}