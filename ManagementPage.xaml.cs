using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;

namespace final_work;

public partial class ManagementPage : ContentPage
{
    ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
    private Employee selectedEmployee;
    Dictionary<string, string> postalCodeToPostOffice = new Dictionary<string, string>();
    private bool leaveWithoutSaving = false;
    public ManagementPage(Employee employee)
    {
        //Defined Events on page
        InitializeComponent();
        selectedEmployee = employee;
        UpdateEntryFields();
        entryFirstnames.Unfocused += EntryUnfocused;
        entryLastname.Unfocused += EntryUnfocused;
        entryNickName.Unfocused += EntryUnfocused;
        entrySSN.Unfocused += EntryUnfocused;
        entryStreetAddress.Unfocused += EntryUnfocused;
        entryPOCode.Unfocused += EntryUnfocused;
        entryPostOffice.Unfocused += EntryUnfocused;
        entryStartDate.Unfocused += EntryUnfocused;
        entryEndDate.Unfocused += EntryUnfocused;
        entryTitle.Unfocused += EntryUnfocused;
        entryDepartment.Unfocused += EntryUnfocused;
        LoadDataIntoDictionary();
        entryPOCode.TextChanged += EntryPostalCodeTextChanged;
    }
    private void UpdateEntryFields()
    {
        //Updates Entry fields based on selection
        if (selectedEmployee != null)
        {
            entryFirstnames.Text = selectedEmployee.FirstNames;
            entryLastname.Text = selectedEmployee.LastName;
            entryNickName.Text = selectedEmployee.NickName;
            entrySSN.Text = selectedEmployee.SSN;
            entryStreetAddress.Text = selectedEmployee.StreetAddress;
            entryPOCode.Text = selectedEmployee.POCode;
            entryPostOffice.Text = selectedEmployee.PostOffice;
            entryStartDate.Text = selectedEmployee.StartDate;
            entryEndDate.Text = selectedEmployee.EndDate;
            entryTitle.Text = selectedEmployee.Title;
            entryDepartment.Text = selectedEmployee.Department;
        }
    }
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            // Checks every entry field before saving information to file
            if (!CheckAllInformation())
            {
                //if fails alerts user
                await DisplayAlert("Tarkista tiedot", "Täytä pakolliset kentät ennen tallentamista.", "OK");
                return;
            }
            //Gets current information from the ObservableCollection Employee
            ObservableCollection<Employee> existingEmployees = FileHelper.ReadFromFile() ?? new ObservableCollection<Employee>();
            
            // Collects texts from entrys
            string firstnames = entryFirstnames.Text;
            string lastname = entryLastname.Text;
            string nickname = entryNickName.Text;
            string ssn = entrySSN.Text;
            string streetaddress = entryStreetAddress.Text;
            string pocode = entryPOCode.Text;
            string postoffice = entryPostOffice.Text;
            string startdate = entryStartDate.Text;
            string enddate = entryEndDate.Text;
            string title = entryTitle.Text;
            string department = entryDepartment.Text;

            //Creates new Employee and adds it to Employee
            Employee newEmployee = new Employee
            {
                FirstNames = firstnames,
                LastName = lastname,
                NickName = nickname,
                SSN = ssn,
                StreetAddress = streetaddress,
                POCode = pocode,
                PostOffice = postoffice,
                StartDate = startdate,
                EndDate = enddate,
                Title = title,
                Department = department
            };
            existingEmployees.Add(newEmployee);

