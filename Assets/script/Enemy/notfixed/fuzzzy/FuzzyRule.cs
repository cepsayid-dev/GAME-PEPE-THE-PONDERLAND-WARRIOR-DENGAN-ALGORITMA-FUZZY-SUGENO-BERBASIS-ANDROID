

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SugenoRule
{
    public List<RuleCondition> conditions = new List<RuleCondition>();
    public string name = "";
    public float constantTerm;
    public float[] coefficients; // untuk suugeno Orde-1

    public float CalculatedOutput(params float[] inputValues)
    {
        if (coefficients == null || coefficients.Length == 0)
            return constantTerm;

        float output = constantTerm;
        for (int i = 0; i < Mathf.Min(coefficients.Length, inputValues.Length); i++)
        {
            output += coefficients[i] * inputValues[i];
        }
        return output;
    }
}


public class RuleCondition
{
    public string variableName;
    public string fuzzySetName;
}
