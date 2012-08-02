using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace SaidWhat.Common.Infrastructure
{
    /// <summary>
    /// Provides an IOC Container without all the unnecessary bullshit of Unity/Spring/Ninject/etc
    /// </summary>
    public class Container
    {
        /////////////////////////////////////////////////////////////////////
        /// CONSTRUCTORS ////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        #region Constructors

        public Container()
        {
            instanceBindings = new Dictionary<Type, object>();
            typeConstructors = new Dictionary<Type, Func<object>>();
        }

        #endregion

        /////////////////////////////////////////////////////////////////////
        /// PROPERTIES //////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        #region Properties

        Dictionary<Type, object> instanceBindings { get; set; }
        Dictionary<Type, Func<object>> typeConstructors { get; set; }
        static Container Singleton { get; set; }

        /// <summary>
        /// Current instance of the Container
        /// </summary>
        public static Container Instance
        {
            get
            {
                if (Singleton == null)
                    Singleton = new Container();
                return Singleton;
            }
        }

        #endregion

        /////////////////////////////////////////////////////////////////////
        /// METHODS /////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        /// Binds a single instance to the type
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="instance">The instance to bind to the type</param>
        public void Set<T>(T instance)
        {
            instanceBindings.Add(typeof(T), instance);
        }

        /// <summary>
        /// Binds a type for new instance creation
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="type">The type</param>
        public void Set<T>(Type type)            
        {            
            var newExpressions = Expression.New(type);
            var lambdaExpression = Expression.Lambda(newExpressions);
            var compiled = (Func<object>)lambdaExpression.Compile();
            typeConstructors.Add(typeof(T), compiled);
        }

        /// <summary>
        /// Gets an instance
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>An instance</returns>
        public T Get<T>()
        {            
            var type = typeof(T);

            if(instanceBindings.ContainsKey(type))
                return GetInstance<T>();
            else if (typeConstructors.ContainsKey(type))
                return GetType<T>();
            else            
                throw new ApplicationException("Binding for type " + typeof(T) + " could not be found.");            
        }

        T GetInstance<T>()
        {
            var type = typeof(T);            
            return (T)instanceBindings[type];            
        }

        T GetType<T>()
        {
            var type = typeof(T);            
            var func = typeConstructors[type];
            return (T)func();
        }

        #endregion
    }
}
