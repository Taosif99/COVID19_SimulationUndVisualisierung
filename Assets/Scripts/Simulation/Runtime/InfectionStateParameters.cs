using System;
namespace Simulation.Runtime
{
    public static class InfectionStateParameters
    {
        /// <summary>
        /// "Die Inkubationszeit gibt die Zeit von der Ansteckung bis zum Beginn der Erkrankung an. Die mittlere Inkubationszeit(Median) wird in den meisten Studien mit 5-6 Tagen angegeben.
        /// In verschiedenen Studien wurde berechnet, zu welchem Zeitpunkt 95% der Infizierten Symptome entwickelt hatten, dabei lag das 95. Perzentil der Inkubationszeit bei 10-14 Tagen."
        /// Source: <see href="https://www.rki.de/DE/Content/InfAZ/N/Neuartiges_Coronavirus/Steckbrief.html;jsessionid=4F9C8FBD1A6B20D6B25BDFC1EBCC726B.internet082?nn=13490888#doc13776792bodyText5">RKI - Incubation period and serial interval</see> (website called on 05.06.2021)
        /// </summary>
        public const int IncubationMinDay = 5;
        public const int IncubationMaxDay = 6;
        public const int SymptomsMinDay = 10;
        public const int SymptomsMaxDay = 14; //

        /// <summary>
        /// "Zwei Studien geben Startpunkt der Infektiosität mit circa 2,5 Tagen vor Symptombeginn an an, mit einem Maximum an Viruslast also Infektiosität 0,6 Tage vor Symptombeginn."
        /// Source: <see href="https://www.sciencemediacenter.de/alle-angebote/fact-sheet/details/news/verlauf-von-covid-19-und-kritische-abschnitte-der-infektion/">Sciencemediacente - Period of infectivity</see> (website called on 05.06.2021)
        /// </summary>
        public const int InfectiousMinDay = 7;
        public const int InfectiousMaxDay = 11;

        /// <summary>
        ///  "Nach 14 Tagen ab Symptombeginn kann die häusliche Quarantäne beendet werden, wenn der Patient für 48 Stunden symptomfrei war"
        ///  Source: <see href="https://www.sciencemediacenter.de/alle-angebote/fact-sheet/details/news/verlauf-von-covid-19-und-kritische-abschnitte-der-infektion/">Sciencemediacente -  Time from first symptoms to recovery of mild cases</see> (website called on 05.06.2021)
        /// </summary>
        public const int RecoveringMinDay = 24;
        public const int RecoveringMaxDay = 28; //

        /// <summary>
        /// Defines whether the person dies based on the physical state set.
        ///"Das Sterberisiko steigt bei den meisten Vorerkrankungen um bis zu 87 Prozent."
        /// Source: <see href="https://www.quarks.de/gesundheit/medizin/wie-viele-menschen-sterben-an-corona/">Quarks - Mortality risk for people with pre-existing conditions</see> (website called on 12.06.2021)
        /// * "Insgesamt sind 2,6% aller Personen, für die bestätigte SARS-CoV-2-Infektionen in Deutschland übermittelt wurden, im Zusammenhang mit einer COVID-19-Erkrankung verstorben."
        /// Source:  <see href="https://www.rki.de/DE/Content/InfAZ/N/Neuartiges_Coronavirus/Steckbrief.html;jsessionid=9380EA03621C1ECA856E7B2B4D5A9E4A.internet062?nn=13490888#doc13776792bodyText13">RKI - Case-fatality ratio, lethality</see> (website called on 12.06.2021)
        /// </summary>
        public const float FatalityRate = 0.026f;
        public const float FatalityRatePreIllness = 0.87f;

        /// <summary>
        /// "Etwa 36,5 Millionen Menschen in Deutschland haben danach ein erhöhtes Risiko für einen schweren COVID-19-Verlauf.Unter diesen gehören 21,6 Millionen Menschen zur Hochrisikogruppe."
        /// PreIllness Probability:
        /// 36.5 - 21.6 = 14.9
        /// 21.6 / 36.5 ≈ 0.6 
        /// 14.9 / 36.5 ≈ 0.4
        /// 21.6 * 0.6 + 14.9 * 0.4 = 18.92
        /// 18.92 / 83.02 (Einwohner Deutschlands)  = 0.22f
        /// Source: https://www.rki.de/DE/Content/Gesundheitsmonitoring/Gesundheitsberichterstattung/GBEDownloadsJ/JoHM_S2_2021_Risikogruppen_COVID_19.pdf?__blob=publicationFile
        /// </summary>
        public const float PreIllnessProbability = 0.22f;
    }
}