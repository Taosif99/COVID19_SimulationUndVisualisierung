
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



        //This actions holds what will happen when ok or cancel is pressed, they do not need the simulation
        public  Action OnConfirmationPressed;
        public  Action OnCancelPressed;


        //These Actions also need to get the current simulation to the controller
        public Action<Simulation.Edit.Simulation> OnConfirmationPressedS;
        public Action<Simulation.Edit.Simulation> OnCancelPressedS;

        public DialogBox(string name, string message)
        {
            Name = name;
            Message = message;
            HasOkButton = true;
            HasCancelButon = true;
        }


        //static messages for creation
        //Probably a factory would be a cleaner implementation
        

        
        public static DialogBox CreateFileNotFoundDB()
        {
            //add path
            string msg = "Save file not found !";
            string name = "File not found";
            DialogBox dialogBox = new DialogBox(name, msg);
            dialogBox.HasCancelButon = false;
            //We must go back to the previous scene when ok is clicked
            //dialogBox.OnConfirmationPressed += 


            return dialogBox;
        
        }

        public static DialogBox CreateFileAlreadyExistsDB()
        {
            //add path
            string msg = "File with the same name already exists. Do you want to overwrite this file?";
            string name = "File already exists";
            DialogBox dialogBox = new DialogBox(name, msg);
            dialogBox.OnConfirmationPressedS += FileHandler.SaveData;
            dialogBox.OnConfirmationPressed += SceneLoader.Instance.LoadSimulation;
            return dialogBox;

        }


        public static DialogBox CreateSureToDeleteDB()
        {
            //add path
            string msg = "Are you sure you want to delete the file ? ";
            string name = "Delete file ?";
            DialogBox dialogBox = new DialogBox(name, msg);
            dialogBox.OnConfirmationPressed += FileHandler.DeleteData;
            dialogBox.OnConfirmationPressed += StartSimulationSaveManager.Instance.LoadSimulationButtons;
            return dialogBox;
        }

    }
}