namespace hot_chocolate_demo.GraphQL;
public class Query {
  public GetDogsRes GetDogs(DogDataContext dbContext, string? idIn) {

    List<Dog> res = FetchDogs(dbContext, idIn);

    return new GetDogsRes(){
      Data = res,
      TotalResults = res.Count()
    };
  }

  public List<User>? GetUsers(DogDataContext dbContext) {
    return dbContext?.user?.ToList();
  }

  public GetDogsRes GetUserDogs(DogDataContext dbc, int idIn, int page = 0, int pageSize = 10) {
    var currentUser = dbc?.user?.First(user => user.Id == idIn);
    var dogs = currentUser?.GetDogs(dbc!, currentUser, page, pageSize);
    var returnObj = new GetDogsRes() { Data = dogs, TotalResults = currentUser?.DogsIdList?.Count() };
    return returnObj;
  }

  public User? GetUserById(DogDataContext dbContext, int? idIn) {
    if (!idIn.HasValue) {
      throw new ArgumentException("idIn is a required parameter!");
    }
    return dbContext.user?.Where(el => el.Id == idIn).ToList().First<User>();
  }

  private List<Dog> FetchDogs(DogDataContext dbContext, string? idIn) {
    List<Dog>? dbRes = new List<Dog>();
    try {
      dbRes = dbContext?.dog?.ToList();
    } catch(Exception error) {
      Console.WriteLine(error);
    }

    List<Dog> payload = new List<Dog>();
    payload.AddRange(dbRes!);

    return payload;
  }
}