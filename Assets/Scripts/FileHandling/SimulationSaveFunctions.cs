using UnityEngine;
using EditorObjects;

namespace FileHandling
{

    /// <summary>
    /// Class which mainly contains propagates the on click method to an 
    /// appropiate DialogBox
    /// </summary>
    public class SimulationSaveFunctions: MonoBehaviour
    {

        public EditorObjectsManager editorObjectsManager;
        public void BackToMainMenuSave()
        {

            //Get the current file 
            if (FileHandler.SaveFileExists())
            {

                //DialogBox diaogBox = DialogBox.CreateSureToReturnToMainMenueDB(editorObjectsManager);

                string msg = "Do you want to save your changes?";
                string name = "Save file ?";
                DialogBox dialogBox = new DialogBox(name, msg);
                dialogBox.OnConfirmationPressed += ReturnToMainMenuSaveAction;
                dialogBox.OnCancelPressed += SceneLoader.Instance.LoadMainMenu;

                DialogBoxManager.Instance.HandleDialogBox(dialogBox);
            } // else TODO show some error
        }

        private void ReturnToMainMenuSaveAction()
        {
            editorObjectsManager.SaveToFile();
            SceneLoader.Instance.LoadMainMenu();
        }


    }
}