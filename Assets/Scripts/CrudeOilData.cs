
using System;



public enum Itemtype { oil, comsume }
[Serializable]

public class CrudeOilData
{
    public Itemtype itemtype;
    public int item_id;
    public string item_desc;
    public string oil_id;
    public string oil_name;
    public int oil_cost;
    public int probability;
    public int[] setProbability;
}
