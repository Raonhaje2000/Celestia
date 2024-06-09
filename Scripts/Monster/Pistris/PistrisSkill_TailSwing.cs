using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistrisSkill_TailSwing : MonsterSkill
{
    MonsterSkillData skillData; // 해당 스킬의 데이터

    private void Awake()
    {
        skillData = ScriptableObject.Instantiate(Resources.Load<MonsterSkillData>("GameData/Skill/Monster/Field03_ValleyForest/PistrisSkill_TailSwingData"));
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override MonsterSkillData GetMonsterSkillData()
    {
        return skillData;
    }

    public override void UseSkill()
    {
        throw new System.NotImplementedException();
    }

    public override bool UseSkillPossible()
    {
        throw new System.NotImplementedException();
    }
}
