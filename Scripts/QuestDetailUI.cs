using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestDetailUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI questTitleTypeText;
    [SerializeField] TextMeshProUGUI questTitleText;

    [SerializeField] TextMeshProUGUI summaryLocationText;
    [SerializeField] TextMeshProUGUI summaryContentText;
    [SerializeField] TextMeshProUGUI contentCountText;

    [SerializeField] TextMeshProUGUI questContentText;

    [SerializeField] GameObject questRewardExp;
    [SerializeField] TextMeshProUGUI questRewardExpText;

    [SerializeField] GameObject questRewardGold;
    [SerializeField] TextMeshProUGUI questRewardGoldText;

    [SerializeField] GameObject questRewardFeather;
    [SerializeField] TextMeshProUGUI questRewardFeatherText;

    [SerializeField] GameObject[] questRewardItems;
    [SerializeField] Image[] questRewardItemIcons;
    [SerializeField] TextMeshProUGUI[] questRewardItemTexts;

    [SerializeField] GameObject questLiftedSkill;
    [SerializeField] Image questLiftedSkillIcon;

    private void Awake()
    {
        LoadResources();
        //ActiveQuestDetail(false);
    }

    void Start()
    {
        //ActiveQuestDetail(false);
    }

    void LoadResources()
    {
        questRewardItems = new GameObject[2];
        questRewardItemIcons = new Image[2];
        questRewardItemTexts = new TextMeshProUGUI[2];

        Transform[] children = this.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "QuestTitleTypeText") questTitleTypeText = children[i].gameObject.GetComponent<TextMeshProUGUI>();
            if (children[i].name == "QuestTitleText") questTitleText = children[i].gameObject.GetComponent<TextMeshProUGUI>();

            if (children[i].name == "SummaryLocationText") summaryLocationText = children[i].gameObject.GetComponent<TextMeshProUGUI>();
            if (children[i].name == "SummaryContentText") summaryContentText = children[i].gameObject.GetComponent<TextMeshProUGUI>();
            if (children[i].name == "ContentCountText") contentCountText = children[i].gameObject.GetComponent<TextMeshProUGUI>();

            if (children[i].name == "QuestContentText") questContentText = children[i].gameObject.GetComponent<TextMeshProUGUI>();

            if (children[i].name == "QuestRewardExp") questRewardExp = children[i].gameObject;
            if (children[i].name == "QuestRewardExpText") questRewardExpText = children[i].gameObject.GetComponent<TextMeshProUGUI>();

            if (children[i].name == "QuestRewardGold") questRewardGold = children[i].gameObject;
            if (children[i].name == "QuestRewardGoldText") questRewardGoldText = children[i].gameObject.GetComponent<TextMeshProUGUI>();

            if (children[i].name == "QuestRewardFeather") questRewardFeather = children[i].gameObject;
            if (children[i].name == "QuestRewardFeatherText") questRewardFeatherText = children[i].gameObject.GetComponent<TextMeshProUGUI>();

            if (children[i].name == "QuestRewardItem1") questRewardItems[0] = children[i].gameObject;
            if (children[i].name == "QuestRewardItem2") questRewardItems[1] = children[i].gameObject;

            if (children[i].name == "QuestRewardItem1Icon") questRewardItemIcons[0] = children[i].gameObject.GetComponent<Image>();
            if (children[i].name == "QuestRewardItem2Icon") questRewardItemIcons[1] = children[i].gameObject.GetComponent<Image>();

            if (children[i].name == "QuestRewardItem1Text") questRewardItemTexts[0] = children[i].gameObject.GetComponent<TextMeshProUGUI>();
            if (children[i].name == "QuestRewardItem2Text") questRewardItemTexts[1] = children[i].gameObject.GetComponent<TextMeshProUGUI>();

            if (children[i].name == "QuestLiftedSkill") questLiftedSkill = children[i].gameObject;
            if (children[i].name == "QuestLiftedSkillIcon") questLiftedSkillIcon = children[i].gameObject.GetComponent<Image>();
        }
    }

    public void ActiveQuestDetail(bool active)
    {
        questTitleTypeText.gameObject.SetActive(active);
        questTitleText.gameObject.SetActive(active);

        summaryLocationText.gameObject.SetActive(active);
        summaryContentText.gameObject.SetActive(active);
        contentCountText.gameObject.SetActive(active);

        questContentText.gameObject.SetActive(active);

        questRewardExp.SetActive(active);
        questRewardGold.SetActive(active);
        questRewardFeather.SetActive(active);

        for(int i = 0; i < questRewardItems.Length; i++)
        {
            questRewardItems[i].SetActive(active);
        }

        questLiftedSkill.SetActive(active);
    }

    public void SetQuestDetail(QuestData quest, bool isQuestCompletion)
    {
        if (quest == null) ActiveQuestDetail(false);
        else ActiveQuestDetail(true);

        questTitleTypeText.text = string.Format("[ {0} ]", ChangeQuestTypeToString(quest.Type));
        questTitleText.text = quest.QuestTitle;

        summaryLocationText.text = string.Format("[수행 위치] {0}", quest.ProgressAreaString);
        summaryContentText.text = quest.Summary;

        if(quest.Condition != QuestData.CompletionCondition.AreaArrival)
        {
            contentCountText.text = string.Format("{0} / {1}", quest.CurrentCount, quest.CompletionCount);
        }
        else
        {
            contentCountText.gameObject.SetActive(false);
        }

        questContentText.text = (!isQuestCompletion) ? quest.StartContent : quest.CompletionContent;

        questRewardExpText.text = quest.RewardExp.ToString();
        questRewardGoldText.text = quest.RewardGold.ToString();

        if (quest.RewardFeather > 0) questRewardFeatherText.text = quest.RewardFeather.ToString();
        else questRewardFeather.SetActive(false);

        for(int i = 0; i < questRewardItems.Length; i++)
        {
            if (quest.RewardItems[i] != null)
            {
                quest.RewardItems[i].SetInventoryItemData();

                questRewardItemIcons[i].sprite = quest.RewardItems[i].GetInventoryItemData().Icon;
                questRewardItemTexts[i].text = quest.RewardItemsCount[i].ToString();
            }
            else questRewardItems[i].SetActive(false);
        }

        if (quest.LiftedSkill != null) questLiftedSkillIcon.sprite = quest.LiftedSkill.Icon;
        else questLiftedSkill.SetActive(false);
    }

    string ChangeQuestTypeToString(QuestData.QuestType type)
    {
        string str = (type == QuestData.QuestType.main) ? "메인" : "서브";

        return str;
    }
}
