using System;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using DND.UI.Screens;

namespace DND.UI
{
    static class ViewTypeResolver
    {
        static Func<Type, DependencyObject, object, Type> _baseLocateTypeForModelType;

        static readonly string[] AlternateViewNames = new[] {"StandardView", "DefaultView"};

        public static void RegisterBackupFunction(Func<Type, DependencyObject, object, Type> func)
        {
            _baseLocateTypeForModelType = func;
        }

        public static Type LocateTypeForModelType(Type modelType, DependencyObject location, object context)
        {
            Type viewType;

            if (IsDesignViewModel(modelType))
            {
                var modelName = $"DND.UI.Domain.{modelType.Name.Substring("Design".Length)}";
                viewType = Get(modelType, modelName, context);

                if (viewType != null)
                    return viewType;
                
                modelName = $"DND.UI.Components.{modelType.Name.Substring("Design".Length)}";
                viewType = Get(modelType, modelName, context);
            }
            else
            {
                viewType = Get(modelType, modelType.FullName, context);
                if (viewType != null)
                    return viewType;

                viewType = Get(modelType, $"DND.UI.Components.{modelType.Name}", context);
            }

            return viewType ?? _baseLocateTypeForModelType?.Invoke(modelType, location, context);
        }

        static Type Get(Type modelType, string modelName, object context)
        {
            var viewName = GetViewName(modelName, context as string);
            var viewType = GetViewType(modelType, viewName);
            if (viewType == null)
            {
                for (int i = 0; i < AlternateViewNames.Length; i++)
                {
                    viewName = GetViewName(modelName, AlternateViewNames[i]);
                    viewType = GetViewType(modelType, viewName);
                    if (viewType != null)
                        return viewType;
                }
            }

            return viewType;
        }

        static bool IsDesignViewModel(Type modelType)
        {
            return modelType.Name.StartsWith("Design");
        }
        
        static Type GetViewType(Type modelType, string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                return null;

            return modelType.Assembly.DefinedTypes.FirstOrDefault(p => p.FullName == viewName);
        }

        static string GetViewName(string viewModelFullName, string context)
        {
            if (!viewModelFullName.EndsWith("ViewModel"))
                return null;

            if (string.IsNullOrEmpty(context))
                return viewModelFullName.Substring(0, viewModelFullName.Length - "Model".Length);

            return $"{viewModelFullName.Substring(0, viewModelFullName.Length - "ViewModel".Length)}.{context}";
        }
    }

    public class Bootstrapper : BootstrapperBase
    {
        

        public Bootstrapper()
        {
            ViewTypeResolver.RegisterBackupFunction(ViewLocator.LocateTypeForModelType);
            ViewLocator.LocateTypeForModelType = ViewTypeResolver.LocateTypeForModelType;
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            Startup.OnStartup(e);
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
