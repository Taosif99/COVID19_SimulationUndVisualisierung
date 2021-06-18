
using System;

namespace DialogBoxSystem
{
    /// <summary>
    /// Class to implement the basic functionality of a dialog box
    /// </summary>
    [System.Serializable]
    public class DialogBox
    {

        public string Title;
        public string Message;
        public bool HasOkButton;
        public bool HasCancelButon;
        public bool HasTextField;


        //These actions hold what will happen when ok or cancel is pressed
        public  Action OnConfirmationPressed;
        public  Action OnCancelPressed;

        public DialogBox(string name, string message)
        {
            Title = name;
            Message = message;
            HasOkButton = true;
            HasCancelButon = true;
            HasTextField = false;
        }

    }
}