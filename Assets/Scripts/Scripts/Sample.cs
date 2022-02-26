using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Deselect;
        entry.callback.AddListener(data => OnDeselected());
        trigger.triggers.Add(entry);
    }

    public void OnDeselected()
    {

    }
}
