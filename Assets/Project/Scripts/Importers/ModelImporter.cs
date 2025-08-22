// Scripts/Importers/ModelImporter.cs
using Dummiesman;
using System.IO;
using UnityEngine;

public class ModelImporter : MonoBehaviour
{
    public GameObject fluidSystem; // Asignar desde el inspector

    public static ModelImporter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public GameObject ImportOBJ(string filePath)
    {
        string objText = File.ReadAllText(filePath);

        OBJLoader loader = new OBJLoader();
        GameObject model = loader.Load(objText);
        model.name = Path.GetFileNameWithoutExtension(filePath);
        model.transform.localScale = Vector3.one * 0.1f;
        model.transform.position = new Vector3(0, 0.5f, 0);

        // Colisiones
        MeshCollider collider = model.AddComponent<MeshCollider>();
        collider.convex = true;

        var obiCollider = model.AddComponent<Obi.ObiCollider>();
        obiCollider.sourceCollider = collider;
        obiCollider.CollisionMaterial = Resources.Load<Obi.ObiCollisionMaterial>("DefaultObiMaterial");

        Debug.Log("Modelo importado con éxito.");

        // Activar sistema de fluido
        if (fluidSystem != null)
            fluidSystem.SetActive(true);

        FindObjectOfType<ModeController>().EnableEditMode(model);

        return model;
    }
}
