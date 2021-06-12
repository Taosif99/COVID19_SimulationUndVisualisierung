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

                DialogBox diaogBox = DialogBox.CreateSureToReturnToMainMenueDB(editorObjectsManager);
                DialogBoxManager.Instance.HandleDialogBox(diaogBox);
            } // else show some error
        }

    }
}