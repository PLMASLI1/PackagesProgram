using Caliburn.Micro;
using PackagesProgram.Helpers;
using PackagesProgram.Models;
using PackagesProgram.Properties;

namespace PackagesProgram.ViewModels
{
    public class PackagesViewModel : Screen
    {
        private readonly DatabaseOperation databaseOperation;
        private BindableCollection<PackageModel> _packages = new BindableCollection<PackageModel>();
        private int startRange;
        private int endRange;
        private string message;
        private bool startButtonEnabled;
        private int randomId;

        public PackagesViewModel()
        {
            this.databaseOperation = new DatabaseOperation();
        }

        public BindableCollection<PackageModel> Packages
        {
            get
            {
                _packages.Clear();

                foreach (var id in databaseOperation.GetIdCollectionFromPackagesTable())
                    _packages.Add(new PackageModel { PackageId = id });

                return _packages;
            }
            set
            {
                _packages = value;
                NotifyOfPropertyChange(() => Packages);
            }
        }

        public int StartRange
        {
            get
            {
                return startRange;
            }
            set
            {
                CanRandomId();
                startRange = value;
                NotifyOfPropertyChange(() => StartRange);
                NotifyOfPropertyChange(() => StartButtonEnabled);
            }
        }

        public int EndRange
        {
            get
            {
                return endRange;
            }
            set
            {
                CanRandomId();
                endRange = value;
                NotifyOfPropertyChange(() => EndRange);
                NotifyOfPropertyChange(() => StartButtonEnabled);
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }
        public int RandomId
        {
            get
            {
                return randomId;
            }
            set
            {
                randomId = value;
                NotifyOfPropertyChange(() => randomId);
            }
        }

        public bool StartButtonEnabled
        {
            get
            {
                startButtonEnabled = CanRandomId();
                return startButtonEnabled;
            }
            set
            {
                startButtonEnabled = value;
                NotifyOfPropertyChange(() => Message);
                NotifyOfPropertyChange(() => StartButtonEnabled);
            }
        }

        public void SearchAndAddCommand()
        {
            var randomValue = databaseOperation.RandomIdFromTheRange(startRange, endRange);
            randomId = randomValue;
            NotifyOfPropertyChange(() => randomId);

            if (randomValue > 0 && !databaseOperation.CheckIfIdExist(randomValue))
            {
                databaseOperation.InsertIdToPackagesTable(randomValue);
                message = Resources.SuccessfulInsertMessage;
                NotifyOfPropertyChange(() => Packages);
            }
            else
            {
                message = randomValue < 0 ? Resources.IncorrectRangeMessage
                    : string.Format(Resources.IncorrectNumberMessage, randomId);
            }

            NotifyOfPropertyChange(() => Message);
        }

        public bool CanRandomId()
        {
            if (startRange >= endRange)
            {
                message = Resources.StartAndFinalValueAreIncorrectMessage;
                NotifyOfPropertyChange(() => Message);

                return false;
            }

            message = string.Empty;
            NotifyOfPropertyChange(() => Message);

            return true;
        }
    }
}