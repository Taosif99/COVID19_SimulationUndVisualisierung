using UnityEngine;
using TMPro;


namespace FileHandling {

    /// <summary>
    /// Class which manages the possible dialogue messages.
    /// 
    /// TODO: proper singleton
    /// </summary>
    public class DialogBoxManager : MonoBehaviour
    {

        //We will be possible to set up the  messages in the UI
        //public DialogBox CurrentDialogBox;

        //TODO NOT NULL ASSERTIONS
        //References to UI Elements
        public GameObject OkButtonGB;
        public GameObject CancelButtonGB;
        public GameObject DialogTextGB;
        public GameObject DialogBoxGB;
        private TextMeshProUGUI _dialogText;
       



        //Action which will be triggered by the file handler
        //public static DialogBoxManager Instance;
        private DialogBox _currentDialogBox;



        public static DialogBoxManager Instance;

        
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _dialogText = DialogTextGB.GetComponent<TextMeshProUGUI>();
         
        }


        public void HandleDialogBox(DialogBox dialogBox)
        {
            DialogBoxGB.SetActive(true);
            OkButtonGB.SetActive(dialogBox.HasOkButton);
            CancelButtonGB.SetActive(dialogBox.HasCancelButon);
            _dialogText.SetText(dialogBox.Message);
            _currentDialogBox = dialogBox;
        }


        #region button methods

        public void OkClicked() 
        {
            DialogBoxGB.SetActive(false);
            _currentDialogBox.OnConfirmationPressed?.Invoke();
 
        }


        public void CancelClicked()
        {
            DialogBoxGB.SetActive(false);
            _currentDialogBox.OnCancelPressed?.Invoke();
        }
        #endregion
    }
}