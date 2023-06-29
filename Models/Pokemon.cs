namespace PokemonReviewApp.Models
{
    //CREATING A MODEL (ALSO CALLED AS POCO - PLAIN OLD CSHARP OBJECT)
    //A MODEL IS BASICALLY A CLASS WITH A BUNCH OF PROPERTIES. IT IS JUST A REPRESENTATIVE OF DATA IN DATABASES
    public class Pokemon
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        } 
        public DateTime BirthDate
        {
            get; set;
        }
        public ICollection<Review> Reviews{ get; set; }
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
