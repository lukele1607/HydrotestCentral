using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HydrotestCentral.Infrastructure
{
    public abstract class PropertyChangedNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { m_propertyChanged += value; }
            remove { m_propertyChanged -= value; }
        }

        protected void TriggerPropertyChangedEvent (params string[] properties)
        {
            var propertyChangedHandler = m_propertyChanged;
            if (properties != null && properties.Length != 0 && propertyChangedHandler != null)
            {
                foreach (string property in properties)
                {
                    propertyChangedHandler (this, new PropertyChangedEventArgs (property));
                }
            }
        }

        protected void TriggerPropertyChangedEvent (IEnumerable<string> properties)
        {
            var propertyChangedHandler = m_propertyChanged;
            if (properties != null && propertyChangedHandler != null)
            {
                foreach (string property in properties)
                {
                    propertyChangedHandler (this, new PropertyChangedEventArgs (property));
                }
            }
        }

        protected void OnPropertyChanged ([CallerMemberName] string propertyName = null)
        {
            m_propertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }

        #region private fields

        private event PropertyChangedEventHandler m_propertyChanged;

        #endregion private fields
    }
}
