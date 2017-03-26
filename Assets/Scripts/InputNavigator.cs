using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputNavigator : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    EventSystem _system;
    private bool _isSelect = false;

    void Start()
    {
        _system = EventSystem.current;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && _isSelect)
        {

            Selectable next = null;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                next = _system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            }
            else
            {
                next = _system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            }
            if (next != null)
            {
                //InputField inputfield = next.GetComponent<InputField>();
                _system.SetSelectedGameObject(next.gameObject, new BaseEventData(_system));
            }
            else
            {
                Debug.LogError("找不到下一个控件");
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        _isSelect = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _isSelect = false;
    }
}
