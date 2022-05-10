using System;

using SaveSystem;

using ToolBox.Serialization;

using UnityEngine;

using Text = UnityEngine.UI.Text;

public class SaveSystemDemo : MonoBehaviour, ISavableMono
{
    [SerializeField, HideInInspector] private string _id = Guid.NewGuid().ToString();

    public Text _text;

    private struct SaveData
    {
        public string text;
        public Vector3 position;
    }

    public void Save()
    {
        var saveData = new SaveData
        {
            text = _text.text,
            position = transform.position
        };
        DataSerializer.Save(_id, saveData);
    }

    public void Load()
    {
        var saveData = DataSerializer.Load<SaveData>(_id);
        _text.text = saveData.text;
        transform.position = saveData.position;
    }
}