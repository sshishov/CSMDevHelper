﻿using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System;

namespace CSMDevHelper
{

    [Serializable]
    public class CustomBindingList<T> : BindingList<T>
    {

        private T removed_item;

        protected override void RemoveItem(int index)
        {
            this.removed_item = this[index];
            base.RemoveItem(index);
            this.removed_item = default(T);
        }

        public T RemovedItem
        {
            get{return this.removed_item;}
        }
    }
}
