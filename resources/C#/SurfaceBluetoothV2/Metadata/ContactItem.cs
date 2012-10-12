using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareLab.Sys.BusinessObjects;

namespace SurfaceBluetoothV2.Metadata
{
    /// <summary>
    /// This Class Contains informations about Contact
    /// </summary>
    [Serializable]
    public class ContactItem : BaseBusinessEntity
    {
        #region ctor
        public ContactItem(string firstName, string lastName, string mobileTelephoneNumber, string emailAddress, string pictureUri)
        {
            _firstName = firstName;
            _lastName = lastName;
            _mobileTelephoneNumber = mobileTelephoneNumber;
            _emailAddress = emailAddress;
            _pictureUri = pictureUri;
            SetEntityState(true);
            UpdateValidationState();
        }
        #endregion

        #region private properties
        private string _firstName;
        private string _lastName;
        private string _mobileTelephoneNumber;
        private string _emailAddress;
        private string _pictureUri;
        #endregion

        #region public properties
        public string FirstName 
        {
            get { return _firstName; }
            set
            {
                string propertyName = "FirstName";
                _firstName = value;

                SetEntityState(BusinessEntityState.Modified, propertyName);
                UpdateValidationState();
            }
        }
        
        public string LastName
        {
            get { return _lastName; }
            set
            {
                string propertyName = "LastName";
                _lastName = value;

                SetEntityState(BusinessEntityState.Modified, propertyName);
                UpdateValidationState();
            }
        }

        public string MobileTelephoneNumber
        {
            get { return _mobileTelephoneNumber; }
            set
            {
                string propertyName = "MobileTelephoneNumber";
                _mobileTelephoneNumber = value;

                SetEntityState(BusinessEntityState.Modified, propertyName);
                UpdateValidationState();
            }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                string propertyName = "EmailAddress";
                _emailAddress = value;

                SetEntityState(BusinessEntityState.Modified, propertyName);
                UpdateValidationState();
            }
        }

        public string PictureUri
        {
            get { return _pictureUri; }
            set
            {
                string propertyName = "PictureUri";
                _pictureUri = value;

                SetEntityState(BusinessEntityState.Modified, propertyName);
                UpdateValidationState();
            }
        }

        

        #endregion

        #region override from abstract
        public override bool SaveItem()
        {
            throw new NotImplementedException();
        }

        public override string Error
        {
            get
            {
                //StringBuilder sb = new StringBuilder();
                //ConfigurationValidatorFactory validatoryFactory = EnterpriseLibraryContainer.Current.GetInstance<ConfigurationValidatorFactory>();

                //Microsoft.Practices.EnterpriseLibrary.Validation.Validator<Products > validator = validatoryFactory.CreateValidator<Products >("Ruleset.Products");

                //ValidationResults results = validator.Validate(this);
                //foreach (Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult result in results)
                //{
                //    sb.Append(result.Message);
                //}

                //foreach (Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult result in _validationResults)
                //{
                //    sb.Append(result.Message);
                //}
                //return sb.ToString();
                return null;
            }
        }

        public override string this[string columnName]
        {
            get
            {
                //string retVal = string.Empty;
                //ConfigurationValidatorFactory validatoryFactory = EnterpriseLibraryContainer.Current.GetInstance<ConfigurationValidatorFactory>();
                //Microsoft.Practices.EnterpriseLibrary.Validation.Validator<Products> validator = validatoryFactory.CreateValidator<Products >("Ruleset.Products");

                //ValidationResults results = validator.Validate(this);

                //if (results != null)
                //{
                //    var columnResults = results.FirstOrDefault<Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult>(x => string.Compare(x.Tag, columnName, true) == 0);
                //    retVal = columnResults != null ? columnResults.Message : string.Empty;
                //}

                //return retVal;

                //ValidationResults filteredResults = new ValidationResults();

                //filteredResults.AddAllResults(from result in _validationResults
                //                              where result.Key == columnName
                //                              select result);
                //return !filteredResults.IsValid ?
                //    filteredResults.First().Message : null;
                return null;

            }
        }
        #endregion

        private void UpdateValidationState()
        {
            //if (_validator != null)
            //    _validationResults = _validator.Validate(this);
            //if (SetupCommand != null)
            //    SetupCommand.RaiseCanExecuteChanged();
        }
        
    }
}
