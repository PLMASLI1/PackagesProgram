using Caliburn.Micro;
using PackagesProgram.Helpers;
using PackagesProgram.Models;

namespace PackagesProgram.ViewModels
{
    public class PackagesViewModel : Screen
    {
        private readonly DatabaseOperation databaseOperation;
        private BindableCollection<PackagesModel> _packages = new BindableCollection<PackagesModel>();
        private int id;
        private int startRange;
        private int endRange;
        private string message;
        private bool startButtonEnabled;

        public PackagesViewModel()
        {
            this.databaseOperation = new DatabaseOperation();
        }

        public BindableCollection<PackagesModel> Packages
        {
            get
            {
                var idsFromDatabase = databaseOperation.GetIdCollectionFromPackagesTable();
                var packagesModel = new PackagesModel();

                foreach (var id in idsFromDatabase)
                    packagesModel.PackageModels.Add(new PackageModel { PackageId = id });

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
            var randomId = databaseOperation.RandomIdFromTheRange(startRange, endRange);
            var isRandomIdExist = databaseOperation.CheckIfIdExist(randomId);
            if (!isRandomIdExist)
            {
                databaseOperation.InsertIdToPackagesTable(randomId);
                NotifyOfPropertyChange(() => Packages);
            }
            else
            {
                message = $"Wylosowano: {randomId}, ktore istnieje w bazie - sprobuj jeszcze raz.";
                NotifyOfPropertyChange(() => Message);
            }
        }

        public bool CanRandomId()
        {
            if (startRange > endRange)
            {
                message = "Start range position is lower then end range.";
                NotifyOfPropertyChange(() => Message);

                return false;
            }

            return true;
        }
    }
}