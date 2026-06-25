using System.Diagnostics;

namespace final_work
{
    public class SSNCheck
    {
        //Class for checking Social Security Number
        public string CheckSSN(string ssn)
        {
            //check if SSN lenght is correct
            if (string.IsNullOrEmpty(ssn) || ssn.Length != 11)
            {
                Debug.WriteLine("Henkilötunnuksessa on virheellinen pituus");
                return "virheellinen pituus";
            }
            string ddmmyyyy = ssn.Substring(0, 6);
            string nnn = ssn.Substring(7, 3);
            char T = ssn[10];

            int.TryParse(ddmmyyyy, out int ddmmyyyyNumber);
            int.TryParse(nnn, out int nnnNumber);

            int jj = ddmmyyyyNumber * 1000 + nnnNumber;
            jj %= 31;

            string checkMark = "0123456789ABCDEFHJKLMNPRSTUVWXY";
            char rightCheckMark = checkMark[jj];

            //checks is the check mark right or wrong
            if (T == rightCheckMark)
            {
                return "oikein";
            }
            else
            {
                Debug.WriteLine($"hetu oli annettu väärin, oikea tarkiste olisi pitänyt olla {rightCheckMark}");
                return "väärin";
            }
        }
    }
}