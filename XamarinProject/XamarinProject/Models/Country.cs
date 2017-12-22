using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinProject.Models
{
    public class Currency
    {
        public string code { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
    }

    public class Language
    {
        public string iso639_1 { get; set; }
        public string iso639_2 { get; set; }
        public string name { get; set; }
        public string nativeName { get; set; }
    }

    public class Translations
    {
        public string de { get; set; }
        public string es { get; set; }
        public string fr { get; set; }
        public string ja { get; set; }
        public string it { get; set; }
        public string br { get; set; }
        public string pt { get; set; }
        public string nl { get; set; }
        public string hr { get; set; }
        public string fa { get; set; }
    }

    public class RegionalBloc
    {
        public string acronym { get; set; }
        public string name { get; set; }
        public List<object> otherAcronyms { get; set; }
        public List<object> otherNames { get; set; }
    }

    public class Country
    {
        public string name { get; set; }
        public List<string> topLevelDomain { get; set; }
        public string alpha2Code { get; set; }
        public string alpha3Code { get; set; }
        public List<string> callingCodes { get; set; }
        public string capital { get; set; }
        public List<string> altSpellings { get; set; }
        public string region { get; set; }
        public string subregion { get; set; }
        public string population { get; set; }
        public List<string> latlng { get; set; }
        public string demonym { get; set; }
        public string area { get; set; }
        public double? gini { get; set; }
        public List<string> timezones { get; set; }
        public List<string> borders { get; set; }
        public string nativeName { get; set; }
        public string numericCode { get; set; }
        public List<Currency> currencies { get; set; }
        public List<Language> languages { get; set; }
        public Translations translations { get; set; }
        public string flag { get; set; }
        public List<RegionalBloc> regionalBlocs { get; set; }
        public string cioc { get; set; }
    }
}
