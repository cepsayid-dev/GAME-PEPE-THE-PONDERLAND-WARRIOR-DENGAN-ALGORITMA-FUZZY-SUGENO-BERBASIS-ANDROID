using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class FuzzySugenoSystem : MonoBehaviour
{
    public List<FuzzyVariable> inputVariables = new List<FuzzyVariable>();
    public List<SugenoRule> rules = new List<SugenoRule>();

    public float Evaluate(params float[] inputValues)
    {
        List<float> firingStrengths = new List<float>();
        List<float> ruleOutputs = new List<float>();

        foreach (var rule in rules)
        {
            float firingStrength = 1f;

            foreach (var condition in rule.conditions)
            {
                foreach (var variable in inputVariables)
                {
                    if (variable.name == condition.variableName)
                    {
                        int inputIndex = inputVariables.IndexOf(variable);
                        if (inputIndex < inputValues.Length)
                        {
                            float membership = variable.GetMembership
                                (condition.fuzzySetName, inputValues[inputIndex]);
                            firingStrength = Mathf.Min(firingStrength, membership);

                        }
                    }
                }
                
            }
            firingStrengths.Add(firingStrength);

            float output = rule.CalculatedOutput(inputValues);

            ruleOutputs.Add(output);

        }

        float sumWeightOutput = 0f;
        float sumWeight = 0f;

        for (int i = 0; i < rules.Count; i++)
        {

             Debug.Log("rule " + rules[i].name +" : " + firingStrengths[i] + " " + ruleOutputs[i]);
            sumWeightOutput += firingStrengths[i] * ruleOutputs[i];
            sumWeight += firingStrengths[i];
        }





        return sumWeight > 0 ? sumWeightOutput / sumWeight : 0f;
    }
}
