using UnityEngine;
using System.Collections.Generic;

public class NPCAIController : MonoBehaviour
{
    public FuzzySugenoSystem fuzzySystem;
    public Transform player; // Referensi ke pemain

    public float distanceToPlayer;
    public float npcHealth = 1f;
    public float playerDamagePotential = 20f;
    public float npcSpeed = 3f;

    public float moveDecissionInterval = 0.5f;
    private float decissionTimemr = 0f;


    protected void setupFuuzzy()
    {
        #region Input Var FS

        fuzzySystem = new FuzzySugenoSystem();
        //set jarak
        var distamnceVar = new FuzzyVariable();
        distamnceVar.name = "Jarak";
        distamnceVar.minVale = 0f;
        distamnceVar.maxVale = 15f;

        FuzzySet dekatSet = new FuzzySet()
        {
            name = "Dekat",
            type = FuzzySet.SetType.CustomPoint,
            customPoints = new Vector2[]
            {
                new Vector2(0f, 1f),    // 0m: 0% membership
                new Vector2(5f, 1f),  // 5m: 20%
                new Vector2(10f, 0f), // 10m: 10% (dip)
                new Vector2(15f, 0f), // 15m: 80%
            }
        };

        FuzzySet jauhSet = new FuzzySet()
        {
            name = "Jauh",
            type = FuzzySet.SetType.CustomPoint,
            customPoints = new Vector2[]
            {
                new Vector2(0f, 0f),    // 0m: 0% membership
                new Vector2(5f, 0f),  // 5m: 20%
                new Vector2(10f, 1f), // 10m: 10% (dip)
                new Vector2(15f, 1f), // 15m: 80%
            }
        };
        distamnceVar.fuzzySets.Add(dekatSet);
        distamnceVar.fuzzySets.Add(jauhSet);

        //setNyawa
        var hpVar = new FuzzyVariable();
        hpVar.name = "Hp";
        hpVar.minVale = 0f;
        hpVar.maxVale = 100f;

        FuzzySet hlowSet = new FuzzySet()
        {
            name = "Low",
            type = FuzzySet.SetType.CustomPoint,
            customPoints = new Vector2[]
            {
                new Vector2(0f, 1f),    // 0m: 0% membership
                new Vector2(25f, 1f),  // 5m: 20%
                new Vector2(50f, 0f), // 10m: 10% (dip)
                new Vector2(75f, 0f), // 15m: 80%
                new Vector2(100f, 0f), // 15m: 80%
            }
        };

        FuzzySet hmedSet = new FuzzySet()
        {
            name = "Med",
            type = FuzzySet.SetType.CustomPoint,
            customPoints = new Vector2[]
            {
                new Vector2(0f, 0f),    // 0m: 0% membership
                new Vector2(25f, 0f),  // 5m: 20%
                new Vector2(50f, 1f), // 10m: 10% (dip)
                new Vector2(75f, 0f), // 15m: 80%
                new Vector2(100f, 0f), // 15m: 80%
        }
        };

        FuzzySet hhighSet = new FuzzySet()
        {
            name = "Full",
            type = FuzzySet.SetType.CustomPoint,
            customPoints = new Vector2[]
        {
                new Vector2(0f, 0f),    // 0m: 0% membership
                new Vector2(25f, 0f),  // 5m: 20%
                new Vector2(50f, 0f), // 10m: 10% (dip)
                new Vector2(75f, 1f), // 15m: 80%
                new Vector2(100f, 1f), // 15m: 80%
        }
        };
        hpVar.fuzzySets.Add(hlowSet);
        hpVar.fuzzySets.Add(hmedSet);
        hpVar.fuzzySets.Add(hhighSet);

        //setDamage
        var dmgVar = new FuzzyVariable();
        dmgVar.name = "Damage";
        dmgVar.minVale = 0f;
        dmgVar.maxVale = 20f;

        FuzzySet dlowSet = new FuzzySet()
        {
            name = "Low",
            type = FuzzySet.SetType.CustomPoint,
            customPoints = new Vector2[]
        {
                new Vector2(0f, 1f),    // 0m: 0% membership
                new Vector2(6f, 1f),  // 5m: 20%
                new Vector2(12f, 0f), // 10m: 10% (dip)
                new Vector2(20f, 0f), // 15m: 80%
        }
        };

        FuzzySet dhighSet = new FuzzySet()
        {
            name = "High",
            type = FuzzySet.SetType.CustomPoint,
            customPoints = new Vector2[]
        {
                new Vector2(0f, 0f),    // 0m: 0% membership
                new Vector2(6f, 0f),  // 5m: 20%
                new Vector2(12f, 1f), // 10m: 10% (dip)
                new Vector2(20f, 1f), // 15m: 80%
        }
        };
        dmgVar.fuzzySets.Add(dlowSet);
        dmgVar.fuzzySets.Add(dhighSet);

        //setSpeed
        var speedVar = new FuzzyVariable();
        speedVar.name = "Speed";
        speedVar.minVale = 0f;
        speedVar.maxVale = 10f;

        FuzzySet slowSet = new FuzzySet()
        {
            name = "Slow",
            type = FuzzySet.SetType.CustomPoint,
            customPoints = new Vector2[]
        {
                new Vector2(0f, 1f),    // 0m: 0% membership
                new Vector2(2.5f, 1f),  // 5m: 20%
                new Vector2(5f, 0f), // 10m: 10% (dip)
                new Vector2(10f, 0f), // 15m: 80%
        }
        };

        FuzzySet fastSet = new FuzzySet()
        {
            name = "Fast",
            type = FuzzySet.SetType.CustomPoint,
            customPoints = new Vector2[]
            {
                new Vector2(0f, 0f),    // 0m: 0% membership
                new Vector2(2.5f, 0f),  // 5m: 20%
                new Vector2(5f, 1f), // 10m: 10% (dip)
                new Vector2(10f, 1f), // 15m: 80%
            }
        };
        speedVar.fuzzySets.Add(slowSet);
        speedVar.fuzzySets.Add(fastSet);

        fuzzySystem.inputVariables.Add(distamnceVar);
        fuzzySystem.inputVariables.Add(hpVar);
        fuzzySystem.inputVariables.Add(dmgVar);
        fuzzySystem.inputVariables.Add(speedVar);

        #endregion

        #region Rule FS

        // Rule 1: Jika jarak dekat dan HP rendah, menjauh dengan intensitas tinggi
        var rule1 = new SugenoRule();
        rule1.name = "1";
        rule1.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule1.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Low" });
        rule1.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule1.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule1.constantTerm = 1f; // Sugeno Orde-0

        //2
        var rule2 = new SugenoRule();
        rule2.name = "2";
        rule2.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule2.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Low" });
        rule2.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule2.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule2.constantTerm = 1f; // Sugeno Orde-0

        //3
        var rule3 = new SugenoRule();
        rule3.name = "3";
        rule3.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule3.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Low" });
        rule3.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule3.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule3.constantTerm = 1f; // Sugeno Orde-0

        //4
        var rule4 = new SugenoRule();
        rule4.name = "4";
        rule4.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule4.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Low" });
        rule4.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule4.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule4.constantTerm = 1f; // Sugeno Orde-0

        //5
        var rule5 = new SugenoRule();
        rule5.name = "5";
        rule5.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule5.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Mid" });
        rule5.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule5.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule5.constantTerm = 1f; // Sugeno Orde-0

        //6
        var rule6 = new SugenoRule();
        rule6.name = "6";
        rule6.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule6.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Mid" });
        rule6.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule6.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule6.constantTerm = 1f; // Sugeno Orde-0

        //7
        var rule7 = new SugenoRule();
        rule7.name = "7";
        rule7.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule7.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Mid" });
        rule7.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule7.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule7.constantTerm = 1f; // Sugeno Orde-0

        //8
        var rule8 = new SugenoRule();
        rule8.name = "8";
        rule8.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule8.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Mid" });
        rule8.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule8.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule8.constantTerm = 1f; // Sugeno Orde-0

        //9
        var rule9 = new SugenoRule();
        rule9.name = "9";
        rule9.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule9.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Full" });
        rule9.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule9.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule9.constantTerm = 1f; // Sugeno Orde-0

        //10
        var rule10 = new SugenoRule();
        rule10.name = "10";
        rule10.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule10.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Full" });
        rule10.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule10.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule10.constantTerm = 1f; // Sugeno Orde-0

        //11
        var rule11 = new SugenoRule();
        rule11.name = "11";
        rule11.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule11.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Full" });
        rule11.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule11.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule11.constantTerm = 1f; // Sugeno Orde-0

        //12
        var rule12 = new SugenoRule();
        rule12.name = "12";
        rule12.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Dekat" });
        rule12.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Full" });
        rule12.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule12.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule12.constantTerm = 1f; // Sugeno Orde-0

        //13
        var rule13 = new SugenoRule();
        rule13.name = "13";
        rule13.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Jauh" });
        rule13.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Low" });
        rule13.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule13.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule13.constantTerm = 0f; // Sugeno Orde-0

        //14
        var rule14 = new SugenoRule();
        rule14.name = "14";
        rule14.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Jauh" });
        rule14.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Low" });
        rule14.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule14.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule14.constantTerm = 0f; // Sugeno Orde-0

        //15
        var rule15 = new SugenoRule();
        rule15.name = "15";
        rule15.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Jauh" });
        rule15.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Low" });
        rule15.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule15.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule15.constantTerm = 0f; // Sugeno Orde-0

        //16
        var rule16 = new SugenoRule();
        rule16.name = "16";
        rule16.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Jauh" });
        rule16.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Low" });
        rule16.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule16.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule16.constantTerm = 0f; // Sugeno Orde-0

        //17
        var rule17 = new SugenoRule();
        rule17.name = "17";
        rule17.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Jauh" });
        rule17.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Mid" });
        rule17.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule17.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule17.constantTerm = 0f; // Sugeno Orde-0

        //18
        var rule18 = new SugenoRule();
        rule18.name = "18";
        rule18.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "jauh" });
        rule18.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Mid" });
        rule18.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule18.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule18.constantTerm = 0f; // Sugeno Orde-0

        //19
        var rule19 = new SugenoRule();
        rule19.name = "19";
        rule19.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Jauh" });
        rule19.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Mid" });
        rule19.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule19.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule19.constantTerm = 0f; // Sugeno Orde-0

        //20
        var rule20 = new SugenoRule();
        rule20.name = "20";
        rule20.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Jauh" });
        rule20.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Mid" });
        rule20.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule20.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule20.constantTerm = 0f; // Sugeno Orde-0

        //21
        var rule21 = new SugenoRule();
        rule21.name = "21";
        rule21.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Jauh" });
        rule21.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Full" });
        rule21.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule21.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule21.constantTerm = 0f; // Sugeno Orde-0

        //22
        var rule22 = new SugenoRule();
        rule22.name = "22";
        rule22.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Jauh" });
        rule22.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Full" });
        rule22.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Slow" });
        rule22.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule22.constantTerm = 0f; // Sugeno Orde-0

        //23
        var rule23 = new SugenoRule();
        rule23.name = "23";
        rule23.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Jauh" });
        rule23.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Full" });
        rule23.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule23.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "Low" });
        rule23.constantTerm = 0f; // Sugeno Orde-0

        //24
        var rule24 = new SugenoRule();
        rule24.name = "24";
        rule24.conditions.Add(new RuleCondition { variableName = "Jarak", fuzzySetName = "Jauh" });
        rule24.conditions.Add(new RuleCondition { variableName = "Hp", fuzzySetName = "Full" });
        rule24.conditions.Add(new RuleCondition { variableName = "Speed", fuzzySetName = "Fast" });
        rule24.conditions.Add(new RuleCondition { variableName = "Damage", fuzzySetName = "High" });
        rule24.constantTerm = 0f; // Sugeno Orde-0


        fuzzySystem.rules.Add(rule1);
        fuzzySystem.rules.Add(rule2);
        fuzzySystem.rules.Add(rule3);
        fuzzySystem.rules.Add(rule4);
        fuzzySystem.rules.Add(rule5);
        fuzzySystem.rules.Add(rule6);
        fuzzySystem.rules.Add(rule7);
        fuzzySystem.rules.Add(rule8);
        fuzzySystem.rules.Add(rule9);
        fuzzySystem.rules.Add(rule10);
        fuzzySystem.rules.Add(rule11);
        fuzzySystem.rules.Add(rule12);
        fuzzySystem.rules.Add(rule13);
        fuzzySystem.rules.Add(rule14);
        fuzzySystem.rules.Add(rule15);
        fuzzySystem.rules.Add(rule16);
        fuzzySystem.rules.Add(rule17);
        fuzzySystem.rules.Add(rule18);
        fuzzySystem.rules.Add(rule19);
        fuzzySystem.rules.Add(rule20);
        fuzzySystem.rules.Add(rule21);
        fuzzySystem.rules.Add(rule22);
        fuzzySystem.rules.Add(rule23);
        fuzzySystem.rules.Add(rule24);

        #endregion
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setupFuuzzy();
    }
}
