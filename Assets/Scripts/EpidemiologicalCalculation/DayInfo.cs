using System;
namespace EpidemiologicalCalculation
{

    /// <summary>
    /// Class which encapsulates information which repesent a dataset
    /// of a saved day information, which include, date, r-values, incidenece, etc...
    /// </summary>
    public class DayInfo
    {

        private DateTime _date;
        private int _amountNewInfections;
        private float _rValue;
        private float _rValue7;
        private float _incidence;

        private int _amountUninfected;
        private int _amountInfected;
        private int _amountInfectious;
        private int _amountSymptoms;
        private int _amountRecovered;
        private int _amountDead;
        public DayInfo(DateTime date)
        {
            _date = date;
            _amountNewInfections = 0;
            _rValue = -1f; //-1f as undefined value
            _rValue7 = -1f;
        }

        public DateTime Date { get => _date; set => _date = value; }
        public int AmountNewInfections { get => _amountNewInfections; set => _amountNewInfections = value; }
        public float RValue { get => _rValue; set => _rValue = value; }
        public float Incidence { get => _incidence; set => _incidence = value; }
        public float RValue7 { get => _rValue7; set => _rValue7 = value; }

        public int AmountUninfected { get => _amountUninfected; set => _amountUninfected = value; }
        public int AmountInfected { get => _amountInfected; set => _amountInfected = value; }
        public int AmountInfectious { get => _amountInfectious; set => _amountInfectious = value; }
        public int AmountSymptoms { get => _amountSymptoms; set => _amountSymptoms = value; }
        public int AmountRecovered { get => _amountRecovered; set => _amountRecovered = value; }
        public int AmountDead { get => _amountDead; set => _amountDead = value; }

        public override string ToString()
        {
            return Date.ToString("dd/MM/yyyy") + ";" + AmountNewInfections + ";" + RValue + ";" + RValue7 + ";" + Incidence + ";"
                + AmountUninfected + ";" + AmountInfected + ";" + AmountInfectious + ";" + AmountSymptoms + ";" + AmountRecovered +";" + AmountDead ;
        }
        


    }
}