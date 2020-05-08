using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Setup
{
    public class DungeonSetupHandler : MonoBehaviour
    {
        public InputField WidthField;
        public InputField HeightField;
        public InputField DonjonField;
        public Text ValidationErrorText;
    
        private DungeonStateManager m_DungeonStateManager;

        private void Awake()
        {
            if (WidthField == null) throw new ArgumentNullException(nameof(WidthField));
            if (HeightField == null) throw new ArgumentNullException(nameof(HeightField));
            if (ValidationErrorText == null) throw new ArgumentNullException(nameof(ValidationErrorText));

            m_DungeonStateManager = GameObject.FindGameObjectWithTag("DungeonStateManager")?
                                        .GetComponent<DungeonStateManager>() ??
                                    throw new ArgumentNullException(nameof(m_DungeonStateManager));
        }

        private void Start()
        {
            EventSystem.current.SetSelectedGameObject(WidthField.gameObject, null);
            WidthField.OnPointerClick(new PointerEventData(EventSystem.current));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (WidthField.isFocused)
                {
                    EventSystem.current.SetSelectedGameObject(HeightField.gameObject, null);
                    HeightField.OnPointerClick(new PointerEventData(EventSystem.current));
                }
            
                if (HeightField.isFocused)
                {
                    EventSystem.current.SetSelectedGameObject(WidthField.gameObject, null);
                    WidthField.OnPointerClick(new PointerEventData(EventSystem.current));
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                TryStart();
            }
        }

        public void TryStart()
        {
            if (!string.IsNullOrEmpty(DonjonField.text))
            {
                if (!File.Exists(DonjonField.text))
                {
                    ValidationErrorText.text = "Couldn't find that Donjon file";
                    return;
                }

                m_DungeonStateManager.DonjonTsvFilePath = DonjonField.text;
                SceneManager.LoadScene("DungeonScene");
                return;
            }
            
            if (!int.TryParse(WidthField.text, out var widthValue) || widthValue < 1)
            {
                ValidationErrorText.text = "Check your width value";
                return;
            }
        
            if (!int.TryParse(HeightField.text, out var heightValue) || heightValue < 1)
            {
                ValidationErrorText.text = "Check your height value";
                return;
            }

            m_DungeonStateManager.Width = widthValue;
            m_DungeonStateManager.Height = heightValue;
        
            SceneManager.LoadScene("DungeonScene");
        }
    }
}
