using System;
using System.ComponentModel;
using System.Windows;
using SoftwareLab.Sys.BusinessObjects;

namespace SurfaceBluetoothV2.Metadata
{
    /// <summary>
    /// This Class Contains informations about ContentItem
    /// </summary>
    [Serializable]
    public class ContentItem : BaseBusinessEntity
    {
        public const string CONTENT_IMAGE = "Images";
        public const string CONTENT_CONTACT = "Contacts";
        public const string CONTENT_RING = "Ring Tones";

        #region private fields
        private String _thumb;
        private String _media;
        private String _mediaType;
        private ContactItem _contact;
        #endregion

        #region public properties

        /// <summary>
        /// Gets or sets Value of Image
        /// </summary>
        public String Thumb
        {
            get { return _thumb; }
            set
            {
                if (_thumb != value)
                {
                    string propertyName = "Image";
                    _thumb = value;

                    // Validation
                    //ValidationInstance.ValidateClear(propertyName);
                    //ValidationInstance.ValidateRequired(propertyName, value.ToString());

                    SetEntityState(BusinessEntityState.Modified, propertyName);
                    UpdateValidationState();
                }
            }
        }

        /// <summary>
        /// Gets or sets Value of Media
        /// </summary>
        public String Media
        {
            get { return _media; }
            set
            {
                string propertyName = "Media";
                _media = value;

                // Validation
                //ValidationInstance.ValidateClear(propertyName);
                //ValidationInstance.ValidateRequired(propertyName, value.ToString());

                SetEntityState(BusinessEntityState.Modified, propertyName);
                UpdateValidationState();
            }
        }

        /// <summary>
        /// Gets or sets Value of MediaTypeName
        /// </summary>
        public String MediaType
        {
            get { return _mediaType; }
            set
            {
                string propertyName = "MediaType";
                _mediaType = value;

                // Validation
                //ValidationInstance.ValidateClear(propertyName);
                //ValidationInstance.ValidateRequired(propertyName, value.ToString());

                SetEntityState(BusinessEntityState.Modified, propertyName);
                UpdateValidationState();
            }
        }

        public string ObexContentType
        {
            get
            {
                string retVal = string.Empty;
                switch (_mediaType)
                { 
                    case CONTENT_IMAGE:
                        retVal = InTheHand.Net.Mime.MediaTypeNames.Image.Jpg;
                        break;
                    case CONTENT_CONTACT:
                        retVal = InTheHand.Net.Mime.MediaTypeNames.Text.vCard;
                        break;
                    case CONTENT_RING:
                        retVal = "audio/mp3";
                        break;
                }
                return retVal;
            }
        }

        public string ObexFileName
        {
            get
            {
                string retVal = string.Empty;
                if (_mediaType == CONTENT_IMAGE || _mediaType == CONTENT_RING)
                {
                    retVal = System.IO.Path.GetFileName(_mediaType);
                }
                else if (_mediaType == CONTENT_CONTACT)
                {
                    if (_contact != null)
                        retVal = string.Format("{0}{1}.vcf", _contact.FirstName, _contact.LastName);
                }
                return retVal;
            }
        }

        public ContactItem Contact
        {
            get { return _contact; }
            set
            {
                string propertyName = "Contact";
                _contact = value;

                _contact.PropertyChanged += new PropertyChangedEventHandler(Contact_PropertyChanged);

                SetEntityState(BusinessEntityState.Modified, propertyName);
                UpdateValidationState();
            }
        }

        void Contact_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetEntityState(BusinessEntityState.Modified, "Contact");
        }

        
        #endregion

        #region costructors
        /// <summary>
        /// Default Costructor
        /// </summary>
        public ContentItem()
        {
            _thumb = string.Empty;
            _media = string.Empty;
            _mediaType = string.Empty;

            SetEntityState(true);
            UpdateValidationState();
            //ConfigurationValidatorFactory validatoryFactory = EnterpriseLibraryContainer.Current.GetInstance<ConfigurationValidatorFactory>();
            //_validator = validatoryFactory.CreateValidator<Product>("Ruleset.Products");
        }

         //<summary>
         //Overload of the Default Costructor
         //</summary>
        public ContentItem(string image, string media, string medianame, ContactItem contact)
        {
            _thumb = image;
            _media = media;
            _mediaType = medianame;
            _contact = contact;
            if (_contact != null)
                _contact.PropertyChanged += new PropertyChangedEventHandler(Contact_PropertyChanged);
            SetEntityState(true);
            UpdateValidationState();
        }

        public ContentItem(string image, string media, string medianame)
        {
            _thumb = image;
            _media = media;
            _mediaType = medianame;
            SetEntityState(true);
            UpdateValidationState();
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
