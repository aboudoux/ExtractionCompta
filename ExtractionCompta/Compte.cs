using System.Text.RegularExpressions;
using ExtractionCompta.Exceptions;

namespace ExtractionCompta
{
    public class Compte
    {
        public const string BANQUE = "512000";
        public const string TVA = "445660";
        public const string TVA_COLLECTEE = "445710";
        public const string COMPTE_COURANT = "455000";


        public Compte(string valeur)
        {            
            if( string.IsNullOrWhiteSpace(valeur) )
                throw new NumeroCompteInvalideException("vide");
            if(!Regex.IsMatch(valeur,"^[0-9]{6}$"))
                throw new NumeroCompteInvalideException(valeur);
            Valeur = valeur;
        }

        public string Valeur {get; }

        public override string ToString()
        {
            return Valeur;
        }
    }
}