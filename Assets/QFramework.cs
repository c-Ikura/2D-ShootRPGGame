using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

namespace QFramework
{
    #region Architecture
    public interface IArchitecture
    {
        //获取工具层
        T GetUtility<T>() where T : class, IUtility;
        //获取数据层
        T GetModel<T>() where T : class, IModel;
        //获取系统层
        T GetSystem<T>() where T : class, ISystem;
        //注册数据层
        void RegisterModel<T>(T model) where T : IModel;
        //注册工具层
        void RegisterUtility<T>(T Utility) where T : IUtility;
        //注册系统层
        void RegisterSystem<T>(T System) where T : ISystem;

        //发送指令
        void SendCommand<T>() where T : ICommand, new();
        void SendCommand<T>(T command) where T : ICommand;

        //请求查询
        TResult SendQuery<TResult>(IQuery<TResult> query);

        //发送事件
        void SendEvent<T>(T e);
        void SendEvent<T>() where T : new();

        //注册事件
        IUnRegister RegisterEvent<T>(Action<T> onEvent);
        void RegisetEvent<T>(Action<T> onEvent);

    }
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        private static T architecture;

        //是否初始化过模块了
        private bool inited = false;
        //数据层缓存
        private List<IModel> models = new List<IModel>();
        //系统层缓存
        private List<ISystem> systems = new List<ISystem>();
        //注册时的补丁 用于数据层初始化注册之后 初始化之前要做的事情
        public static Action<T> OnRegisterPatch = architecture => { };
        //IOC容器 存放模块
        private IOCContainer container = new IOCContainer();

        public static IArchitecture _interface
        {
            get
            {
                if (architecture == null)
                {
                    MakeSureArchitecture();
                }

                return architecture;
            }
        }

        /// <summary>
        /// 确保architecture有实例 类似单例
        /// </summary>
        static void MakeSureArchitecture()
        {
            if (architecture == null)
            {
                architecture = new T();
                architecture.Init();

                OnRegisterPatch?.Invoke(architecture);
                //初始化数据层
                foreach (var architectureModel in architecture.models)
                {
                    architectureModel.Init();
                }
                architecture.models.Clear();
                //初始化系统层
                foreach (var architectureSystem in architecture.systems)
                {
                    architectureSystem.Init();
                }
                architecture.systems.Clear();

                //初始化完毕
                architecture.inited = true;
            }
        }

        /// <summary>
        /// 初始化(交给子类完善具体注册的模块)
        /// </summary>
        protected abstract void Init();


        #region 获取模块

