using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkillUsing : MonoBehaviour
{
    public PlayerSkill meteor;

    // Start is called before the first frame update
    void Start()
    {
        meteor = GetComponent<PlayerSkill_Meteor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            meteor.UseSkill();
        }
    }
}
