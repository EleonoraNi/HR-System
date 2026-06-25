using System.Collections.ObjectModel;

namespace final_work
{
    class EncryptionHelper
    {
        //Class that encrypts and decrypts parameters in ObservableCollection Employee
        public static ObservableCollection<Employee> EncryptEmployees(ObservableCollection<Employee> employees)
        {
            return new ObservableCollection<Employee>(employees.Select(employee => new Employee
            {
                FirstNames = Encryption.Encrypt(employee.FirstNames),
                LastName = Encryption.Encrypt(employee.LastName),
                NickName = Encryption.Encrypt(employee.NickName),
                SSN = Encryption.Encrypt(employee.SSN),
                StreetAddress = Encryption.Encrypt(employee.StreetAddress),
                POCode = Encryption.Encrypt(employee.POCode),
                PostOffice = Encryption.Encrypt(employee.PostOffice),
                StartDate = employee.StartDate,
                EndDate = employee.EndDate,
                Title = Encryption.Encrypt(employee.Title),
                Department = Encryption.Encrypt(employee.Department)
            }));
        }
        public static ObservableCollection<Employee> DecryptEmployees(ObservableCollection<Employee> employees)
        {
            return new ObservableCollection<Employee>(employees.Select(employee => new Employee
            {
                FirstNames = Encryption.Decrypt(employee.FirstNames),
                LastName = Encryption.Decrypt(employee.LastName),
                NickName = Encryption.Decrypt(employee.NickName),
                SSN = Encryption.Decrypt(employee.SSN),
                StreetAddress = Encryption.Decrypt(employee.StreetAddress),
                POCode = Encryption.Decrypt(employee.POCode),
                PostOffice = Encryption.Decrypt(employee.PostOffice),
                StartDate = employee.StartDate,
                EndDate = employee.EndDate,
                Title = Encryption.Decrypt(employee.Title),
                Department = Encryption.Decrypt(employee.Department)
            }));
        }
    }
}