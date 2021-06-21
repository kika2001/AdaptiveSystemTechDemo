
public class Dado
{
    private float ratio;
    private float currentValue;
    private float referenceValue;
    public Dado(float value,float reference)
    {
        currentValue = value;
        referenceValue = reference;
        CalculateRatio();
    }

    private void CalculateRatio()
    {
        ratio = currentValue / referenceValue;
    }

    public void UpdateData(float value)
    {
        currentValue = value;
        CalculateRatio();
    }
    public void UpdateData(float value, float reference)
    {
        currentValue = value;
        referenceValue = reference;
        CalculateRatio();
    }

    public float GetRatio()
    {
        return ratio;
    }

}
