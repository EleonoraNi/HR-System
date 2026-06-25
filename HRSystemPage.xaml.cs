using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;


namespace final_work;

public partial class HRSystemPage : ContentPage
{
    ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
    private string currentSortField = "NickName";
    private bool isSortAscending = true;
    private Employee selectedEmployee;
    private bool logOut = false;
    public HRSystemPage()
	{
		InitializeComponent();
        EmploymentsLV.ItemSelected += OnItemSelected;
        employees = FileHelper.ReadFromFile();
        EmploymentsLV.ItemsSource = employees;
    }
    private void OnAddClicked(object sender, EventArgs e)
    {
        //Writes logdetail and moves user to Management page
        Log.LogDetail($"Henkilöä lisätään");
        Navigation.PushAsync(new ManagementPage(null));
    }
    private async void OnModifyClicked(object sender, EventArgs e)
    {
        //Check that there is employee selected 
        if (selectedEmployee == null)
        {
            await DisplayAlert("Virhe", "Valitse henkilö ennen muokkaamista.", "OK");
            return;
        }
        //confirms that user wants to modify selected employee
        bool result = await DisplayAlert("Vahvista muokkaus", $"Haluatko muokata valittua henkilöä?\nHenkilö: {selectedEmployee.FirstNames} {selectedEmployee.LastName}", "Kyllä", "Ei");

        if (result)//User picked "Kyllä"
        {
            // Selected employee is deleted, file is updated with eployees left,
            // writes logdetail and continues to Management Page with selected employee details
            employees.Remove(selectedEmployee);
            FileHelper.SaveToFile(employees);
            Log.LogDetail($"Henkilöä muokataan: {selectedEmployee.FirstNames} {selectedEmployee.LastName}");
            await Navigation.PushAsync(new ManagementPage(selectedEmployee));
        }
        else//User picked "Ei"
        {
            //writes logdetail and alerts user
            Log.LogDetail($"Henkilön muokkaus peruutettu: {selectedEmployee.FirstNames} {selectedEmployee.LastName}");
            await DisplayAlert("Peruutettu", "Henkilön muokkaus peruutettu.", "OK");
        }       
    }
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        //Check that there is employee selected
        if (selectedEmployee == null)
        {
            await DisplayAlert("Virhe", "Valitse henkilö ennen poistamista.", "OK");
            return;
        }
        //confirms that user wants to delete selected employee
        bool result = await DisplayAlert("Vahvista poisto", $"Haluatko varmasti poistaa valitun henkilön?\nHenkilö: { selectedEmployee.FirstNames} { selectedEmployee.LastName}", "Kyllä", "Ei");
        
        if (result)//User picked "Kyllä"
        {
            // Selected employee is deleted, file is updated with eployees left,
            // writes logdetail and continues to Management Page with selected employee details
            employees.Remove(selectedEmployee);
            FileHelper.SaveToFile(employees);
            Log.LogDetail($"Henkilö poistettu: {selectedEmployee.FirstNames} {selectedEmployee.LastName}");
            await DisplayAlert("Poisto onnistui", $"Henkilö {selectedEmployee.FirstNames} {selectedEmployee.LastName} poistettu onnistuneesti.", "OK");
        }
        else//User picked "Ei"
        {
            //writes logdetail and alerts user
            Log.LogDetail($"Henkilön poisto peruutettu: {selectedEmployee.FirstNames} {selectedEmployee.LastName}");
            await DisplayAlert("Peruutettu", "Henkilön poisto peruutettu.", "OK");
        }
    }
    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        //Gets which employee is selected from the list
        EmploymentsLV.SelectedItem = null;//Clears selection
        if (e.SelectedItem == null)
        {
            return;
        }
        selectedEmployee = e.SelectedItem as Employee;
    }
    private void OnTapGestureRecognizerTapped(object sender, TappedEventArgs e)
    {
        // Toggles the sort order
        isSortAscending = !isSortAscending;
        // Gets the tapped Label and writes logdetail
        Label tappedLabel = (Label)sender;
        Log.LogDetail($"Listaa järjestetty {tappedLabel.Text} -otsikon mukaan");
        // Sets the current sort field based on the tapped Label (Sukunimi, Kutsumani tai Nimike)
        switch (tappedLabel.Text)
        {
            case "Sukunimi":
                currentSortField = "LastName";
                break;
            case "Kutsumanimi":
                currentSortField = "NickName";
                break;
            case "Nimike":
                currentSortField = "Title";
                break;
        }
        // Sorts the data based on the current sort field
        switch (currentSortField)
        {
            case "NickName":
                employees = new ObservableCollection<Employee>(isSortAscending ? employees.OrderBy(emp => emp.NickName, StringComparer.OrdinalIgnoreCase) : employees.OrderByDescending(emp => emp.NickName, StringComparer.OrdinalIgnoreCase));
                break;
            case "LastName":
                employees = new ObservableCollection<Employee>(isSortAscending ? employees.OrderBy(emp => emp.LastName, StringComparer.OrdinalIgnoreCase) : employees.OrderByDescending(emp => emp.LastName, StringComparer.OrdinalIgnoreCase));
                break;
            case "Title":
                employees = new ObservableCollection<Employee>(isSortAscending ? employees.OrderBy(emp => emp.Title, StringComparer.OrdinalIgnoreCase) : employees.OrderByDescending(emp => emp.Title, StringComparer.OrdinalIgnoreCase));
                break;
        }
        EmploymentsLV.ItemsSource = employees;
    }
    protected override bool OnBackButtonPressed()
    {
        //Modifys action when backbutton is pressed
        if (!logOut)
        {
            AskToLogOut();
            return true; // Stops going back before confirming user
        }
        return base.OnBackButtonPressed();
    }
    private async void AskToLogOut()
    {
        //Confirming user logging out
        var result = await DisplayAlert("Kirjaudu ulos", "Haluatko kirjautua ulos?", "Kyllä", "Ei");

        if (result)//User picked "Kyllä"
        {
            //Sets logOut as true, writes logdetail, and moves to MainPage
            logOut = true;
            Log.LogDetail($"Kirjauduttu ulos");
            await Navigation.PushAsync(new MainPage());
        }
    }
}