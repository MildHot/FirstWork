using UnityEngine;
using System.Collections;

namespace FrameWork
{
    public class BaseModule
    {
        public enum EnumRegisterMode
        {
            NotRegister,
            AutoRegister,
            AlreadyRegister,
        }

        private EnumRegisterMode registerMode = EnumRegisterMode.NotRegister;
        public bool AutoRegister
        {
            get { return registerMode == EnumRegisterMode.NotRegister ? false : true; }
            set
            {
                if (registerMode == EnumRegisterMode.NotRegister || registerMode == EnumRegisterMode.AutoRegister)
                {
                    registerMode = value ? EnumRegisterMode.AutoRegister : EnumRegisterMode.NotRegister;
                }
            }
        }

        public bool HasRegisered
        {
            get { return registerMode == EnumRegisterMode.AlreadyRegister; }
        }

        private EnumObjectState _state = EnumObjectState.Initial;
        public event StateChangeEvent StateChanged;
        public EnumObjectState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    EnumObjectState oldState = this._state;
                    _state = value;
                    if (StateChanged != null)
                    {
                        StateChanged(this, this._state, oldState);
                    }
                    OnStateChange(_state, oldState);
                }
            }
        }

        protected virtual void OnStateChange(EnumObjectState newSate, EnumObjectState oldState) { 
        
        }

        public BaseModule()
        {

        }


        public void Load()
        {
            if (State != EnumObjectState.Initial) return;
            State = EnumObjectState.Loading;
            if (registerMode == EnumRegisterMode.AutoRegister)
            {
                //todo 注册
                ModuleManager.Instance.Register(this);
                registerMode = EnumRegisterMode.AlreadyRegister;
            }

            OnLoad();
            State = EnumObjectState.Ready;
        }

        protected virtual void OnLoad()
        {

        }

        public void Release()
        {
            if (State != EnumObjectState.Disabled)
            {
                State = EnumObjectState.Disabled;
                if (registerMode == EnumRegisterMode.AlreadyRegister)
                {
                    ModuleManager.Instance.UnRegister(this);
                    registerMode = EnumRegisterMode.AutoRegister;
                }

                OnRealease();
            }
        }

        protected virtual void OnRealease()
        {

        }

    }

}
