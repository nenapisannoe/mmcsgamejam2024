using UnityEngine;
using UnityEngine.UI;

public class ProjectorControllerDialog : MonoBehaviour {

    public GameObject EnabledContainer;
    public Toggle EnabledToggle;
    [SerializeField] private Sprite ToggleOnSprite;
    [SerializeField] private Sprite ToggleOffSprite;

    [Space]
    public GameObject AngleContainer;
    public Slider AngleSlider;
    [Space]
    public GameObject RedContainer;
    public Slider RedSlider;
    [Space]
    public GameObject GreenContainer;
    public Slider GreenSlider;
    [Space]
    public GameObject BlueContainer;
    public Slider BlueSlider;

    private LightProjectorControllerData projectorData;

    private Image toggleImage;

    public void Init(LightProjectorControllerData data) {
        projectorData = data;
        
        EnabledContainer.SetActive(projectorData.PlayerCanModifyEnabled);
        if (projectorData.PlayerCanModifyEnabled) {
            EnabledToggle.SetIsOnWithoutNotify(projectorData.IsEnabled);
        }
        
        AngleContainer.SetActive(projectorData.PlayerCanModifyAngle);
        if (projectorData.PlayerCanModifyAngle) {
            AngleSlider.minValue = projectorData.MinAngleDelta;
            AngleSlider.maxValue = projectorData.MaxAngleDelta;
            AngleSlider.SetValueWithoutNotify(projectorData.CurrentAngleDelta);
        }
        
        RedContainer.SetActive(projectorData.PlayerCanModifyRed);
        if (projectorData.PlayerCanModifyRed) {
            RedSlider.SetValueWithoutNotify(projectorData.Color.r);
        }
        
        GreenContainer.SetActive(projectorData.PlayerCanModifyGreen);
        if (projectorData.PlayerCanModifyGreen) {
            GreenSlider.SetValueWithoutNotify(projectorData.Color.g);
        }
        
        BlueContainer.SetActive(projectorData.PlayerCanModifyBlue);
        if (projectorData.PlayerCanModifyBlue) {
            BlueSlider.SetValueWithoutNotify(projectorData.Color.b);
        }
    }

    void Awake()
    {
        toggleImage = EnabledToggle.GetComponentInChildren<Image>();
    }
    
    public void OnEnabledToggleChanged(bool value) {
        projectorData.IsEnabled = value;
        toggleImage.sprite = value ? ToggleOnSprite : ToggleOffSprite;
        projectorData.ProjectorDataChangedEvent();
    }

    public void OnAngleSliderChanged(float value) {
        projectorData.CurrentAngleDelta = (int)value;
        projectorData.ProjectorDataChangedEvent();
    }
    
    public void OnRedSliderChanged(float value) {
        projectorData.Color = new Color(value, projectorData.Color.g, projectorData.Color.b);
        projectorData.ProjectorDataChangedEvent();
    }
    
    public void OnGreenSliderChanged(float value) {
        projectorData.Color = new Color(projectorData.Color.r, value, projectorData.Color.b);
        projectorData.ProjectorDataChangedEvent();
    }
    
    public void OnBlueSliderChanged(float value) {
        projectorData.Color = new Color(projectorData.Color.r, projectorData.Color.g, value);
        projectorData.ProjectorDataChangedEvent();
    }
    
    public void OnExitButtonClicked() {
        projectorData = null;
        GameController.Instance.HideProjectorControllerDialog();
    }

}