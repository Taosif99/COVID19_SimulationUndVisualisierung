
using System;

namespace FileHandling
{
    /// <summary>
    /// Class to implement the basic functionality of a dialog box
    /// </summary>
    [System.Serializable]
    public class DialogBox
    {

        public string Name;
        public string Message;
        public bool HasOkButton;
        public bool HasCancelButon;



        //These actions hold what will happen when ok or cancel is pressed
        public  Action OnConfirmationPressed;
        public  Action OnCancelPressed;

        public DialogBox(string name, string message)
        {
            Name = name;
            Message = message;
            HasOkButton = true;
            HasCancelButon = true;
        }

        //TODO SHOW METHOD
    }
}