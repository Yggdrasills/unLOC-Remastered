using DialogSystem.Localization;

using UnityEngine;
using UnityEngine.UI;

using Ink.Runtime;

using VContainer;

namespace SevenDays.DialogueSystem
{
    public class InkExample : MonoBehaviour
    {
        [SerializeField] private TextAsset inkJSONAsset;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Text textPrefab;
        [SerializeField] private Button buttonPrefab;
        
        private Story _story;
        private LocalizationService _localizationService;

        [Inject]
        private void Construct(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }
        
        private void Awake()
        {
            RemoveChildren();
            StartStory();
        }

        private void StartStory()
        {
            _story = new Story(inkJSONAsset.text);
            _localizationService.SetCurrentLanguage(Language.Default);

            RefreshView();
        }

        private void RefreshView()
        {
            RemoveChildren();

            while (_story.canContinue)
            {
                string text = _localizationService.GetLocalizedLine(_story.Continue()).Trim();

                if(_story.currentTags.Contains("engLock"))
                    _localizationService.SetCurrentLanguage(Language.English);

                CreateContentView(text);
            }

            // Display all the choices, if there are any!
            if (_story.currentChoices.Count > 0)
            {
                for (int i = 0; i < _story.currentChoices.Count; i++)
                {
                    Choice choice = _story.currentChoices[i];

                    var choiceText = _localizationService.GetLocalizedLine(choice.text).Trim();
                    Button button = CreateChoiceView(choiceText);
                    button.onClick.AddListener(() => OnClickChoiceButton(choice));
                }
            }
            // If we've read all the content and there's no choices, the story is finished!
            else
            {
                Button choice = CreateChoiceView("End of story.\nRestart?");
                choice.onClick.AddListener(StartStory);
            }
        }

        private void OnClickChoiceButton(Choice choice)
        {
            _story.ChooseChoiceIndex(choice.index);
            RefreshView();
        }

        private void CreateContentView(string text)
        {
            Text storyText = Instantiate(textPrefab) as Text;
            storyText.text = text;
            storyText.transform.SetParent(canvas.transform, false);
        }

        private Button CreateChoiceView(string text)
        {
            Button choice = Instantiate(buttonPrefab) as Button;
            choice.transform.SetParent(canvas.transform, false);

            Text choiceText = choice.GetComponentInChildren<Text>();
            choiceText.text = text;

            HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.childForceExpandHeight = false;

            return choice;
        }

        private void RemoveChildren()
        {
            int childCount = canvas.transform.childCount;
            for (int i = childCount - 1; i >= 0; --i)
            {
                GameObject.Destroy(canvas.transform.GetChild(i).gameObject);
            }
        }
    }
}