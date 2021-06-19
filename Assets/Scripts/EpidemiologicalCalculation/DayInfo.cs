using System;
namespace EpidemiologicalCalculation
{

    /// <summary>
    /// Class which encapsulates information which repesent a dataset
    /// in the calclualtion of the epidemiological R-Value.
    /// 
    /// 
    /// </summary>
    public class DayInfo
    {

        private DateTime _date;
        private int _amountNewInfections;
        private float _rValue;


        public DayInfo(DateTime date)
        {
            _date = date;
            _amountNewInfections = 0;
            _rValue = -1f; //-1f as undefined value

        }

        public DateTime Date { get => _date; set => _date = value; }
        public int AmountNewInfections { get => _amountNewInfections; set => _amountNewInfections = value; }
        public float RValue { get => _rValue; set => _rValue = value; }

        
                
        public override string ToString()
        {
            return Date.ToString() + "|" + AmountNewInfections + "|" + RValue;
        }
        


    }
}