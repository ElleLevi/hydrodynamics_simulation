using UnityEngine;
using UnityEngine.UI;
using SFB;

public class ModelImportUI : MonoBehaviour
{
    public Button importButton;
    public Text modelNameText;
    public Button nextButton;
    public GameObject fluidSystem;

    private GameObject importedModel;

    void Start()
    {
        nextButton.interactable = false;

        importButton.onClick.AddListener(() =>
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Selecciona un modelo", "", "obj", false);
            if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
            {
                importedModel = ModelImporter.Instance.ImportOBJ(paths[0]);
                modelNameText.text = importedModel.name;
                nextButton.interactable = true;

                FindObjectOfType<ModeController>().EnableEditMode(importedModel);
            }
        });

        nextButton.onClick.AddListener(() =>
        {
            fluidSystem.SetActive(false); // aún no iniciamos simulación
            FindObjectOfType<StageController>().NextStage();
        });
    }
}
