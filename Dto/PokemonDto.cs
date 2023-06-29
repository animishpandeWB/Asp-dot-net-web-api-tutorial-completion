namespace PokemonReviewApp.Dto
{
    //DTO STANDS FOR DATA TRANSFER OBJECTS
    //A DTO (Data Transfer Object) is an object that defines how data will be sent between applications.
    //It’s used only to send and receive data and does not contain in itself any business logic.
    public class PokemonDto
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
    }
}
