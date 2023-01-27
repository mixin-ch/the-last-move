
using UnityEngine.UIElements;

public class CustomSlider : Slider
{
    VisualElement progressFiller;

    public CustomSlider()
    {
        // Add custom class for easy manipulation later
        AddToClassList("custom-slider");

        // Get the tracker
        VisualElement tracker = this.Q<VisualElement>("unity-tracker");

        // Create progress filler inside the tracker
        progressFiller = new VisualElement();
        progressFiller.name = "unity-filler";
        progressFiller.AddToClassList("horizontal-unity-filler");
        progressFiller.style.position = new StyleEnum<Position>(Position.Absolute);
        progressFiller.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
        tracker.Add(progressFiller);

        // Prevent filler to overflow tracker (if it has rounded borders)
        tracker.style.overflow = new StyleEnum<Overflow>(Overflow.Hidden);

        // Make sure the filler will fill-in the good portion of length when changing slider position
        this.RegisterValueChangedCallback((ChangeEvent<float> ev) => { UpdateProgressFillerPosition(ev.newValue); });

        // Initialize the filler correctly once the slider is drawn
        RegisterCallback((GeometryChangedEvent ev) => { UpdateProgressFillerPosition(value); });
    }

    /// <summary>
    /// Method to position the filler at the right spot
    /// </summary>
    /// <param name="value"></param>
    private void UpdateProgressFillerPosition(float value)
    {
        float percent = (value / (highValue - lowValue)) * 100;
        progressFiller.style.width = new StyleLength(new Length(percent, LengthUnit.Percent));
    }


    /// <summary>
    /// Factory for easy use inside the UI builder
    /// </summary>
    public new class UxmlFactory : UxmlFactory<CustomSlider, CustomSliderTraits>
    {
    }

    /// <summary>
    /// Inherit default traits
    /// </summary>
    public class CustomSliderTraits : UxmlTraits
    {
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            CustomSlider slider = (CustomSlider)ve;
            slider.UpdateProgressFillerPosition(slider.value);
        }
    }
}


