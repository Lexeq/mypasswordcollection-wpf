using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MPC.ViewModels
{
    class DataErrorNotifyingViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, HashSet<string>> errors;

        public DataErrorNotifyingViewModel()
        {
            errors = new Dictionary<string, HashSet<string>>();
        }

        public bool HasErrors => errors.Values.FirstOrDefault(l => l.Count > 0) != null;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (!errors.TryGetValue(propertyName, out var errorsForProperty))
            {
                yield break;
            }
            else
            {
                foreach (var item in errorsForProperty)
                {
                    yield return item;
                }
            }
        }

        protected virtual void AddPropertyError(string error, [CallerMemberName]string propName = "")
        {
            propName = propName ?? string.Empty;

            if (!errors.TryGetValue(propName, out var propErrors))
                errors[propName] = propErrors = new HashSet<string>();

            if (propErrors.Add(error))
            {
                OnErrorsChanged(propName);
            }
        }

        protected virtual void ClearPropertyErrors([CallerMemberName]string propName = "")
        {
            propName = propName ?? string.Empty;

            if (errors.Remove(propName))
            {
                OnErrorsChanged(propName);
            }
        }

        protected virtual void ClearAllErrors()
        {
            if (errors.Count > 0)
            {
                var keys = errors.Keys.ToArray();
                errors.Clear();
                foreach (var key in keys)
                {
                    OnErrorsChanged(key);
                }
            }
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            var handler = ErrorsChanged;
            handler?.Invoke(this, e);
        }

        private void OnErrorsChanged(string propName)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnErrorsChanged(new DataErrorsChangedEventArgs(propName));
        }
    }
}