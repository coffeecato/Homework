using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

/* *
 *      UI初始化流程
 * 
 *  1、UIModel.Init()              // 单件，首次使用时创建并初始化
 *  2、UIViewModel::OnEnable()
 *                    |__ AttachModels                  // 绑定Models，数据拷贝(Model -> ViewModel)
 *                             |__ Attach               // 绑定Model
 *                             |__ CopyPropertiesFrom   // 数据拷贝
 *                    |__ UISysManager.Register         // 注册ViewModel
 *     UIViewModel::OnDisable()
 *                    |__ DetachModels                  // 解绑Models
 *                             |__ Detach               // 解绑Models
 *                    |__ UISysManager.Unregister       // 注销ViewModel
 *  3、UIView.Awake
 *              |__ InitWidgets                         // 
 *              |__ WidgetInitialized                   // 初始化UI控件
 *     UIView.Start
 *              |__ Attach ViewModel                    // 建立与UIViewModel的关联，此时ViewModel数据已准备好
 *              |__ SetAnchors                          // 设置与parent的绑定关系
 *              |__ OnShow                              // 显示UI
 *     UIView.OnEnable
 *              |__ Attach ViewModel                    // 建立与UIViewModel的关联，此时ViewModel数据已准备好
 *     UIView.OnDisable
 *              |__ Detach ViewModel
 * */

namespace Framework.UI
{
    public class UISysManager : SingletonMono<UISysManager>
    {
        protected int _firstID = -1;
        public Dictionary<int, UIViewModel> _viewModelDict = new Dictionary<int, UIViewModel>();
        
        public UIViewModel GetViewModelByID(int id)
        {
            UIViewModel vm;
            return _viewModelDict.TryGetValue(id, out vm) ? vm : null;
        }

        public int Register(UIViewModel vm)
        {
            int id = AssignID();

            if (_viewModelDict.ContainsKey(id))
            {
                LogManager.LogWarning("UISysManager:Register", vm.name+" has already exists!");
            }
            else
            {
                _viewModelDict.Add(id, vm);
            }
            return id;
        }

        public void Unregister(UIViewModel vm)
        {
            if (_viewModelDict.ContainsKey(UIViewModel.id))
            {
                _viewModelDict.Remove(UIViewModel.id);
            }
            else
            {
                LogManager.LogWarning("UISysManager:Unregister ", vm.name + " not exists!");
            }
        }

        protected virtual int AssignID()
        {
            ++_firstID;
            return _firstID;
        }

        public void Load(Object asset)
        {
        }

        public virtual void Load(string path)
        {
        }

        public virtual IEnumerator LoadAsync(string path)
        {
            yield break;
        }

        public void Unload(int id)
        {
            UIViewModel vm = GetViewModelByID(id);
            if (vm != null)
            {
                Destroy(vm.gameObject);
            }
        }

        public void SetActive(int id, bool active)
        {
            UIViewModel vm = GetViewModelByID(id);
            if (vm != null)
            {
                vm.gameObject.SetActive(active);
            }
        }
    }
}