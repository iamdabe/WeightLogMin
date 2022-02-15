public class WeightLogWrapperDto
{
    public string secret { get; set; }
    public WeightLogDto weightLog { get; set; }
}

public class WeightLogDto
{
    public string dateAndTime { get; set; }
    public double weight { get; set; }
    public string unit { get; set; }
    public double fatMass { get; set; }
    public double fatMassPercent { get; set; }
    public double leanMass { get; set; }

}