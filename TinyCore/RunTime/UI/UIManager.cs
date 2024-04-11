using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Bronya
{
    public class UIManager : MonoSingleton<UIManager>       //µ¥ÀýÄ£Ê½
    {
        [SerializeField] private Transform formParent;
        private readonly Dictionary<string, UGuiForm> allOpeneForms = new();
        public void Init()
        {
            // Parent -> Layer
        }

        private void Update()
        {
            foreach(KeyValuePair<string,UGuiForm> keyValuePair in allOpeneForms)
            {
                keyValuePair.Value.OnUpdate();
            }
        }

        //TODO Configs
        public void Open(string formName, object userData = null)
        {
            if(allOpeneForms.TryGetValue(formName,out UGuiForm openedForm))
            {
                openedForm.OnOpen();
                return;
            }
            //lambada
            SourceManager.Instance.LoadAsync<GameObject>($"Form/{formName}",uiForm =>
            {
                uiForm.transform.SetParent(formParent);
                UGuiForm uGuiForm = uiForm.GetComponent<UGuiForm>();
                uGuiForm.OnInit();
                uGuiForm.OnOpen();
                allOpeneForms.Add(formName, uGuiForm);
            });
        }
        public void Close(string formName,bool isDestroy = false)
        {
            if (!allOpeneForms.TryGetValue(formName, out UGuiForm openedForm))
            {
                return;
            }
            openedForm.OnClose();

            if(!isDestroy)
            {
                return;
            }
            allOpeneForms.Remove(formName);
            //objectManager.Instance.Reload();
        }
    }

}
