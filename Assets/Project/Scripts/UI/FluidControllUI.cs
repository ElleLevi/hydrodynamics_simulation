using UnityEngine;
using UnityEngine.UI;
using Obi;

public class FluidControlUI : MonoBehaviour
{
    [Header("Referencias")]
    public ObiEmitter emitter;
    public ObiFluidEmitterBlueprint emitterBlueprint;

    private ObiEmitterShapeSphere sphereShape;

    [Header("Sliders")]
    public Slider directionXSlider, directionYSlider, directionZSlider;
    public Slider viscositySlider, radiusSlider, densitySlider;

    private void Start()
    {
        // Intenta obtener el shape (componente hijo)
        sphereShape = emitter.GetComponentInChildren<ObiEmitterShapeSphere>();

        if (sphereShape == null)
        {
            Debug.LogWarning("No se encontró un ObiEmitterShapeSphere como hijo del ObiEmitter.");
        }

        // Inicializa valores desde los sliders
        UpdateAllValues();

        // Listeners de dirección
        directionXSlider.onValueChanged.AddListener(_ => UpdateDirection());
        directionYSlider.onValueChanged.AddListener(_ => UpdateDirection());
        directionZSlider.onValueChanged.AddListener(_ => UpdateDirection());

        // Listener para viscosidad
        viscositySlider.onValueChanged.AddListener(v => emitterBlueprint.viscosity = v);

        // Listener para radio del shape
        radiusSlider.onValueChanged.AddListener(UpdateRadius);

        // Listener para densidad
        densitySlider.onValueChanged.AddListener(v => emitterBlueprint.restDensity = v);
    }

    private void UpdateRadius(float value)
    {
        if (sphereShape != null)
        {
            sphereShape.radius = value;
        }
    }

    private void UpdateDirection()
    {
        Vector3 dir = new Vector3(
            directionXSlider.value,
            directionYSlider.value,
            directionZSlider.value
        ).normalized;

        if (dir != Vector3.zero)
            emitter.transform.forward = dir;
    }

    private void UpdateAllValues()
    {
        emitterBlueprint.viscosity = viscositySlider.value;
        emitterBlueprint.restDensity = densitySlider.value;

        UpdateRadius(radiusSlider.value);
        UpdateDirection();
    }
}
