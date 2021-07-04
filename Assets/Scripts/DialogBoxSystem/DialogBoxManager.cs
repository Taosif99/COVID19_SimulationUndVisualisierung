using UnityEngine;
using TMPro;
namespace DialogBoxSystem {

    /// <summary>
    /// Class which manages the possible dialogue messages.
    /// 
    /// </summary>
    public class DialogBoxManager : MonoBehaviour
    {

        //TODO NOT NULL ASSERTIONS
        //References to UI Elements
        public GameObject OkButtonGB;
        public GameObject CancelButtonGB;
        public GameObject DialogTextGB;
        public GameObject DialogTitleGB;
        public GameObject DialogBoxGB;
        public GameObject InputfieldGB;
        private TextMeshProUGUI _dialogText;
        private TextMeshProUGUI _dialogTitle;




        //Action which will be triggered by the file handler
        //public static DialogBoxManager Instance;
        private DialogBox _currentDialogBox;
        public static DialogBoxManager Instance;

        
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            _dialogTitle = DialogTitleGB.GetComponent<TextMeshProUGUI>();
            _dialogText = DialogTextGB.GetComponent<TextMeshProUGUI>();
        }


        public void HandleDialogBox(DialogBox dialogBox)
        {
            DialogTitleGB.SetActive(true);
            DialogBoxGB.SetActive(true);
            OkButtonGB.SetActive(dialogBox.HasOkButton);
            CancelButtonGB.SetActive(dialogBox.HasCancelButton);
            InputfieldGB.SetActive(dialogBox.HasTextField);
            _dialogTitle.SetText(dialogBox.Title);
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