using System.Linq;
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

        public PackagesViewModel()
        {
            this.databaseOperation = new DatabaseOperation();

            var packageModels = databaseOperation.GetIdCollectionFromPackagesTable();

            var packagesModel = new PackagesModel();
            //packagesModel.PackageModels.AddRange(packageModels);
            Packages.Add(packagesModel);
        }

        public BindableCollection<PackagesModel> Packages
        {
            get
            {
                return _packages;
            }
            set
            {
                _packages = value;
                //NotifyOfPropertyChange(() => PackageId);
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

        public void SearchAndAddCommand()
        {
            var randomId = databaseOperation.RandomIdFromTheRange(startRange, endRange);
            var isRandomIdExist = databaseOperation.CheckIfIdExist(randomId);
            if (!isRandomIdExist)
                databaseOperation.InsertIdToPackagesTable(randomId);
            else
                message = $"Wylosowano: {randomId}, ktore istnieje w bazie - sprobuj jeszcze raz.";
        }

        public void CanRandomId()
        {
            if (startRange > endRange)
                message = "Start range position is lower then end range.";
        }
    }
}