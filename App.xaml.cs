
namespace final_work;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
    protected override void OnStart()
    {
        // Logdetail when app is started
        Log.LogDetail("Sovellus käynnistetty");
    } 
}