        /// <summary>
        /// 用于获取工具 没有初始化 对内部使用
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns></returns>
        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return container.Get<TUtility>();
        }

        /// <summary>
        /// 获取数据层 没有初始化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return container.Get<TModel>();
        }

        /// <summary>
        /// 获取系统层 没有初始化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return container.Get<TSystem>();
        }

        #endregion


        #region 注册模块

        /// <summary>
        /// 往IOC容器中注册模块 有初始化 可以用于外部调用避免没有初始化
        /// </summary>
        /// <param name="instance">实例</param>
        public static void Register<TValue>(TValue instance)
        {
            MakeSureArchitecture();

            architecture.container.Regiset<TValue>(instance);
        }

        /// <summary>
        /// 用于注册数据层 没有初始化 对内部使用
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="model">模块实例</param>
        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetArchitecture(this);
            container.Regiset<TModel>(model);

            if (!inited)
            {
                models.Add(model);
            }
            else
            {
                model.Init();
            }

        }

        /// <summary>
        /// 用于注册工具层 没有初始化对内部使用
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="instance">模块实例</param>
        public void RegisterUtility<TUtility>(TUtility instance) where TUtility : IUtility
        {
            container.Regiset<TUtility>(instance);
        }

        /// <summary>
        /// 用于注册系统层 没有初始化 
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="system">模块实例</param>

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetArchitecture(this);
            container.Regiset<TSystem>(system);

            if (!inited)
            {
                systems.Add(system);
            }
            else
            {
                system.Init();
            }
        }


        #endregion

        #region 发送指令
        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            var command = new TCommand();
            command.SetArchitecture(this);
            command.Execute();
            command.SetArchitecture(null);
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        #endregion

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }

        #region 事件

        private TypeEventSystem typeEventSystem = new TypeEventSystem();

        public void SendEvent<TEvent>(TEvent e)
        {
            typeEventSystem.Send(e);
        }

        public void SendEvent<TEvent>() where TEvent : new()
        {
            typeEventSystem.Send<T>();
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return typeEventSystem.Register<TEvent>(onEvent);
        }

        public void RegisetEvent<TEvent>(Action<TEvent> onEvent)
        {
            typeEventSystem.Register<TEvent>(onEvent);
        }
        #endregion

    }
    #endregion

    #region Controller
    public interface IController : IBelongToArchitecture, ICanGetModel, ICanGetSystem, ICanSendCommand, ICanRegisterEvent, ICanSendQuery
    {

    }
    #endregion

    #region System
    public interface ISystem :
       IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetUtility, ICanRegisterEvent, ICanSendEvent, ICanGetSystem
    {
        void Init();
    }

    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture architecture;
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            this.architecture = architecture;
        }
        void ISystem.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();

    }
    #endregion

    #region Model
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
    {
        void Init();
    }

    public abstract class AbstractModel : IModel
    {
        private IArchitecture architecture;
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            this.architecture = architecture;
        }
        void IModel.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();

    }
    #endregion

    #region Utility
    public interface IUtility
    {

    }
    #endregion

    #region Command
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanGetSystem, ICanGetModel, ICanSendEvent, ICanSendCommand, ICanSendQuery
    {
        void Execute();
    }

    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture architecture;

        void ICommand.Execute()
        {
            OnExecute();
        }

        protected abstract void OnExecute();

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            this.architecture = architecture;
        }


    }
    #endregion

    #region Query
    public interface IQuery<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetSystem, ICanSendQuery
    {
        TResult Do();
    }

    public abstract class AbstractQuery<T> : IQuery<T>
    {
        public T Do()
        {
            return OnDo();
        }

        protected abstract T OnDo();

        protected IArchitecture mArchitecture;

        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
    }
    #endregion

    #region Rule

    #region  Architecture
    public interface IBelongToArchitecture
    {
        IArchitecture GetArchitecture();
    }

    public interface ICanSetArchitecture
    {
        void SetArchitecture(IArchitecture architecture);
    }
    #endregion

    #region Model
    public interface ICanGetModel : IBelongToArchitecture
    {

    }

    public static class CanGetModelExtension
    {
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }
    #endregion

    #region System
    public interface ICanGetSystem : IBelongToArchitecture
    {

    }

    public static class CanGetSystemExtension
    {
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }
    #endregion

    #region Utility
    public interface ICanGetUtility : IBelongToArchitecture
    {

    }

    public static class CanGetUtilityExtension
    {
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }
    #endregion

    #region Event
    public interface ICanRegisterEvent : IBelongToArchitecture
    {

    }

    public static class CanRegisterEventExtension
    {
        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            return self.GetArchitecture().RegisterEvent<T>(onEvent);
        }
        public static IUnRegister SendEvent<T>(this ICanSendEvent self, Action<T> onEvent)
        {
            return self.GetArchitecture().RegisterEvent<T>(onEvent);
        }
    }

    public interface ICanSendEvent : IBelongToArchitecture
    {

    }

    public static class CanSendEventExtension
    {
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }
        public static void SendEvent<T>(this ICanSendEvent self, T e)
        {
            self.GetArchitecture().SendEvent<T>(e);
        }
    }

    #endregion

    #region Command
    public interface ICanSendCommand : IBelongToArchitecture
    {

    }

    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : class, ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : class, ICommand
        {
            self.GetArchitecture().SendCommand<T>(command);
        }
    }
    #endregion

    #region Query
    public interface ICanSendQuery : IBelongToArchitecture
    {

    }

    public static class CanSendQueryExtension
    {
        public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query)
        {
            return self.GetArchitecture().SendQuery(query);
        }
    }
    #endregion

    #region TypeEventSystem
    public interface ITypeEventSystem
    {
        /// <summary>
        /// 发送事件
        /// </summary>
        void Send<T>() where T : new();
        void Send<T>(T e);

        /// <summary>
        /// 注册事件
        /// </summary>
        IUnRegister Register<T>(Action<T> onEvent);

        /// <summary>
        /// 注销事件
        /// </summary>
        void UnRegister<T>(Action<T> onEvent);
    }

    /// <summary>
    /// 用于注销事件的接口
    /// </summary>
    public interface IUnRegister
    {
        void UnRegister();
    }

    /// <summary>
    /// 注销接口的实现
    /// </summary>
    public struct TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem TypeEventSystem { get; set; }
        public Action<T> OnEvent { get; set; }

        public void UnRegister()
        {
            TypeEventSystem.UnRegister<T>(OnEvent);

            TypeEventSystem = null;

            OnEvent = null;
        }
    }

    /// <summary>
    /// 注销的触发器
    /// </summary>
    public class UnRegisterOnDestoryTrigger : MonoBehaviour
    {
        private HashSet<IUnRegister> unRegisters = new HashSet<IUnRegister>();

        //添加需要注销的事件
        public void AddUnRegister(IUnRegister unRegister)
        {
            unRegisters.Add(unRegister);
        }

        //在该obj销毁时注销哈希表内的事件
        private void OnDestroy()
        {
            foreach (var unRegister in unRegisters)
            {
                unRegister.UnRegister();//调用事件的销毁方法
            }

            unRegisters.Clear();
        }
    }

    /// <summary>
    /// 注销触发器的简化使用
    /// </summary>
    public static class UnRegisterExension
    {
        //拓展方法
        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestoryTrigger>();
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterOnDestoryTrigger>();
            }

            trigger.AddUnRegister(unRegister);
        }
    }

    public class TypeEventSystem : ITypeEventSystem
    {
        private Dictionary<Type, IRegistrations> eventRegistration = new Dictionary<Type, IRegistrations>();
        public interface IRegistrations
        {

        }

        public class Registrations<T> : IRegistrations
        {
            /// <summary>
            /// 委托可以一对多注册
            /// </summary>
            public Action<T> OnEvent = obj => { };
        }

        public void Send<T>(T e)
        {
            var type = typeof(T);
            IRegistrations registrations;

            if (eventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>)?.OnEvent.Invoke(e);
            }
        }

        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }


        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;

            if (eventRegistration.TryGetValue(type, out registrations))
            {

            }
            else
            {
                registrations = new Registrations<T>();
                eventRegistration.Add(type, registrations);
            }
            (registrations as Registrations<T>).OnEvent += onEvent;

            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };

        }
        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;

            if (eventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
    #endregion

    #region IOC
    /// <summary>
    /// IOC容器
    /// </summary>
    public class IOCContainer
    {
        private Dictionary<Type, object> instances = new Dictionary<Type, object>();

        /// <summary>
        /// 往字典内添加实例
        /// type 为 key
        /// 实例 为 value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public void Regiset<T>(T instance)
        {
            var key = typeof(T);

            //如果包含
            if (instances.ContainsKey(key))
            {
                instances[key] = instance;
            }
            else
            {
                instances.Add(key, instance);
            }
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : class
        {
            var key = typeof(T);


            if (instances.TryGetValue(key, out var retInstance))
            {
                return retInstance as T;
            }

            return null;
        }
    }
    #endregion

    #region Bindableproperty
    public class BindableProperty<T>
    {
        public BindableProperty(T defaultValue = default)
        {
            mValue = defaultValue;
        }
        private T mValue = default(T);
        public T Value
        {
            get => mValue;
            set
            {
                if (value == null && mValue == null) return;
                if (value != null && value.Equals(mValue)) return;

                mValue = value;
                OnValueChanged?.Invoke(value);
            }
        }

        public Action<T> OnValueChanged = (v) => { };

        public IUnRegister Register(Action<T> onValueChanged)
        {
            OnValueChanged += onValueChanged;
            return new BindablePropertyUnRegister<T>
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }

        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged?.Invoke(mValue);
            return Register(onValueChanged);
        }

        public static implicit operator T(BindableProperty<T> property)
        {
            return property.Value;
        }
        public void UnRegisterOnValueChanged(Action<T> onValueChanged)
        {
            OnValueChanged -= onValueChanged;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class BindablePropertyUnRegister<T> : IUnRegister
    {
        public BindableProperty<T> BindableProperty { get; set; }
        public Action<T> OnValueChanged { get; set; }

        public void UnRegister()
        {
            BindableProperty.UnRegisterOnValueChanged(OnValueChanged);
        }
    }
    #endregion

    #endregion



}

