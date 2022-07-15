namespace hot_chocolate_demo.GraphQL;
public class Query {
  private static readonly HttpClient client = new HttpClient();
  public List<Dog> GetDogs(DogDataContext dbContext, string? idIn) {

    List<Dog> res = FetchDogs(dbContext, idIn);

    return res;
  }

  public List<User>? GetUsers(DogDataContext dbContext) {
    return dbContext?.user?.ToList();
  }

  public User? GetUserById(DogDataContext dbContext, int? idIn) {
    if (!idIn.HasValue) {
      throw new ArgumentException("idIn is a required parameter!");
    }
    return dbContext.user?.Where(el => el.Id == idIn).ToList().First<User>();
  }

  private List<Dog> FetchDogs(DogDataContext dbContext, string? idIn) {
    // var res = await client.GetStreamAsync("https://api.thedogapi.com/v1/images/search?limit=10");

    List<Dog>? dbRes = new List<Dog>();
    try {
      dbRes = dbContext?.dog?.ToList();
      Console.WriteLine("here");
    } catch(Exception error) {
      Console.WriteLine(error);
    }

    List<Dog> payload = new List<Dog>();
    payload.AddRange(dbRes);

    return payload;
  }
}