            //Adds POCode and Postoffice to dictionary
            if (!string.IsNullOrEmpty(pocode) && !string.IsNullOrEmpty(postoffice))
            {
                //Cheks if dictionary allready includes given POCode and PostOffice
                if (!postalCodeToPostOffice.ContainsKey(pocode))
                {
                    postalCodeToPostOffice.Add(pocode, postoffice);
                }
            }
            //writes logdetail, saves information, alerts user, returns to HRSystemPage
            Log.LogDetail($"Tiedot tallennettu: {newEmployee.FirstNames} {newEmployee.LastName} Alkamispäivä {newEmployee.StartDate} Loppumispäivä {newEmployee.EndDate}");
            FileHelper.SaveToFile(existingEmployees);
            await DisplayAlert("Tallennus", "Tiedot tallennettu onnistuneesti!", "OK");
            await Navigation.PushAsync(new HRSystemPage());
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"OnSaveClicked failed: {ex.Message}");
        }
    }
    private bool CheckAllInformation()
    {
        //Checks that all necessary entrys are filled
        var entryFields = GetEntryFields();

        foreach (var entry in entryFields)
        {
            if (entry.BackgroundColor == Colors.Red)
            {
                // If the background color is red, it means there's an issue with the entry
                ToolTipProperties.SetText(entry, "Korjaa virheelliset tiedot");
                return false;
            }
            else if (entry != entryEndDate && string.IsNullOrEmpty(entry.Text))
            {
                //Not filled background is red
                entry.BackgroundColor = Colors.Red;
                ToolTipProperties.SetText(entry, "Kenttä ei voi olla tyhjä");
                return false;
            }
            else
            {
                entry.BackgroundColor = Colors.LightSteelBlue;
                ToolTipProperties.SetText(entry, null);
            }
        }
        return true;
    }
    private IEnumerable<Entry> GetEntryFields()
    {
        //Makes list of Entry fields
        return new List<Entry>
        {
        entryFirstnames,
        entryLastname,
        entryNickName,
        entrySSN,
        entryStreetAddress,
        entryPOCode,
        entryPostOffice,
        entryStartDate,
        entryEndDate,
        entryTitle,
        entryDepartment
    };
    }
    private async void EntryUnfocused(object sender, FocusEventArgs e)
    {
        //Entrys are check as their are unfocused
        Entry entry = (Entry)sender;
        //Check if entrys are null or empty, except entryEndDate which is optional
        if (string.IsNullOrEmpty(entry.Text) && entry != entryEndDate)
        {
            entry.BackgroundColor = Colors.Red;
            ToolTipProperties.SetText(entry, "Kenttä ei voi olla tyhjä");
            return;
        }
        else
        {
            entry.BackgroundColor = Colors.LightSteelBlue;
            ToolTipProperties.SetText(entry, null);
        }
        //Checks that user uses letters to write names
        if (entry == entryFirstnames || entry == entryLastname)
        {
            string nameToCheck = entry.Text;
            if (nameToCheck.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                ToolTipProperties.SetText(entry, null);
            }
            else
            {
                entry.BackgroundColor = Colors.Red;
                ToolTipProperties.SetText(entry, "Käytä kirjaimia nimissä");
                return;
            }
        }
        // Checks that Nickname is one of the given firstnames
        if (entry == entryNickName)
        {
            string[] firstNames = entryFirstnames.Text.Split(' ');

            if (!firstNames.Any(firstName => string.Equals(firstName, entryNickName.Text, StringComparison.OrdinalIgnoreCase)))
            {
                entry.BackgroundColor = Colors.Red;
                ToolTipProperties.SetText(entry, "Kutsumanimen on oltava jokin etunimistä");
                return;
            }
        }
        // Checks that Social Security Number is correct
        if (entry == entrySSN)
        {
            // Gets current information from the ObservableCollection Employee
            ObservableCollection<Employee> existingEmployees = FileHelper.ReadFromFile() ?? new ObservableCollection<Employee>();
            string ssn = entrySSN.Text;
            SSNCheck ssnChecker = new SSNCheck();
            string checkResult = ssnChecker.CheckSSN(ssn);

            if (checkResult != "oikein")
            {
                entry.BackgroundColor = Colors.Red;
                ToolTipProperties.SetText(entry, $"Tarkista, että henkiötunnus on oikein");
                return;
            }

            // Check if the employee with the given SSN already exists
            Employee existingEmployee = existingEmployees.FirstOrDefault(emp => emp.SSN == ssn);

            if (existingEmployee != null)
            {
                // If the employee already exists, ask the user if they want to add a new employment relationship
                var result = await DisplayAlert("Tiedot jo olemassa",$"Henkilötunnus viittaa henkilöön {existingEmployee.FirstNames} {existingEmployee.LastName}, jolla on jo työsuhde. Haluatko tuoda henkilön tiedot ja lisätä uuden työsuhteen?", "Kyllä", "Ei");

                if (result)
                {
                    // If the user wants to add a new employment relationship, update the entry fields with existing employee's data
                    entryFirstnames.Text = existingEmployee.FirstNames;
                    entryLastname.Text = existingEmployee.LastName;
                    entryNickName.Text = existingEmployee.NickName;
                    entryStreetAddress.Text = existingEmployee.StreetAddress;
                    entryPOCode.Text = existingEmployee.POCode;
                    entryPostOffice.Text = existingEmployee.PostOffice;
                    entryTitle.Text = existingEmployee.Title;
                    entryDepartment.Text = existingEmployee.Department;

                    // You may want to handle the start date logic here based on your requirements
                    // Example: Set the start date for the new employment relationship as the day after the end date of the previous one
                    if (!string.IsNullOrEmpty(existingEmployee.EndDate) && DateTime.TryParseExact(existingEmployee.EndDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
                    {
                        DateTime nextDay = endDate.AddDays(1);
                        entryStartDate.Text = nextDay.ToString("dd.MM.yyyy");
                    }
                }
                else
                {
                    // If the user doesn't want to add a new employment relationship, clear the entrySSN field
                    entrySSN.Text = string.Empty;
                }
            }
        }
        //Cheks that dates are in correct format and that End date is bigger than startdate
        if (entry == entryStartDate || entry == entryEndDate)
        {
            if (string.IsNullOrEmpty(entry.Text))
            {
                entry.BackgroundColor = Colors.LightSteelBlue;
                ToolTipProperties.SetText(entry, null);
            }
            else if (DateTime.TryParseExact(entry.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate) &&
                     parsedDate.Month >= 1 && parsedDate.Month <= 12 && // Check if the parsed month is valid
                     parsedDate.Day >= 1 && parsedDate.Day <= DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month)) // Check if the parsed day is valid
            {
                entry.BackgroundColor = Colors.LightSteelBlue;
                ToolTipProperties.SetText(entry, null);

                if (entry == entryEndDate && !string.IsNullOrEmpty(entryStartDate.Text))
                {
                    if (DateTime.TryParseExact(entryStartDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate) &&
                        DateTime.TryParseExact(entryEndDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
                    {
                        if (endDate < startDate)
                        {
                            entryEndDate.BackgroundColor = Colors.Red;
                            ToolTipProperties.SetText(entryEndDate, "Päättymispäivä on oltava suurempi kuin alkamispäivä");
                        }
                        else
                        {
                            entryEndDate.BackgroundColor = Colors.LightSteelBlue;
                            ToolTipProperties.SetText(entryEndDate, null);
                        }
                    }
                }
            }
            else
            {
                entry.BackgroundColor = Colors.Red;
                ToolTipProperties.SetText(entry, "Virheellinen päivämäärä. Käytä muotoa pp.kk.vvvv");
            }
        }
    }
    private void LoadDataIntoDictionary()
    {
        //Loads data into the dictionary from the file
        ObservableCollection<Employee> existingEmployees = FileHelper.ReadFromFile() ?? new ObservableCollection<Employee>();
        // Populates the dictionary with postal codes and post offices
        foreach (var employee in existingEmployees)
        {
            if (!string.IsNullOrEmpty(employee.POCode) && !string.IsNullOrEmpty(employee.PostOffice))
            {
                // Adds or updates postal code and post office in the dictionary
                postalCodeToPostOffice[employee.POCode] = employee.PostOffice;
            }
        }
    }
    private void EntryPostalCodeTextChanged(object sender, TextChangedEventArgs e)
    {
        //handles postal code entry changes
        try
        {
            string enteredPostalCode = e.NewTextValue;

            // Checks if any postal code starts with the entered value
            var matchingPostalCodes = postalCodeToPostOffice.Keys
                .Where(code => code.StartsWith(enteredPostalCode))
                .ToList();

            if (matchingPostalCodes.Count == 1)
            {
                // If there is exactly one matching postal code, sets the post office field
                entryPostOffice.Text = postalCodeToPostOffice[matchingPostalCodes[0]];
            }
            else
            {
                entryPostOffice.Text = string.Empty; // Clear the post office field if no or multiple suggestions are found
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"EntryPostalCodeTextChanged failed: {ex.Message}");
        }
    }
    protected override bool OnBackButtonPressed()
    {
        //Modifys action when backbutton is pressed
        if (!leaveWithoutSaving)
        {
            AskToSaveChanges();
            return true; // Stops going back before confirming user
        }

        return base.OnBackButtonPressed();
    }
    private async void AskToSaveChanges()
    {
        // Confirming user that wants to go back without saving
        var result = await DisplayAlert("Poistuminen", "Haluatko poistua tallentamatta tietoja?", "Kyllä", "Ei");

        if (result)//User picked "Kyllä"
        {
            //Sets leaveWithaoutSaving as true, writes logdetail, and moves to HRSystemPage
            leaveWithoutSaving = true;
            Log.LogDetail($"Poistuttu tallentamatta tietoja");
            await Navigation.PushAsync(new HRSystemPage());
        }
    }
}