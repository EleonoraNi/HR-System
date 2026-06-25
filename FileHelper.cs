using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace final_work
{
    class FileHelper
    {
        //Class which serialize to file and and deserialiaze from file 
        private const string LogFilePath= "c:\\HR-System\\Employees.txt";

        public static void SaveToFile(ObservableCollection<Employee> employees)
        {
            try
            {
                ObservableCollection<Employee> encryptedEmployees = new ObservableCollection<Employee>(EncryptionHelper.EncryptEmployees(employees));
                string jsonString = JsonSerializer.Serialize(encryptedEmployees.ToList());
                File.WriteAllText(LogFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Tiedoston tallennus epäonnistui: {ex.Message}");
            }
        }
        public static ObservableCollection<Employee> ReadFromFile()
        {
            try
            {
                string jsonString = File.ReadAllText(LogFilePath);
                ObservableCollection<Employee> encryptedEmployees = JsonSerializer.Deserialize<ObservableCollection<Employee>>(jsonString);
                ObservableCollection<Employee> decryptedEmployees = new ObservableCollection<Employee>(EncryptionHelper.DecryptEmployees(encryptedEmployees));
                return decryptedEmployees;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Tiedoston lukeminen epäonnistui: {ex.Message}");
                return null;
            }
        }
    }
}