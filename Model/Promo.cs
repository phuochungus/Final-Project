//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _4NH_HAO_Coffee_Shop.Model
{
<<<<<<< HEAD
    using _4NH_HAO_Coffee_Shop.ViewModel;
    using System;
    using System.Collections.Generic;
    
    public partial class Promo : BaseViewModel
=======
    using System;
    using System.Collections.Generic;
    
    public partial class Promo
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Promo()
        {
            this.Bills = new HashSet<Bill>();
        }
<<<<<<< HEAD
        private string _Id;
        public string Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                OnPropertyChanged();
            }
        }
        private string _DisplayName;
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
                OnPropertyChanged();
            }
        }
        private string _Script;
        public string Script
        {
            get
            {
                return _Script;
            }
            set
            {
                _Script = value;
                OnPropertyChanged();
            }
        }
        private Nullable<System.DateTime> _StartTime;
        public Nullable<System.DateTime> StartTime
        {
            get
            {
                return _StartTime;
            }
            set
            {
                _StartTime = value;
                OnPropertyChanged();
            }
        }
        private Nullable<System.DateTime> _EndTime;
        public Nullable<System.DateTime> EndTime
        {
            get
            {
                return _EndTime;
            }
            set
            {
                _EndTime = value;
                OnPropertyChanged();
            }
        }

=======
    
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Script { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
    
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
