using System.Diagnostics;

namespace final_work;

public partial class MainPage : ContentPage
{
    private Log loggedInUser;
    public MainPage()
	{
		InitializeComponent();
	}
    private async void OnLogInClicked(object sender, EventArgs e)
    {
        //includes lod in button actions
        try
        {
            string enteredUsername = userName.Text;
            string enteredPassword = password.Text;

            // Check that username and password are correct as defined in assingment
            if (enteredUsername == null)
            {
                ToolTipProperties.SetText(userName, "Syötä käyttäjätunnus ja salasana");
                ToolTipProperties.SetText(password, "Syötä käyttäjätunnus ja salasana");
                await DisplayAlert("Virhe", "Virheellinen käyttäjätunnus tai salasana. Yritä uudelleen.", "OK");
            }
            else
            {
                if (enteredUsername == enteredPassword)
                {
                    try
                    {
                        // Saves username for logdetails, writes logdetail, makes entrys empty, and continues to next page
                        Log.UserName = enteredUsername;
                        userName.Text = string.Empty;
                        password.Text = string.Empty;
                        Log.LogDetail($"Käyttäjä kirjautunut");
                        await Navigation.PushAsync(new HRSystemPage());
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Virhe Navigation.PushAsync:ssa: " + ex.Message);
                    }
                }
                else
                {
                    //Username or password isn't correct, emptys entrys, sends logdetail and alerts user
                    userName.Text = string.Empty;
                    password.Text = string.Empty;
                    Log.LogDetail($"Kirjautuminen epäonnistunut");
                    await DisplayAlert("Virhe", "Virheellinen käyttäjätunnus tai salasana. Yritä uudelleen.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"OnLogInClicked failed: {ex.Message}");
        }
    }
}

