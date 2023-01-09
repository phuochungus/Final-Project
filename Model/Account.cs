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
    using _4NH_HAO_Coffee_Shop.ViewModel;
    using System;
    using System.Collections.Generic;

    public partial class Account : BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            this.Account1 = new HashSet<Account>();
            this.Bills = new HashSet<Bill>();
        }

        private string _Id;
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private string _Displayname;
        public string DisplayName { get => _Displayname; set { _Displayname = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _PhonenNumber;
        public string PhoneNumber { get => _PhonenNumber; set { _PhonenNumber = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        private string _AccountType;
        public string AccountType { get => _AccountType; set { _AccountType = value; OnPropertyChanged(); } }
        private string _ImageURL { get; set; }
        public string ImageURL { get => _ImageURL; set { _ImageURL = value; OnPropertyChanged(); } }

        private string _ManageBy;
        public string ManagedBy { get => _ManageBy; set { _ManageBy = value; OnPropertyChanged(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Account1 { get; set; }
        public virtual Account Account2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
